namespace UrbaGIStory.Server.Exceptions;

/// <summary>
/// Exception thrown when an entity is not found in the database.
/// </summary>
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException()
        : base("Entity not found")
    {
    }

    public EntityNotFoundException(string message)
        : base(message)
    {
    }

    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public EntityNotFoundException(string entityType, object id)
        : base($"{entityType} with id '{id}' was not found")
    {
        EntityType = entityType;
        Id = id;
    }

    public string? EntityType { get; }
    public object? Id { get; }
}

