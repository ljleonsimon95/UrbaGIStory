using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbaGIStory.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddEntitiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique identifier for the entity."),
                    EntityType = table.Column<int>(type: "integer", nullable: false, comment: "Type of entity (building, street, plaza, etc.)."),
                    QGISGeometryId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Optional link to QGIS geometry. Null if entity has no spatial representation."),
                    DynamicProperties = table.Column<JsonDocument>(type: "jsonb", nullable: true, comment: "Dynamic properties stored as JSONB. Properties are defined by categories assigned to the entity type."),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false, comment: "Row version used for optimistic concurrency control. Automatically updated by the database on each update."),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Whether the entity is deleted (soft delete)."),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the entity was deleted (null if not deleted)."),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "ID of the user who deleted the entity (null if not deleted)."),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the entity was created (UTC)."),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false, comment: "ID of the user who created the entity."),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the entity was last updated (UTC)."),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "ID of the user who last updated the entity.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entities_CreatedBy",
                table: "Entities",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_DynamicProperties",
                table: "Entities",
                column: "DynamicProperties")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_EntityType",
                table: "Entities",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_IsDeleted",
                table: "Entities",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_QGISGeometryId",
                table: "Entities",
                column: "QGISGeometryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entities");
        }
    }
}
