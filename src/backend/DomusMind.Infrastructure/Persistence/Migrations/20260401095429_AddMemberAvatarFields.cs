using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomusMind.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberAvatarFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "avatar_color_id",
                table: "family_members",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "avatar_icon_id",
                table: "family_members",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar_color_id",
                table: "family_members");

            migrationBuilder.DropColumn(
                name: "avatar_icon_id",
                table: "family_members");
        }
    }
}
