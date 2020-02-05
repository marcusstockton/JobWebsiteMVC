using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JobWebsiteMVC.Data.Migrations
{
    public partial class JobSkills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobSkill",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    UpdatedById = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobSkill_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobSkill_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Job_JobSkills",
                columns: table => new
                {
                    JobId = table.Column<Guid>(nullable: false),
                    JobSkillId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job_JobSkills", x => new { x.JobId, x.JobSkillId });
                    table.ForeignKey(
                        name: "FK_Job_JobSkills_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Job_JobSkills_JobSkill_JobSkillId",
                        column: x => x.JobSkillId,
                        principalTable: "JobSkill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Job_JobSkills_JobSkillId",
                table: "Job_JobSkills",
                column: "JobSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkill_CreatedById",
                table: "JobSkill",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkill_UpdatedById",
                table: "JobSkill",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Job_JobSkills");

            migrationBuilder.DropTable(
                name: "JobSkill");
        }
    }
}
