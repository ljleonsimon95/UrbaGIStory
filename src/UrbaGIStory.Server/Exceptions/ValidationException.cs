namespace UrbaGIStory.Server.Exceptions;

/// <summary>
/// Exception thrown when validation fails.
/// </summary>
public class ValidationException : Exception
{
    public ValidationException()
        : base("Validation failed")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(string message)
        : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(string message, Dictionary<string, string[]> errors)
        : base(message)
    {
        Errors = errors ?? new Dictionary<string, string[]>();
    }

    public ValidationException(Dictionary<string, string[]> errors)
        : base("Validation failed")
    {
        Errors = errors ?? new Dictionary<string, string[]>();
    }

    /// <summary>
    /// Dictionary of validation errors, keyed by field name.
    /// </summary>
    public Dictionary<string, string[]> Errors { get; }
}

