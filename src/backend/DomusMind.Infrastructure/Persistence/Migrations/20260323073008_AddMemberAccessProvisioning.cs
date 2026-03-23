using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomusMind.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberAccessProvisioning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "auth_users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "auth_users",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "auth_users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "MemberId",
                table: "auth_users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordChangedAtUtc",
                table: "auth_users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "auth_users");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "auth_users");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "auth_users");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "auth_users");

            migrationBuilder.DropColumn(
                name: "PasswordChangedAtUtc",
                table: "auth_users");
        }
    }
}
