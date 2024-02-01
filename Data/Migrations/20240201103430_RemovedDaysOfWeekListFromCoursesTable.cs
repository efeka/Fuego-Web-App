using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedDaysOfWeekListFromCoursesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedule_Courses_CourseId",
                table: "CourseSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseSchedule",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "DaysOfWeek",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "CourseSchedule",
                newName: "CourseSchedules");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSchedule_CourseId",
                table: "CourseSchedules",
                newName: "IX_CourseSchedules_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseSchedules",
                table: "CourseSchedules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedules_Courses_CourseId",
                table: "CourseSchedules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedules_Courses_CourseId",
                table: "CourseSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseSchedules",
                table: "CourseSchedules");

            migrationBuilder.RenameTable(
                name: "CourseSchedules",
                newName: "CourseSchedule");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSchedules_CourseId",
                table: "CourseSchedule",
                newName: "IX_CourseSchedule_CourseId");

            migrationBuilder.AddColumn<string>(
                name: "DaysOfWeek",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseSchedule",
                table: "CourseSchedule",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedule_Courses_CourseId",
                table: "CourseSchedule",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
