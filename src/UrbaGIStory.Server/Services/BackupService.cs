using System.Diagnostics;
using System.Text;
using Npgsql;
using UrbaGIStory.Server.DTOs.Requests;
using UrbaGIStory.Server.DTOs.Responses;
using UrbaGIStory.Server.Exceptions;

namespace UrbaGIStory.Server.Services;

/// <summary>
/// Service for database backup and restore operations.
/// </summary>
public class BackupService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<BackupService> _logger;
    private readonly string _backupDirectory;
    private readonly string _backupMainDirectory;
    private readonly bool _autoBackupBeforeRestore;

    public BackupService(IConfiguration configuration, ILogger<BackupService> logger)
    {
        _configuration = configuration;
        _logger = logger;

        var backupSettings = _configuration.GetSection("BackupSettings");
        var backupDir = backupSettings["BackupDirectory"] ?? "backups";
        _backupMainDirectory = backupSettings["BackupMainDirectory"] ?? "";
        _backupDirectory = Path.Combine(_backupMainDirectory, backupDir);
        _autoBackupBeforeRestore = backupSettings.GetValue<bool>("AutoBackupBeforeRestore", true);

        // Ensure backup directory exists
        if (!Directory.Exists(_backupDirectory))
        {
            Directory.CreateDirectory(_backupDirectory);
            _logger.LogInformation("Created backup directory: {BackupDirectory}", _backupDirectory);
        }
    }

    /// <summary>
    /// Creates a backup of the database.
    /// </summary>
    public async Task<BackupResponse> CreateBackupAsync()
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection")
                                   ?? throw new InvalidOperationException("DefaultConnection string is not configured");

            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var filename = $"UrbaGIStory_backup_{timestamp}.dump";
            var filePath = Path.Combine(_backupDirectory, filename);
            ;

            if (!String.IsNullOrWhiteSpace(_backupMainDirectory))
            {
                filePath = Path.Combine(_backupMainDirectory, _backupDirectory, filename);
            }

            // Build pg_dump command
            var pgDumpPath = FindPostgresTool("pg_dump");
            if (string.IsNullOrEmpty(pgDumpPath))
            {
                throw new InvalidOperationException(
                    "pg_dump not found. Please ensure PostgreSQL tools are installed and in PATH.");
            }

            var arguments = new StringBuilder();
            arguments.Append($"-h {builder.Host}");
            arguments.Append($" -p {builder.Port}");
            arguments.Append($" -U {builder.Username}");
            arguments.Append($" -d {builder.Database}");
            arguments.Append($" -F c"); // Custom format (compressed)
            arguments.Append($" -f \"{filePath}\"");

            var processStartInfo = new ProcessStartInfo
            {
                FileName = pgDumpPath,
                Arguments = arguments.ToString(),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                Environment = { ["PGPASSWORD"] = builder.Password ?? string.Empty }
            };

            _logger.LogInformation("Starting database backup to: {FilePath}", filePath);

            using var process = Process.Start(processStartInfo);
            if (process == null)
            {
                throw new InvalidOperationException("Failed to start pg_dump process");
            }

            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                _logger.LogError("pg_dump failed with exit code {ExitCode}. Error: {Error}",
                    process.ExitCode, error);
                throw new InvalidOperationException($"Backup failed: {error}");
            }

            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists || fileInfo.Length == 0)
            {
                throw new InvalidOperationException("Backup file was not created or is empty");
            }

            _logger.LogInformation("Backup created successfully: {FilePath} ({Size} bytes)",
                filePath, fileInfo.Length);

            return new BackupResponse
            {
                BackupId = Path.GetFileNameWithoutExtension(filename),
                Filename = filename,
                FilePath = filePath,
                SizeBytes = fileInfo.Length,
                CreatedAt = fileInfo.CreationTimeUtc,
                Message = "Backup created successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating database backup");
            throw;
        }
    }

    /// <summary>
    /// Lists all available backups.
    /// </summary>
    public BackupListResponse ListBackups()
    {
        try
        {
            if (!Directory.Exists(Path.Combine(_backupDirectory)))
            {
                return new BackupListResponse { Backups = new List<BackupInfo>(), TotalCount = 0 };
            }

            var backupFiles = Directory.GetFiles(Path.Combine(_backupDirectory), "UrbaGIStory_backup_*.dump")
                .Select(filePath =>
                {
                    var fileInfo = new FileInfo(filePath);
                    var filename = Path.GetFileName(filePath);
                    var backupId = Path.GetFileNameWithoutExtension(filename);

                    return new BackupInfo
                    {
                        BackupId = backupId,
                        Filename = filename,
                        SizeBytes = fileInfo.Length,
                        SizeFormatted = FormatFileSize(fileInfo.Length),
                        CreatedAt = fileInfo.CreationTimeUtc
                    };
                })
                .OrderByDescending(b => b.CreatedAt)
                .ToList();

            return new BackupListResponse
            {
                Backups = backupFiles,
                TotalCount = backupFiles.Count
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing backups");
            throw;
        }
    }

    /// <summary>
    /// Restores a database from a backup file.
    /// </summary>
    public async Task RestoreBackupAsync(RestoreRequest request)
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection")
                                   ?? throw new InvalidOperationException("DefaultConnection string is not configured");

            var builder = new NpgsqlConnectionStringBuilder(connectionString);

            // Find backup file
            var backupFileRaw = Directory.GetFiles(Path.Combine(_backupDirectory), "UrbaGIStory_backup_*.dump");

            var backupFile = backupFileRaw
                .FirstOrDefault(f => Path.GetFileNameWithoutExtension(f) == request.BackupId);

            string? name = Path.GetFileNameWithoutExtension(backupFileRaw[0]);

            if (backupFile == null || !File.Exists(backupFile))
            {
                throw new EntityNotFoundException($"Backup file not found: {request.BackupId}");
            }

            // Validate backup file
            var fileInfo = new FileInfo(backupFile);
            if (fileInfo.Length == 0)
            {
                throw new ValidationException("Backup file is empty");
            }

            // Create automatic backup before restore (safety measure)
            if (request.CreateBackupBeforeRestore && _autoBackupBeforeRestore)
            {
                _logger.LogInformation("Creating automatic backup before restore...");
                var preRestoreBackup = await CreateBackupAsync();
                _logger.LogInformation("Pre-restore backup created: {BackupId}", preRestoreBackup.BackupId);
            }

            // Build pg_restore command
            var pgRestorePath = FindPostgresTool("pg_restore");
            if (string.IsNullOrEmpty(pgRestorePath))
            {
                throw new InvalidOperationException(
                    "pg_restore not found. Please ensure PostgreSQL tools are installed and in PATH.");
            }

            var arguments = new StringBuilder();
            arguments.Append($"-h {builder.Host}");
            arguments.Append($" -p {builder.Port}");
            arguments.Append($" -U {builder.Username}");
            arguments.Append($" -d {builder.Database}");
            arguments.Append($" -c"); // Clean (drop) database objects before recreating
            arguments.Append($" -v"); // Verbose
            arguments.Append($" \"{backupFile}\"");

            var processStartInfo = new ProcessStartInfo
            {
                FileName = pgRestorePath,
                Arguments = arguments.ToString(),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                Environment = { ["PGPASSWORD"] = builder.Password ?? string.Empty }
            };

            _logger.LogInformation("Starting database restore from: {BackupFile}", backupFile);

            using var process = Process.Start(processStartInfo);
            if (process == null)
            {
                throw new InvalidOperationException("Failed to start pg_restore process");
            }

            // Iniciar lectura asíncrona de streams
            var outputTask = process.StandardOutput.ReadToEndAsync();
            var errorTask = process.StandardError.ReadToEndAsync();

            // Esperar a que el proceso termine (esto permite que los streams se llenen)
            await process.WaitForExitAsync();

            // Ahora leer los resultados (ya están completos)
            var output = await outputTask;
            var error = await errorTask;
            
            
            if (process.ExitCode != 0)
            {
                _logger.LogError("pg_restore failed with exit code {ExitCode}. Error: {Error}",
                    process.ExitCode, error);
                throw new InvalidOperationException($"Restore failed: {error}");
            }

            _logger.LogInformation("Database restored successfully from: {BackupFile}", backupFile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restoring database backup");
            throw;
        }
    }

    /// <summary>
    /// Finds PostgreSQL tool executable in PATH or common installation locations.
    /// </summary>
    private string? FindPostgresTool(string toolName)
    {
        // Check if tool is in PATH
        var pathEnv = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
        var paths = pathEnv.Split(Path.PathSeparator);

        foreach (var path in paths)
        {
            var fullPath = Path.Combine(path, toolName);
            if (File.Exists(fullPath))
            {
                return fullPath;
            }

            // On Windows, try with .exe extension
            var exePath = fullPath + ".exe";
            if (File.Exists(exePath))
            {
                return exePath;
            }
        }

        // Common PostgreSQL installation locations (Windows)
        if (OperatingSystem.IsWindows())
        {
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var commonLocations = new[]
            {
                Path.Combine(programFiles, "PostgreSQL", "16", "bin", $"{toolName}.exe"),
                Path.Combine(programFiles, "PostgreSQL", "15", "bin", $"{toolName}.exe"),
                Path.Combine(programFiles, "PostgreSQL", "14", "bin", $"{toolName}.exe"),
                Path.Combine(programFiles, "PostgreSQL", "13", "bin", $"{toolName}.exe"),
                Path.Combine(programFiles, "PostgreSQL", "12", "bin", $"{toolName}.exe"),
            };

            foreach (var location in commonLocations)
            {
                if (File.Exists(location))
                {
                    return location;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Formats file size in human-readable format.
    /// </summary>
    private static string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }
}