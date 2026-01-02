using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using Serilog.Events;
using UrbaGIStory.Server.Data;
using UrbaGIStory.Server.Identity;
using UrbaGIStory.Server.Middleware;
using UrbaGIStory.Server.Services;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/urbagistory-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        fileSizeLimitBytes: 10 * 1024 * 1024, // 10 MB
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{ThreadId}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Use Serilog for logging
builder.Host.UseSerilog();

var configuration = builder.Configuration;

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Authorization
builder.Services.AddAuthorization(options =>
{
    // Configure role-based authorization
    options.AddPolicy("TechnicalAdministrator", policy => 
        policy.RequireRole("TechnicalAdministrator"));
    options.AddPolicy("OfficeManager", policy => 
        policy.RequireRole("OfficeManager"));
    options.AddPolicy("Specialist", policy => 
        policy.RequireRole("Specialist"));
});
builder.Services.AddSwaggerGen(options =>
{
    // API Information
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "UrbaGIStory API",
        Version = "v1",
        Description = "API for UrbaGIStory - Urban Geographic Information System"
    });

    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // JWT Bearer authentication configuration for Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(doc => new Microsoft.OpenApi.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        }
    });
});

// Configure EF Core with PostgreSQL and PostGIS
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions => npgsqlOptions.UseNetTopologySuite()
    );
    
    // Enable query logging for performance monitoring (only in development)
    // Note: Detailed query logging is handled by Serilog configuration
});

// Configure ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Password settings (using Identity defaults)
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    
    // User settings
    options.User.RequireUniqueEmail = true;
    
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
var jwtSettings = configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero, // Remove delay of token expiration
        // Map the "role" claim from JWT to ClaimTypes.Role for authorization
        RoleClaimType = "role",
        // Also set NameClaimType for consistency
        NameClaimType = System.Security.Claims.ClaimTypes.Name
    };

    // Add event handlers for debugging
    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();
            
            var claims = context.Principal?.Claims.Select(c => $"{c.Type}={c.Value}").ToList() ?? new List<string>();
            var roles = context.Principal?.Claims
                .Where(c => c.Type == "role" || c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList() ?? new List<string>();
            
            logger.LogInformation("JWT Token Validated - Claims: {Claims}", string.Join(", ", claims));
            logger.LogInformation("JWT Token Validated - Roles: {Roles}", string.Join(", ", roles));
            
            // DEBUG: Check if user is in role
            if (context.Principal != null)
            {
                var isInRole = context.Principal.IsInRole("TechnicalAdministrator");
                logger.LogInformation("JWT Token Validated - IsInRole(TechnicalAdministrator): {IsInRole}", isInRole);
                
                // CRITICAL FIX: Create a new ClaimsIdentity with correct RoleClaimType
                // The existing identity has RoleClaimType set incorrectly, so we need to recreate it
                var oldIdentity = context.Principal.Identity as System.Security.Claims.ClaimsIdentity;
                if (oldIdentity != null)
                {
                    // Create new identity with correct RoleClaimType
                    var newIdentity = new System.Security.Claims.ClaimsIdentity(
                        oldIdentity.Claims,
                        oldIdentity.AuthenticationType,
                        oldIdentity.NameClaimType,
                        System.Security.Claims.ClaimTypes.Role); // Set RoleClaimType here
                    
                    // Create new principal with the corrected identity
                    var newPrincipal = new System.Security.Claims.ClaimsPrincipal(newIdentity);
                    context.Principal = newPrincipal;
                    
                    logger.LogInformation("Created new ClaimsIdentity with RoleClaimType: {RoleClaimType}", newIdentity.RoleClaimType);
                    
                    // Verify roles exist
                    var identityRoles = newIdentity.Claims
                        .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                        .Select(c => c.Value)
                        .ToList();
                    logger.LogInformation("Roles found in new identity: {Roles}", string.Join(", ", identityRoles));
                    
                    // Test IsInRole after recreating identity
                    var testIsInRole = newPrincipal.IsInRole("TechnicalAdministrator");
                    logger.LogInformation("After recreating identity - IsInRole(TechnicalAdministrator): {IsInRole}", testIsInRole);
                }
            }
            
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();
            logger.LogError(context.Exception, "JWT Authentication Failed");
            return Task.CompletedTask;
        }
    };
});

// CORS configuration for Blazor WASM client
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorWasmPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:5001", "http://localhost:5000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Register application services
builder.Services.AddScoped<BackupService>();
builder.Services.AddScoped<LogsService>();
builder.Services.AddSingleton<PerformanceMetricsService>();

var app = builder.Build();

// Seed default roles on application startup
using (var scope = app.Services.CreateScope())
{
    await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
    
    // Seed test user in development environment only
    if (app.Environment.IsDevelopment())
    {
        await TestUserSeeder.SeedTestUserAsync(scope.ServiceProvider);
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Request logging middleware - must be early in pipeline
app.UseMiddleware<RequestLoggingMiddleware>();

// Global exception handler middleware - must be early in pipeline
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseCors("BlazorWasmPolicy");

// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting UrbaGIStory API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
