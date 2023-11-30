using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BethanysPieShopAdmin.Migrations
{
    public partial class DbInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Treninzi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treninzi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vezbe",
                columns: table => new
                {
                    VezbaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    VezbaName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VezbaDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rekviziti = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TreningId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vezbe", x => x.VezbaId);
                    table.ForeignKey(
                        name: "FK_Vezbe_Treninzi_TreningId",
                        column: x => x.TreningId,
                        principalTable: "Treninzi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vezbe_TreningId",
                table: "Vezbe",
                column: "TreningId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vezbe");

            migrationBuilder.DropTable(
                name: "Treninzi");
        }
    }
}
