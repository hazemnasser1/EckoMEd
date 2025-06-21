using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class pharmacyid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "pharmacies");

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "pharmacies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "pharmacies");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "pharmacies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "pharmacies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "pharmacies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
