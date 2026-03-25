using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomusMind.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAreaIdToWorkItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "area_id",
                table: "routines",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "area_id",
                table: "household_tasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "area_id",
                table: "calendar_events",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "area_id",
                table: "routines");

            migrationBuilder.DropColumn(
                name: "area_id",
                table: "household_tasks");

            migrationBuilder.DropColumn(
                name: "area_id",
                table: "calendar_events");
        }
    }
}
