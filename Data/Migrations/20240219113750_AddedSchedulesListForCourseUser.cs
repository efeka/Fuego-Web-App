using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSchedulesListForCourseUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseUserId",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_CourseUserId",
                table: "Schedules",
                column: "CourseUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_CourseUsers_CourseUserId",
                table: "Schedules",
                column: "CourseUserId",
                principalTable: "CourseUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_CourseUsers_CourseUserId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_CourseUserId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "CourseUserId",
                table: "Schedules");
        }
    }
}
