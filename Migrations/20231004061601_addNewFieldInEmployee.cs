using System;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace employeeDailyTaskRecorder.Migrations
{
    /// <inheritdoc />
    public partial class addNewFieldInEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentStageCompletionDate",
                table: "Employees",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EmpRole",
                table: "Employees",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmpStage",
                table: "Employees",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Employees",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinDate",
                table: "Employees",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CurrentStageCompletionDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmpRole",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmpStage",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "JoinDate",
                table: "Employees");
        }
    }
}
