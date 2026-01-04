namespace UrbaGIStory.Server.Models;

/// <summary>
/// Enumeration of entity types in the urban planning system.
/// </summary>
public enum EntityType
{
    /// <summary>
    /// Building or property (inmueble/edificio).
    /// </summary>
    Building = 1,

    /// <summary>
    /// City block (manzana).
    /// </summary>
    Block = 2,

    /// <summary>
    /// Street (calle).
    /// </summary>
    Street = 3,

    /// <summary>
    /// Boulevard (bulevar).
    /// </summary>
    Boulevard = 4,

    /// <summary>
    /// Facade line (línea de fachada).
    /// </summary>
    FacadeLine = 5,

    /// <summary>
    /// Plaza or public square (plaza).
    /// </summary>
    Plaza = 6,

    /// <summary>
    /// District or zone (zona/distrito).
    /// </summary>
    District = 7,

    /// <summary>
    /// Other entity type.
    /// </summary>
    Other = 99
}

