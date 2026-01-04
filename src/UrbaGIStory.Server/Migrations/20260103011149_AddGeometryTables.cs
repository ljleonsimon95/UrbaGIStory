using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace UrbaGIStory.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddGeometryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Entities_QGISGeometryId",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "QGISGeometryId",
                table: "Entities");

            migrationBuilder.AddColumn<Guid>(
                name: "GeoLineId",
                table: "Entities",
                type: "uuid",
                nullable: true,
                comment: "Optional link to a line geometry. Only ONE geometry FK can be set.");

            migrationBuilder.AddColumn<Guid>(
                name: "GeoPointId",
                table: "Entities",
                type: "uuid",
                nullable: true,
                comment: "Optional link to a point geometry. Only ONE geometry FK can be set.");

            migrationBuilder.AddColumn<Guid>(
                name: "GeoPolygonId",
                table: "Entities",
                type: "uuid",
                nullable: true,
                comment: "Optional link to a polygon geometry. Only ONE geometry FK can be set.");

            migrationBuilder.CreateTable(
                name: "geo_lines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()", comment: "Unique identifier for the line geometry."),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Name of the geometry (visible in QGIS)."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional description of the geometry."),
                    Geometry = table.Column<LineString>(type: "geometry(LineString, 4326)", nullable: true, comment: "LineString geometry in WGS84 (EPSG:4326)."),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()", comment: "Date and time when the geometry was created (UTC)."),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the geometry was last updated (UTC)."),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Whether the geometry is deleted (soft delete)."),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the geometry was deleted (null if not deleted).")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geo_lines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "geo_points",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()", comment: "Unique identifier for the point geometry."),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Name of the geometry (visible in QGIS)."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional description of the geometry."),
                    Geometry = table.Column<Point>(type: "geometry(Point, 4326)", nullable: true, comment: "Point geometry in WGS84 (EPSG:4326)."),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()", comment: "Date and time when the geometry was created (UTC)."),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the geometry was last updated (UTC)."),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Whether the geometry is deleted (soft delete)."),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the geometry was deleted (null if not deleted).")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geo_points", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "geo_polygons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()", comment: "Unique identifier for the polygon geometry."),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Name of the geometry (visible in QGIS)."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional description of the geometry."),
                    Geometry = table.Column<Polygon>(type: "geometry(Polygon, 4326)", nullable: true, comment: "Polygon geometry in WGS84 (EPSG:4326)."),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()", comment: "Date and time when the geometry was created (UTC)."),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the geometry was last updated (UTC)."),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Whether the geometry is deleted (soft delete)."),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the geometry was deleted (null if not deleted).")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geo_polygons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entities_GeoLineId",
                table: "Entities",
                column: "GeoLineId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_GeoPointId",
                table: "Entities",
                column: "GeoPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_GeoPolygonId",
                table: "Entities",
                column: "GeoPolygonId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Entities_SingleGeometry",
                table: "Entities",
                sql: "(\r\n                (CASE WHEN \"GeoPointId\" IS NOT NULL THEN 1 ELSE 0 END) +\r\n                (CASE WHEN \"GeoLineId\" IS NOT NULL THEN 1 ELSE 0 END) +\r\n                (CASE WHEN \"GeoPolygonId\" IS NOT NULL THEN 1 ELSE 0 END)\r\n            ) <= 1");

            migrationBuilder.CreateIndex(
                name: "IX_geo_lines_Geometry",
                table: "geo_lines",
                column: "Geometry")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "IX_geo_lines_IsDeleted",
                table: "geo_lines",
                column: "IsDeleted",
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_geo_points_Geometry",
                table: "geo_points",
                column: "Geometry")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "IX_geo_points_IsDeleted",
                table: "geo_points",
                column: "IsDeleted",
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_geo_polygons_Geometry",
                table: "geo_polygons",
                column: "Geometry")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "IX_geo_polygons_IsDeleted",
                table: "geo_polygons",
                column: "IsDeleted",
                filter: "\"IsDeleted\" = false");

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_geo_lines_GeoLineId",
                table: "Entities",
                column: "GeoLineId",
                principalTable: "geo_lines",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_geo_points_GeoPointId",
                table: "Entities",
                column: "GeoPointId",
                principalTable: "geo_points",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_geo_polygons_GeoPolygonId",
                table: "Entities",
                column: "GeoPolygonId",
                principalTable: "geo_polygons",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entities_geo_lines_GeoLineId",
                table: "Entities");

            migrationBuilder.DropForeignKey(
                name: "FK_Entities_geo_points_GeoPointId",
                table: "Entities");

            migrationBuilder.DropForeignKey(
                name: "FK_Entities_geo_polygons_GeoPolygonId",
                table: "Entities");

            migrationBuilder.DropTable(
                name: "geo_lines");

            migrationBuilder.DropTable(
                name: "geo_points");

            migrationBuilder.DropTable(
                name: "geo_polygons");

            migrationBuilder.DropIndex(
                name: "IX_Entities_GeoLineId",
                table: "Entities");

            migrationBuilder.DropIndex(
                name: "IX_Entities_GeoPointId",
                table: "Entities");

            migrationBuilder.DropIndex(
                name: "IX_Entities_GeoPolygonId",
                table: "Entities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Entities_SingleGeometry",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "GeoLineId",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "GeoPointId",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "GeoPolygonId",
                table: "Entities");

            migrationBuilder.AddColumn<Guid>(
                name: "QGISGeometryId",
                table: "Entities",
                type: "uuid",
                nullable: true,
                comment: "Optional link to QGIS geometry. Null if entity has no spatial representation.");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_QGISGeometryId",
                table: "Entities",
                column: "QGISGeometryId");
        }
    }
}
