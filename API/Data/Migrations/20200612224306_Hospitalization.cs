using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class Hospitalization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hospitalization",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    Amarillo = table.Column<int>(nullable: false),
                    Lubbock = table.Column<int>(nullable: false),
                    Wichita = table.Column<int>(nullable: false),
                    Abilene = table.Column<int>(nullable: false),
                    Dallas = table.Column<int>(nullable: false),
                    Paris = table.Column<int>(nullable: false),
                    Tyler = table.Column<int>(nullable: false),
                    Lufkin = table.Column<int>(nullable: false),
                    ElPaso = table.Column<int>(nullable: false),
                    MidlanOdessa = table.Column<int>(nullable: false),
                    SanAngelo = table.Column<int>(nullable: false),
                    BeltonKilleen = table.Column<int>(nullable: false),
                    Waco = table.Column<int>(nullable: false),
                    BryanCollegStation = table.Column<int>(nullable: false),
                    Austin = table.Column<int>(nullable: false),
                    SanAntonio = table.Column<int>(nullable: false),
                    Houston = table.Column<int>(nullable: false),
                    Galveston = table.Column<int>(nullable: false),
                    Victoria = table.Column<int>(nullable: false),
                    Laredo = table.Column<int>(nullable: false),
                    CorpusChristi = table.Column<int>(nullable: false),
                    RioGrandeValley = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitalization", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hospitalization");
        }
    }
}
