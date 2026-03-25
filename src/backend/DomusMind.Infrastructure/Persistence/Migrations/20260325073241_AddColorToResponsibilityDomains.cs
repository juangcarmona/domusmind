using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomusMind.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddColorToResponsibilityDomains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "responsibility_domains",
                type: "character varying(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "#6A4C93");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "color",
                table: "responsibility_domains");
        }
    }
}
