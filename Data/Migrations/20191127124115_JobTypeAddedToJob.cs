using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JobWebsiteMVC.Data.Migrations
{
    public partial class JobTypeAddedToJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "JobTypeId",
                table: "Jobs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "JobTypeId1",
                table: "Jobs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobTypeId",
                table: "Jobs",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobTypeId1",
                table: "Jobs",
                column: "JobTypeId1");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Jobs_JobType_JobTypeId",
            //     table: "Jobs",
            //     column: "JobTypeId",
            //     principalTable: "JobType",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Cascade);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Jobs_JobType_JobTypeId1",
            //     table: "Jobs",
            //     column: "JobTypeId1",
            //     principalTable: "JobType",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobType_JobTypeId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobType_JobTypeId1",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobTypeId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobTypeId1",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobTypeId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobTypeId1",
                table: "Jobs");
        }
    }
}
