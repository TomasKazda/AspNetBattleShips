using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BattleShips.Migrations
{
    public partial class GameDataGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GameCreatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2020, 1, 28, 6, 41, 46, 152, DateTimeKind.Utc).AddTicks(2605)),
                    Player1Id = table.Column<string>(nullable: false),
                    Player2Id = table.Column<string>(nullable: true),
                    GameState = table.Column<int>(nullable: false),
                    Player1OnTurn = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_AspNetUsers_Player1Id",
                        column: x => x.Player1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_AspNetUsers_Player2Id",
                        column: x => x.Player2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GamePieces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<string>(nullable: true),
                    GameId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Hidden = table.Column<bool>(nullable: false),
                    CoordinateX = table.Column<int>(nullable: false),
                    CoordinateY = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePieces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamePieces_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePieces_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamePieces_GameId",
                table: "GamePieces",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePieces_OwnerId",
                table: "GamePieces",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Player1Id",
                table: "Games",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Player2Id",
                table: "Games",
                column: "Player2Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamePieces");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
