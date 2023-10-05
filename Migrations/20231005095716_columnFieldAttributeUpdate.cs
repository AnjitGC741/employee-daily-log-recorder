using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace employeeDailyTaskRecorder.Migrations
{
    /// <inheritdoc />
    public partial class columnFieldAttributeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Employees SET ContactNumber = '123456', EmpRole = '1', EmpStage = '1', Gender = '1', CurrentStageCompletionDate = GETDATE(), JoinDate = GETDATE() where ContactNumber is null");

            migrationBuilder.AlterColumn<string>(
                name: "ContactNumber",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CurrentStageCompletionDate",
                table: "Employees",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "EmpRole",
                table: "Employees",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "EmpStage",
                table: "Employees",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Employees",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "JoinDate",
                table: "Employees",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
