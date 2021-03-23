using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristickaAgencija.Data.Migrations
{
    public partial class VezivanjePrevozaZaSmestaj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prevozi_Aranzmani_AranzmanId",
                table: "Prevozi");

            migrationBuilder.DropIndex(
                name: "IX_Prevozi_AranzmanId",
                table: "Prevozi");

            migrationBuilder.DropColumn(
                name: "AranzmanId",
                table: "Prevozi");

            migrationBuilder.CreateTable(
                name: "PrevozSmestaj",
                columns: table => new
                {
                    PrevozsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SmestajsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrevozSmestaj", x => new { x.PrevozsId, x.SmestajsId });
                    table.ForeignKey(
                        name: "FK_PrevozSmestaj_Prevozi_PrevozsId",
                        column: x => x.PrevozsId,
                        principalTable: "Prevozi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrevozSmestaj_Smestaji_SmestajsId",
                        column: x => x.SmestajsId,
                        principalTable: "Smestaji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacije",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SmestajId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrevozId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacije", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rezervacije_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezervacije_Prevozi_PrevozId",
                        column: x => x.PrevozId,
                        principalTable: "Prevozi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezervacije_Smestaji_SmestajId",
                        column: x => x.SmestajId,
                        principalTable: "Smestaji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrevozSmestaj_SmestajsId",
                table: "PrevozSmestaj",
                column: "SmestajsId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_PrevozId",
                table: "Rezervacije",
                column: "PrevozId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_SmestajId",
                table: "Rezervacije",
                column: "SmestajId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_UserId",
                table: "Rezervacije",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrevozSmestaj");

            migrationBuilder.DropTable(
                name: "Rezervacije");

            migrationBuilder.AddColumn<Guid>(
                name: "AranzmanId",
                table: "Prevozi",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Prevozi_AranzmanId",
                table: "Prevozi",
                column: "AranzmanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prevozi_Aranzmani_AranzmanId",
                table: "Prevozi",
                column: "AranzmanId",
                principalTable: "Aranzmani",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
