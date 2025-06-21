using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RecordUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Scans",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Records",
                newName: "HospitalName");

            migrationBuilder.RenameColumn(
                name: "medicines",
                table: "prescriptions",
                newName: "frequency");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "invoices",
                newName: "PatientName");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Scans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Scans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Records",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DoctorName",
                table: "Records",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "visitDate",
                table: "Records",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Dosage",
                table: "prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "doctorNotes",
                table: "prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "duration",
                table: "prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DoctorName",
                table: "invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Total",
                table: "invoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created",
                table: "invoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Charge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Charge_invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "invoices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Medicine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    frequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prescriptionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicine_prescriptions_prescriptionId",
                        column: x => x.prescriptionId,
                        principalTable: "prescriptions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Charge_InvoiceId",
                table: "Charge",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicine_prescriptionId",
                table: "Medicine",
                column: "prescriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Charge");

            migrationBuilder.DropTable(
                name: "Medicine");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Scans");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Scans");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "DoctorName",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "visitDate",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "Dosage",
                table: "prescriptions");

            migrationBuilder.DropColumn(
                name: "doctorNotes",
                table: "prescriptions");

            migrationBuilder.DropColumn(
                name: "duration",
                table: "prescriptions");

            migrationBuilder.DropColumn(
                name: "DoctorName",
                table: "invoices");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "invoices");

            migrationBuilder.DropColumn(
                name: "created",
                table: "invoices");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Scans",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "HospitalName",
                table: "Records",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "frequency",
                table: "prescriptions",
                newName: "medicines");

            migrationBuilder.RenameColumn(
                name: "PatientName",
                table: "invoices",
                newName: "Name");
        }
    }
}
