using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JackalWebHost2.Migrations
{
    public partial class GamesTable_RemoveCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Games");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Games",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");
        }
    }
}
