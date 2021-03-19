using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristickaAgencija.Data.Migrations
{
    public partial class DodavanjeSmestaja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Smestaji",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrojKreveta = table.Column<int>(type: "int", nullable: false),
                    Tip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cena = table.Column<float>(type: "real", nullable: false),
                    AranzmanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Smestaji", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Smestaji_Aranzmani_AranzmanId",
                        column: x => x.AranzmanId,
                        principalTable: "Aranzmani",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Smestaji_AranzmanId",
                table: "Smestaji",
                column: "AranzmanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Smestaji");
        }
    }
}
