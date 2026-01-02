namespace UrbaGIStory.Server.Exceptions;

/// <summary>
/// Exception thrown when a concurrency conflict is detected during an update operation.
/// This occurs when the entity being updated has been modified by another user since it was last read.
/// </summary>
public class ConcurrencyConflictException : Exception
{
    public ConcurrencyConflictException()
        : base("The entity has been modified by another user. Please refresh and try again.")
    {
    }

    public ConcurrencyConflictException(string message)
        : base(message)
    {
    }

    public ConcurrencyConflictException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public ConcurrencyConflictException(string entityType, object id)
        : base($"{entityType} with id '{id}' has been modified by another user. Please refresh and try again.")
    {
        EntityType = entityType;
        Id = id;
    }

    /// <summary>
    /// The type of entity that had a concurrency conflict.
    /// </summary>
    public string? EntityType { get; }

    /// <summary>
    /// The ID of the entity that had a concurrency conflict.
    /// </summary>
    public object? Id { get; }
}


