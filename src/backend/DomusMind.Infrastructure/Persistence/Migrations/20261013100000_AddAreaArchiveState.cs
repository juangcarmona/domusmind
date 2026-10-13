using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using DomusMind.Infrastructure.Persistence;

namespace DomusMind.Infrastructure.Persistence.Migrations;

[DbContext(typeof(DomusMindDbContext))]
[Migration("20261013100000_AddAreaArchiveState")]
public sealed class AddAreaArchiveState : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "is_archived",
            table: "responsibility_domains",
            type: "boolean",
            nullable: false,
            defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "is_archived",
            table: "responsibility_domains");
    }
}
