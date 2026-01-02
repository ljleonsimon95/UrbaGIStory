using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbaGIStory.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            // Create PostGIS extension
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS postgis;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
