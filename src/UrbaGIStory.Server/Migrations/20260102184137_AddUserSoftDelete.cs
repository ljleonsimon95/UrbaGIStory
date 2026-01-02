using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbaGIStory.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Date and time when the user was deactivated (null if active).");

            migrationBuilder.AddColumn<Guid>(
                name: "DeactivatedBy",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true,
                comment: "ID of the administrator who deactivated the user (null if active).");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether the user is active (not deactivated). When false, the user cannot log in.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeactivatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeactivatedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");
        }
    }
}
