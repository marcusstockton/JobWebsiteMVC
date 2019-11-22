using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JobWebsiteMVC.Data.Migrations
{
    public partial class M2M_Work : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Job_JobBenefits_JobBenefits_JobBenefitsId",
            //    table: "Job_JobBenefits");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Job_JobBenefits",
            //    table: "Job_JobBenefits");

            //migrationBuilder.DropIndex(
            //    name: "IX_Job_JobBenefits_JobBenefitsId",
            //    table: "Job_JobBenefits");

            //migrationBuilder.DropColumn(
            //    name: "JobBenefitsId",
            //    table: "Job_JobBenefits");

            migrationBuilder.AddColumn<Guid>(
                name: "JobBenefitId",
                table: "Job_JobBenefits",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Job_JobBenefits",
            //    table: "Job_JobBenefits",
            //    columns: new[] { "JobId", "JobBenefitId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Job_JobBenefits_JobBenefitId",
            //    table: "Job_JobBenefits",
            //    column: "JobBenefitId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Job_JobBenefits_JobBenefits_JobBenefitId",
            //    table: "Job_JobBenefits",
            //    column: "JobBenefitId",
            //    principalTable: "JobBenefits",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        //    migrationBuilder.DropForeignKey(
        //        name: "FK_Job_JobBenefits_JobBenefits_JobBenefitId",
        //        table: "Job_JobBenefits");

        //    migrationBuilder.DropPrimaryKey(
        //        name: "PK_Job_JobBenefits",
        //        table: "Job_JobBenefits");

        //    migrationBuilder.DropIndex(
        //        name: "IX_Job_JobBenefits_JobBenefitId",
        //        table: "Job_JobBenefits");

        //    migrationBuilder.DropColumn(
        //        name: "JobBenefitId",
        //        table: "Job_JobBenefits");

        //    migrationBuilder.AddColumn<Guid>(
        //        name: "JobBenefitsId",
        //        table: "Job_JobBenefits",
        //        type: "TEXT",
        //        nullable: false,
        //        defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        //    migrationBuilder.AddPrimaryKey(
        //        name: "PK_Job_JobBenefits",
        //        table: "Job_JobBenefits",
        //        columns: new[] { "JobId", "JobBenefitsId" });

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Job_JobBenefits_JobBenefitsId",
        //        table: "Job_JobBenefits",
        //        column: "JobBenefitsId");

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_Job_JobBenefits_JobBenefits_JobBenefitsId",
        //        table: "Job_JobBenefits",
        //        column: "JobBenefitsId",
        //        principalTable: "JobBenefits",
        //        principalColumn: "Id",
        //        onDelete: ReferentialAction.Cascade);
        } 
    }
}
