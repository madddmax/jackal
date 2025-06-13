using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JackalWebHost2.Migrations
{
    public partial class GamePlayersTable_AddWinner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Winner",
                table: "GamePlayers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Winner",
                table: "GamePlayers");
        }
    }
}
