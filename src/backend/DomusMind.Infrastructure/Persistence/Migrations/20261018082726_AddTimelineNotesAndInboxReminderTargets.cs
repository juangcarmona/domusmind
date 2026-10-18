using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomusMind.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTimelineNotesAndInboxReminderTargets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "reminder_minutes_before",
                table: "inbox_proposals",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "target_plan_id",
                table: "inbox_proposals",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "target_plan_title",
                table: "inbox_proposals",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "calendar_timeline_notes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    family_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    text = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calendar_timeline_notes", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_calendar_timeline_notes_family_id_date",
                table: "calendar_timeline_notes",
                columns: new[] { "family_id", "date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "calendar_timeline_notes");

            migrationBuilder.DropColumn(
                name: "reminder_minutes_before",
                table: "inbox_proposals");

            migrationBuilder.DropColumn(
                name: "target_plan_id",
                table: "inbox_proposals");

            migrationBuilder.DropColumn(
                name: "target_plan_title",
                table: "inbox_proposals");
        }
    }
}
