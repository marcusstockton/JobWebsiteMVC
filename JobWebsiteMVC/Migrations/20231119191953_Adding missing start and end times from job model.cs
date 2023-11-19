using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobWebsiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class Addingmissingstartandendtimesfromjobmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingHoursEnd",
                table: "JobDetailsViewModel");

            migrationBuilder.DropColumn(
                name: "WorkingHoursStart",
                table: "JobDetailsViewModel");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkingHoursEnd",
                table: "Jobs",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkingHoursStart",
                table: "Jobs",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingHoursEnd",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "WorkingHoursStart",
                table: "Jobs");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkingHoursEnd",
                table: "JobDetailsViewModel",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkingHoursStart",
                table: "JobDetailsViewModel",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
