using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class CreatedUserSchedulesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "UserSchedules",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSchedules", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_UserSchedules_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSchedules_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSchedules_ApplicationUserId",
                table: "UserSchedules",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSchedules_ScheduleId",
                table: "UserSchedules",
                column: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSchedules");

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
    }
}
