using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PresPolls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(nullable: false),
                    PollId = table.Column<int>(nullable: false),
                    State = table.Column<string>(nullable: true),
                    PollsterId = table.Column<long>(nullable: false),
                    SponsorId = table.Column<long>(nullable: false),
                    PollsterRatingId = table.Column<long>(nullable: false),
                    FteGrade = table.Column<string>(nullable: true),
                    SampleSize = table.Column<int>(nullable: false),
                    Methodology = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Partisan = table.Column<string>(nullable: true),
                    RaceId = table.Column<int>(nullable: false),
                    CandidateName = table.Column<string>(nullable: true),
                    CandidateParty = table.Column<string>(nullable: true),
                    Pct = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresPolls", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PresPolls");
        }
    }
}
