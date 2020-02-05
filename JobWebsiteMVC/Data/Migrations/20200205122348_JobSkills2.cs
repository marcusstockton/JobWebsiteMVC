using Microsoft.EntityFrameworkCore.Migrations;

namespace JobWebsiteMVC.Data.Migrations
{
    public partial class JobSkills2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_Job_JobSkills_JobSkill_JobSkillId",
            //     table: "Job_JobSkills");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_JobSkill_AspNetUsers_CreatedById",
            //     table: "JobSkill");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_JobSkill_AspNetUsers_UpdatedById",
            //     table: "JobSkill");

            // migrationBuilder.DropPrimaryKey(
            //     name: "PK_JobSkill",
            //     table: "JobSkill");

            migrationBuilder.RenameTable(
                name: "JobSkill",
                newName: "JobSkills");

            // migrationBuilder.RenameIndex(
            //     name: "IX_JobSkill_UpdatedById",
            //     table: "JobSkills",
            //     newName: "IX_JobSkills_UpdatedById");

            // migrationBuilder.RenameIndex(
            //     name: "IX_JobSkill_CreatedById",
            //     table: "JobSkills",
            //     newName: "IX_JobSkills_CreatedById");

            // migrationBuilder.AddPrimaryKey(
            //     name: "PK_JobSkills",
            //     table: "JobSkills",
            //     column: "Id");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Job_JobSkills_JobSkills_JobSkillId",
            //     table: "Job_JobSkills",
            //     column: "JobSkillId",
            //     principalTable: "JobSkills",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Cascade);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_JobSkills_AspNetUsers_CreatedById",
            //     table: "JobSkills",
            //     column: "CreatedById",
            //     principalTable: "AspNetUsers",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_JobSkills_AspNetUsers_UpdatedById",
            //     table: "JobSkills",
            //     column: "UpdatedById",
            //     principalTable: "AspNetUsers",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_JobSkills_JobSkills_JobSkillId",
                table: "Job_JobSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSkills_AspNetUsers_CreatedById",
                table: "JobSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSkills_AspNetUsers_UpdatedById",
                table: "JobSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSkills",
                table: "JobSkills");

            migrationBuilder.RenameTable(
                name: "JobSkills",
                newName: "JobSkill");

            migrationBuilder.RenameIndex(
                name: "IX_JobSkills_UpdatedById",
                table: "JobSkill",
                newName: "IX_JobSkill_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_JobSkills_CreatedById",
                table: "JobSkill",
                newName: "IX_JobSkill_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSkill",
                table: "JobSkill",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_JobSkills_JobSkill_JobSkillId",
                table: "Job_JobSkills",
                column: "JobSkillId",
                principalTable: "JobSkill",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSkill_AspNetUsers_CreatedById",
                table: "JobSkill",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSkill_AspNetUsers_UpdatedById",
                table: "JobSkill",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
