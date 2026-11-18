using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomusMind.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHouseholdAssetFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "household_asset_locations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    family_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_household_asset_locations", x => x.id);
                    table.UniqueConstraint("AK_household_asset_locations_family_id_id", x => new { x.family_id, x.id });
                });

            migrationBuilder.CreateTable(
                name: "household_assets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    family_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "text", nullable: false),
                    make = table.Column<string>(type: "text", nullable: true),
                    model = table.Column<string>(type: "text", nullable: true),
                    serial_number = table.Column<string>(type: "text", nullable: true),
                    management_basis = table.Column<string>(type: "text", nullable: true),
                    parent_asset_id = table.Column<Guid>(type: "uuid", nullable: true),
                    location_id = table.Column<Guid>(type: "uuid", nullable: true),
                    responsible_member_id = table.Column<Guid>(type: "uuid", nullable: true),
                    responsible_area_id = table.Column<Guid>(type: "uuid", nullable: true),
                    version = table.Column<long>(type: "bigint", nullable: false),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_household_assets", x => x.id);
                    table.UniqueConstraint("AK_household_assets_family_id_id", x => new { x.family_id, x.id });
                    table.ForeignKey(
                        name: "FK_household_assets_household_asset_locations_family_id_locati~",
                        columns: x => new { x.family_id, x.location_id },
                        principalTable: "household_asset_locations",
                        principalColumns: new[] { "family_id", "id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_household_assets_household_assets_family_id_parent_asset_id",
                        columns: x => new { x.family_id, x.parent_asset_id },
                        principalTable: "household_assets",
                        principalColumns: new[] { "family_id", "id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_household_assets_family_id_is_archived",
                table: "household_assets",
                columns: new[] { "family_id", "is_archived" });

            migrationBuilder.CreateIndex(
                name: "IX_household_assets_family_id_location_id",
                table: "household_assets",
                columns: new[] { "family_id", "location_id" });

            migrationBuilder.CreateIndex(
                name: "IX_household_assets_family_id_parent_asset_id",
                table: "household_assets",
                columns: new[] { "family_id", "parent_asset_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // A rollback is safe only before these durable household identities have been used.
            migrationBuilder.Sql("""
                LOCK TABLE household_assets, household_asset_locations IN ACCESS EXCLUSIVE MODE;
                DO $$ BEGIN
                    IF EXISTS (SELECT 1 FROM household_assets) OR EXISTS (SELECT 1 FROM household_asset_locations) THEN
                        RAISE EXCEPTION 'Household asset rollback refused: preserve or export existing household data first.';
                    END IF;
                END $$;
                """);

            migrationBuilder.DropTable(
                name: "household_assets");

            migrationBuilder.DropTable(
                name: "household_asset_locations");
        }
    }
}
