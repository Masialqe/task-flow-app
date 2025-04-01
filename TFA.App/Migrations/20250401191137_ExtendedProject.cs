using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFA.App.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskSubTaskCount",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskSubTaskCount",
                table: "Tasks");
        }
    }
}
