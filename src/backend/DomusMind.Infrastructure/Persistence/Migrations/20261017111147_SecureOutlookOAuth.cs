using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomusMind.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SecureOutlookOAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Existing Outlook credentials use the previous unversioned format. Remove them
            // so connections must be authorized again before the new encryption contract is used.
            migrationBuilder.Sql("""
                UPDATE external_calendar_connections
                SET encrypted_refresh_token = NULL,
                    cached_access_token = NULL,
                    access_token_expires_at_utc = NULL,
                    granted_scopes = NULL
                WHERE provider = 'microsoft';
                """);

            migrationBuilder.CreateTable(
                name: "external_calendar_oauth_transactions",
                columns: table => new
                {
                    state_hash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    requested_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    family_id = table.Column<Guid>(type: "uuid", nullable: false),
                    member_id = table.Column<Guid>(type: "uuid", nullable: false),
                    redirect_uri = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    protected_pkce_verifier = table.Column<string>(type: "text", nullable: false),
                    expires_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_external_calendar_oauth_transactions", x => x.state_hash);
                });

            migrationBuilder.CreateIndex(
                name: "ix_external_calendar_oauth_transactions_expires_at_utc",
                table: "external_calendar_oauth_transactions",
                column: "expires_at_utc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "external_calendar_oauth_transactions");
        }
    }
}
