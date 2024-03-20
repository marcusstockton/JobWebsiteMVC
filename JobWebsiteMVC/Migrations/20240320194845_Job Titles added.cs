using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobWebsiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class JobTitlesadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "WorkingHoursEnd",
                table: "JobDetailsViewModel",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "WorkingHoursStart",
                table: "JobDetailsViewModel",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.CreateTable(
                name: "JobTitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobTitles");

            migrationBuilder.DropColumn(
                name: "WorkingHoursEnd",
                table: "JobDetailsViewModel");

            migrationBuilder.DropColumn(
                name: "WorkingHoursStart",
                table: "JobDetailsViewModel");
        }
    }
}
