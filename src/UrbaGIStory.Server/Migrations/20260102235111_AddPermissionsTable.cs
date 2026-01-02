using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbaGIStory.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique identifier for the permission."),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "ID of the user who has this permission."),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false, comment: "ID of the entity this permission applies to."),
                    CanRead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Whether the user can read/view the entity."),
                    CanWrite = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Whether the user can write/edit the entity."),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the permission was created (UTC)."),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false, comment: "ID of the Office Manager who created this permission."),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the permission was last updated (UTC)."),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "ID of the Office Manager who last updated this permission.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_EntityId",
                table: "Permissions",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserId_EntityId",
                table: "Permissions",
                columns: new[] { "UserId", "EntityId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permissions");
        }
    }
}
