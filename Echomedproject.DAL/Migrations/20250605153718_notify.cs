using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class notify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsExist",
                table: "notifications",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicineaName",
                table: "notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PharmacyID",
                table: "notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "IsExist",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "MedicineaName",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "PharmacyID",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "notifications");
        }
    }
}
