using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace employeeDailyTaskRecorder.Migrations
{
    /// <inheritdoc />
    public partial class RemovedNullColValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"update Employees set ProfileImg = '' where ProfileImg is null");
            migrationBuilder.AlterColumn<string>(
     name: "ProfileImg",
     table: "Employees",
     type: "nvarchar(max)",
     nullable: false,
     defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
