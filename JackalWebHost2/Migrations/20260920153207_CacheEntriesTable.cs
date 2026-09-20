using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JackalWebHost2.Migrations
{
    /// <inheritdoc />
    public partial class CacheEntriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CacheEntries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Payload = table.Column<string>(type: "jsonb", nullable: false),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false),
                    CreatorName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Players = table.Column<string>(type: "jsonb", nullable: true),
                    TimeStamp = table.Column<long>(type: "bigint", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CacheEntries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CacheEntries_IsCompleted",
                table: "CacheEntries",
                column: "IsCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_CacheEntries_TimeStamp",
                table: "CacheEntries",
                column: "TimeStamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CacheEntries");
        }
    }
}
