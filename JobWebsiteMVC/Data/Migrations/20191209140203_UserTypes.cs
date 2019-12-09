using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JobWebsiteMVC.Data.Migrations
{
    public partial class UserTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_Jobs_JobType_JobTypeId",
            //     table: "Jobs");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_Jobs_JobType_JobTypeId1",
            //     table: "Jobs");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_JobType_AspNetUsers_CreatedById",
            //     table: "JobType");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_JobType_AspNetUsers_UpdatedById",
            //     table: "JobType");

            // migrationBuilder.DropPrimaryKey(
            //     name: "PK_JobType",
            //     table: "JobType");

            migrationBuilder.RenameTable(
                name: "JobType",
                newName: "JobTypes");

            migrationBuilder.RenameIndex(
                name: "IX_JobType_UpdatedById",
                table: "JobTypes",
                newName: "IX_JobTypes_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_JobType_CreatedById",
                table: "JobTypes",
                newName: "IX_JobTypes_CreatedById");

            // migrationBuilder.AlterColumn<TimeSpan>(
            //     name: "WorkingHoursStart",
            //     table: "Jobs",
            //     nullable: true,
            //     oldClrType: typeof(TimeSpan),
            //     oldType: "TEXT");

            // migrationBuilder.AlterColumn<TimeSpan>(
            //     name: "WorkingHoursEnd",
            //     table: "Jobs",
            //     nullable: true,
            //     oldClrType: typeof(TimeSpan),
            //     oldType: "TEXT");

            // migrationBuilder.AlterColumn<decimal>(
            //     name: "MinSalary",
            //     table: "Jobs",
            //     nullable: true,
            //     oldClrType: typeof(decimal),
            //     oldType: "TEXT");

            // migrationBuilder.AlterColumn<decimal>(
            //     name: "MaxSalary",
            //     table: "Jobs",
            //     nullable: true,
            //     oldClrType: typeof(decimal),
            //     oldType: "TEXT");

            // migrationBuilder.AlterColumn<decimal>(
            //     name: "HoursPerWeek",
            //     table: "Jobs",
            //     nullable: true,
            //     oldClrType: typeof(decimal),
            //     oldType: "TEXT");

            // migrationBuilder.AlterColumn<decimal>(
            //     name: "HolidayEntitlement",
            //     table: "Jobs",
            //     nullable: true,
            //     oldClrType: typeof(decimal),
            //     oldType: "TEXT");

            migrationBuilder.AddColumn<Guid>(
                name: "UserTypeId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            // migrationBuilder.AddPrimaryKey(
            //     name: "PK_JobTypes",
            //     table: "JobTypes",
            //     column: "Id");

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserTypeId",
                table: "AspNetUsers",
                column: "UserTypeId");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_AspNetUsers_UserTypes_UserTypeId",
            //     table: "AspNetUsers",
            //     column: "UserTypeId",
            //     principalTable: "UserTypes",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Cascade);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Jobs_JobTypes_JobTypeId",
            //     table: "Jobs",
            //     column: "JobTypeId",
            //     principalTable: "JobTypes",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Cascade);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Jobs_JobTypes_JobTypeId1",
            //     table: "Jobs",
            //     column: "JobTypeId1",
            //     principalTable: "JobTypes",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_JobTypes_AspNetUsers_CreatedById",
            //     table: "JobTypes",
            //     column: "CreatedById",
            //     principalTable: "AspNetUsers",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_JobTypes_AspNetUsers_UpdatedById",
            //     table: "JobTypes",
            //     column: "UpdatedById",
            //     principalTable: "AspNetUsers",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserTypes_UserTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobTypes_JobTypeId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobTypes_JobTypeId1",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTypes_AspNetUsers_CreatedById",
                table: "JobTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTypes_AspNetUsers_UpdatedById",
                table: "JobTypes");

            migrationBuilder.DropTable(
                name: "UserTypes");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobTypes",
                table: "JobTypes");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "JobTypes",
                newName: "JobType");

            migrationBuilder.RenameIndex(
                name: "IX_JobTypes_UpdatedById",
                table: "JobType",
                newName: "IX_JobType_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_JobTypes_CreatedById",
                table: "JobType",
                newName: "IX_JobType_CreatedById");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "WorkingHoursStart",
                table: "Jobs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "WorkingHoursEnd",
                table: "Jobs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MinSalary",
                table: "Jobs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxSalary",
                table: "Jobs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "HoursPerWeek",
                table: "Jobs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "HolidayEntitlement",
                table: "Jobs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobType",
                table: "JobType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobType_JobTypeId",
                table: "Jobs",
                column: "JobTypeId",
                principalTable: "JobType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobType_JobTypeId1",
                table: "Jobs",
                column: "JobTypeId1",
                principalTable: "JobType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobType_AspNetUsers_CreatedById",
                table: "JobType",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobType_AspNetUsers_UpdatedById",
                table: "JobType",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
