using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Polls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(nullable: false),
                    PollId = table.Column<int>(nullable: false),
                    State = table.Column<string>(nullable: true),
                    PollsterId = table.Column<int>(nullable: false),
                    SponsorIds = table.Column<int>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PresPolls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(nullable: false),
                    PollId = table.Column<int>(nullable: true),
                    Stage = table.Column<string>(nullable: true),
                    RaceId = table.Column<int>(nullable: false),
                    State = table.Column<string>(nullable: true),
                    CandidateName = table.Column<string>(nullable: true),
                    CandidateParty = table.Column<string>(nullable: true),
                    pct = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresPolls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresPolls_Polls_PollId",
                        column: x => x.PollId,
                        principalTable: "Polls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PresPolls_PollId",
                table: "PresPolls",
                column: "PollId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PresPolls");

            migrationBuilder.DropTable(
                name: "Polls");
        }
    }
}
