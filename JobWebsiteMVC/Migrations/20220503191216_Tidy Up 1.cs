using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobWebsiteMVC.Migrations
{
    public partial class TidyUp1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobBenefits_AspNetUsers_CreatedById",
                table: "JobBenefits");

            migrationBuilder.DropForeignKey(
                name: "FK_JobBenefits_AspNetUsers_UpdatedById",
                table: "JobBenefits");

            migrationBuilder.DropTable(
                name: "Job_JobBenefits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobBenefits",
                table: "JobBenefits");

            migrationBuilder.DropIndex(
                name: "IX_JobBenefits_CreatedById",
                table: "JobBenefits");

            migrationBuilder.DropIndex(
                name: "IX_JobBenefits_UpdatedById",
                table: "JobBenefits");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "JobBenefits");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "JobBenefits");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "JobBenefits");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "JobBenefits");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Jobs",
                newName: "JobTypeId2");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "JobBenefits",
                newName: "JobDetailsViewModelId");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "JobBenefits",
                newName: "JobBenefitId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "JobBenefits",
                newName: "JobId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobBenefits",
                table: "JobBenefits",
                columns: new[] { "JobId", "JobBenefitId" });

            migrationBuilder.CreateTable(
                name: "Benefits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedById = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedById = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benefits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Benefits_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Benefits_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    JobId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedById = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCategories_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobCategories_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobCategories_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobTypeId2",
                table: "Jobs",
                column: "JobTypeId2");

            migrationBuilder.CreateIndex(
                name: "IX_JobBenefits_JobBenefitId",
                table: "JobBenefits",
                column: "JobBenefitId");

            migrationBuilder.CreateIndex(
                name: "IX_JobBenefits_JobDetailsViewModelId",
                table: "JobBenefits",
                column: "JobDetailsViewModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Benefits_CreatedById",
                table: "Benefits",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Benefits_UpdatedById",
                table: "Benefits",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobCategories_CreatedById",
                table: "JobCategories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobCategories_JobId",
                table: "JobCategories",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCategories_UpdatedById",
                table: "JobCategories",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_JobBenefits_Benefits_JobBenefitId",
                table: "JobBenefits",
                column: "JobBenefitId",
                principalTable: "Benefits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobBenefits_JobDetailsViewModel_JobDetailsViewModelId",
                table: "JobBenefits",
                column: "JobDetailsViewModelId",
                principalTable: "JobDetailsViewModel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobBenefits_Jobs_JobId",
                table: "JobBenefits",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobTypes_JobTypeId2",
                table: "Jobs",
                column: "JobTypeId2",
                principalTable: "JobTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobBenefits_Benefits_JobBenefitId",
                table: "JobBenefits");

            migrationBuilder.DropForeignKey(
                name: "FK_JobBenefits_JobDetailsViewModel_JobDetailsViewModelId",
                table: "JobBenefits");

            migrationBuilder.DropForeignKey(
                name: "FK_JobBenefits_Jobs_JobId",
                table: "JobBenefits");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobTypes_JobTypeId2",
                table: "Jobs");

            migrationBuilder.DropTable(
                name: "Benefits");

            migrationBuilder.DropTable(
                name: "JobCategories");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobTypeId2",
                table: "Jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobBenefits",
                table: "JobBenefits");

            migrationBuilder.DropIndex(
                name: "IX_JobBenefits_JobBenefitId",
                table: "JobBenefits");

            migrationBuilder.DropIndex(
                name: "IX_JobBenefits_JobDetailsViewModelId",
                table: "JobBenefits");

            migrationBuilder.RenameColumn(
                name: "JobTypeId2",
                table: "Jobs",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "JobDetailsViewModelId",
                table: "JobBenefits",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "JobBenefitId",
                table: "JobBenefits",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "JobId",
                table: "JobBenefits",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "JobBenefits",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "JobBenefits",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "JobBenefits",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "JobBenefits",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobBenefits",
                table: "JobBenefits",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Job_JobBenefits",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "TEXT", nullable: false),
                    JobBenefitId = table.Column<Guid>(type: "TEXT", nullable: false),
                    JobDetailsViewModelId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job_JobBenefits", x => new { x.JobId, x.JobBenefitId });
                    table.ForeignKey(
                        name: "FK_Job_JobBenefits_JobBenefits_JobBenefitId",
                        column: x => x.JobBenefitId,
                        principalTable: "JobBenefits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Job_JobBenefits_JobDetailsViewModel_JobDetailsViewModelId",
                        column: x => x.JobDetailsViewModelId,
                        principalTable: "JobDetailsViewModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Job_JobBenefits_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobBenefits_CreatedById",
                table: "JobBenefits",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobBenefits_UpdatedById",
                table: "JobBenefits",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Job_JobBenefits_JobBenefitId",
                table: "Job_JobBenefits",
                column: "JobBenefitId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_JobBenefits_JobDetailsViewModelId",
                table: "Job_JobBenefits",
                column: "JobDetailsViewModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobBenefits_AspNetUsers_CreatedById",
                table: "JobBenefits",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobBenefits_AspNetUsers_UpdatedById",
                table: "JobBenefits",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
