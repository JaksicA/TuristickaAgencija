using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristickaAgencija.Data.Migrations
{
    public partial class DodavanjePrevoza : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prevozi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VrstaPrevoza = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cena = table.Column<float>(type: "real", nullable: false),
                    AranzmanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prevozi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prevozi_Aranzmani_AranzmanId",
                        column: x => x.AranzmanId,
                        principalTable: "Aranzmani",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prevozi_AranzmanId",
                table: "Prevozi",
                column: "AranzmanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prevozi");
        }
    }
}
