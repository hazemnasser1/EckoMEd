using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class dashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfbirth",
                table: "patientHospital",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "patientHospital",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "patientName",
                table: "patientHospital",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LabTestCount",
                table: "dataEntry",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfbirth",
                table: "patientHospital");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "patientHospital");

            migrationBuilder.DropColumn(
                name: "patientName",
                table: "patientHospital");

            migrationBuilder.DropColumn(
                name: "LabTestCount",
                table: "dataEntry");
        }
    }
}
