using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomusMind.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetObligationLifecycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "time_zone_id",
                table: "families",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "asset_obligations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    family_id = table.Column<Guid>(type: "uuid", nullable: false),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    definition_operation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    initial_due_date = table.Column<DateOnly>(type: "date", nullable: false),
                    recurrence = table.Column<string>(type: "text", nullable: false),
                    interval_months = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<string>(type: "text", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_obligations", x => x.id);
                    table.ForeignKey(
                        name: "FK_asset_obligations_household_assets_family_id_asset_id",
                        columns: x => new { x.family_id, x.asset_id },
                        principalTable: "household_assets",
                        principalColumns: new[] { "family_id", "id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "asset_occurrences",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    obligation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    due_date = table.Column<DateOnly>(type: "date", nullable: false),
                    state = table.Column<string>(type: "text", nullable: false),
                    completed_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_occurrences", x => x.id);
                    table.UniqueConstraint("AK_asset_occurrences_obligation_id_id", x => new { x.obligation_id, x.id });
                    table.ForeignKey(
                        name: "FK_asset_occurrences_asset_obligations_obligation_id",
                        column: x => x.obligation_id,
                        principalTable: "asset_obligations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "asset_obligation_operations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    obligation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cycle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    requested_date = table.Column<DateOnly>(type: "date", nullable: true),
                    previous_due_date = table.Column<DateOnly>(type: "date", nullable: false),
                    result_obligation_state = table.Column<string>(type: "text", nullable: false),
                    result_occurrence_state = table.Column<string>(type: "text", nullable: false),
                    result_due_date = table.Column<DateOnly>(type: "date", nullable: false),
                    result_completed_date = table.Column<DateOnly>(type: "date", nullable: true),
                    result_next_cycle_id = table.Column<Guid>(type: "uuid", nullable: true),
                    result_next_due_date = table.Column<DateOnly>(type: "date", nullable: true),
                    recorded_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_obligation_operations", x => new { x.obligation_id, x.id });
                    table.ForeignKey(
                        name: "FK_asset_obligation_operations_asset_obligations_obligation_id",
                        column: x => x.obligation_id,
                        principalTable: "asset_obligations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_asset_obligation_operations_asset_occurrences_obligation_id~",
                        columns: x => new { x.obligation_id, x.cycle_id },
                        principalTable: "asset_occurrences",
                        principalColumns: new[] { "obligation_id", "id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_asset_obligation_operations_obligation_id_cycle_id",
                table: "asset_obligation_operations",
                columns: new[] { "obligation_id", "cycle_id" });

            migrationBuilder.CreateIndex(
                name: "IX_asset_obligations_family_id_asset_id",
                table: "asset_obligations",
                columns: new[] { "family_id", "asset_id" });

            migrationBuilder.CreateIndex(
                name: "IX_asset_obligations_family_id_definition_operation_id",
                table: "asset_obligations",
                columns: new[] { "family_id", "definition_operation_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DO $$ BEGIN
                  IF EXISTS (SELECT 1 FROM asset_obligations)
                     OR EXISTS (SELECT 1 FROM families WHERE time_zone_id IS NOT NULL) THEN
                    RAISE EXCEPTION 'Preserve obligation history and household timezone configuration before rollback';
                  END IF;
                END $$;
                """);
            migrationBuilder.DropTable(
                name: "asset_obligation_operations");

            migrationBuilder.DropTable(
                name: "asset_occurrences");

            migrationBuilder.DropTable(
                name: "asset_obligations");

            migrationBuilder.DropColumn(
                name: "time_zone_id",
                table: "families");
        }
    }
}
