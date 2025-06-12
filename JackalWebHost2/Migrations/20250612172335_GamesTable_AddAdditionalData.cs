using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JackalWebHost2.Migrations
{
    public partial class GamesTable_AddAdditionalData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameMode",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "GameOver",
                table: "Games",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MapId",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MapSize",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TilesPackName",
                table: "Games",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Coins",
                table: "GamePlayers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "GamePlayers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameMode",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GameOver",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MapId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MapSize",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "TilesPackName",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Coins",
                table: "GamePlayers");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "GamePlayers");
        }
    }
}
