using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomusMind.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AssistantProviderRoutingAndInboxAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "quantity",
                table: "inbox_proposals",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "target_item_id",
                table: "inbox_proposals",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "target_list_id",
                table: "inbox_proposals",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "provider_fallback_occurred",
                table: "inbox_items",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "provider_hosting",
                table: "inbox_items",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "provider_name",
                table: "inbox_items",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "source_chat_message_id",
                table: "inbox_items",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "fallback_occurred",
                table: "chat_messages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "provider_hosting",
                table: "chat_messages",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "provider_name",
                table: "chat_messages",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "assistant_provider_routes",
                columns: table => new
                {
                    family_id = table.Column<Guid>(type: "uuid", nullable: false),
                    targets_json = table.Column<string>(type: "text", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assistant_provider_routes", x => x.family_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assistant_provider_routes");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "inbox_proposals");

            migrationBuilder.DropColumn(
                name: "target_item_id",
                table: "inbox_proposals");

            migrationBuilder.DropColumn(
                name: "target_list_id",
                table: "inbox_proposals");

            migrationBuilder.DropColumn(
                name: "provider_fallback_occurred",
                table: "inbox_items");

            migrationBuilder.DropColumn(
                name: "provider_hosting",
                table: "inbox_items");

            migrationBuilder.DropColumn(
                name: "provider_name",
                table: "inbox_items");

            migrationBuilder.DropColumn(
                name: "source_chat_message_id",
                table: "inbox_items");

            migrationBuilder.DropColumn(
                name: "fallback_occurred",
                table: "chat_messages");

            migrationBuilder.DropColumn(
                name: "provider_hosting",
                table: "chat_messages");

            migrationBuilder.DropColumn(
                name: "provider_name",
                table: "chat_messages");
        }
    }
}
