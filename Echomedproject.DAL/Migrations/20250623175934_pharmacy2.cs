using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class pharmacy2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "IsExist",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "MedicineName",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "notifications");

            migrationBuilder.RenameColumn(
                name: "PharmacyID",
                table: "notifications",
                newName: "Text");

            migrationBuilder.AddColumn<string>(
                name: "Response",
                table: "Request",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "SentAt",
                table: "Request",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Response",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "SentAt",
                table: "Request");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "notifications",
                newName: "PharmacyID");

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
                name: "MedicineName",
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
    }
}
