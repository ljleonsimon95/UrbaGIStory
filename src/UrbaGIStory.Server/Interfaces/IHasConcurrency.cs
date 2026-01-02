namespace UrbaGIStory.Server.Interfaces;

/// <summary>
/// Interface for entities that support optimistic concurrency control.
/// Entities implementing this interface will have version-based conflict detection.
/// </summary>
public interface IHasConcurrency
{
    /// <summary>
    /// Row version used for optimistic concurrency control.
    /// This value is automatically updated by the database on each update.
    /// </summary>
    byte[] RowVersion { get; set; }
}


