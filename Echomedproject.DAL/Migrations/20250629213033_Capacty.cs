using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Capacty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NomOfDoctors",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomOfDoctors",
                table: "Departments");
        }
    }
}
