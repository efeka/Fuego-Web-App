using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInstructorColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IBAN",
                table: "Instructors");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Instructors",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Instructors",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "IBAN",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
