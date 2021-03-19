using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristickaAgencija.Data.Migrations
{
    public partial class DodavanjeAranzmana : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aranzmani",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrojDana = table.Column<int>(type: "int", nullable: false),
                    Mesto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Popust = table.Column<float>(type: "real", nullable: true),
                    PonudaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aranzmani", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aranzmani_Ponude_PonudaId",
                        column: x => x.PonudaId,
                        principalTable: "Ponude",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aranzmani_PonudaId",
                table: "Aranzmani",
                column: "PonudaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aranzmani");
        }
    }
}
