using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace employeeDailyTaskRecorder.Migrations
{
    /// <inheritdoc />
    public partial class altertablefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
         name: "ProfileImg",
         table: "Employees",
         type: "nvarchar(max)",
         nullable: true,
         defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
           name: "ProfileImg",
           table: "Employees");
        }
    }
}



