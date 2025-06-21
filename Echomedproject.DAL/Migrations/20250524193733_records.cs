using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class records : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Scans_ScanId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_invoices_InvoiceId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_labTests_LabTestId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_prescriptions_prescriptionId",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_ScanId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "ScanId",
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
                name: "frequency",
                table: "prescriptions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Scans",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Scans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecordsId",
                table: "Scans",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "prescriptionId",
                table: "Records",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LabTestId",
                table: "Records",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                table: "Records",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MedDate",
                table: "Midicine",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Scans_RecordsId",
                table: "Scans",
                column: "RecordsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_invoices_InvoiceId",
                table: "Records",
                column: "InvoiceId",
                principalTable: "invoices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_labTests_LabTestId",
                table: "Records",
                column: "LabTestId",
                principalTable: "labTests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_prescriptions_prescriptionId",
                table: "Records",
                column: "prescriptionId",
                principalTable: "prescriptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Scans_Records_RecordsId",
                table: "Scans",
                column: "RecordsId",
                principalTable: "Records",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_invoices_InvoiceId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_labTests_LabTestId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_prescriptions_prescriptionId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Scans_Records_RecordsId",
                table: "Scans");

            migrationBuilder.DropIndex(
                name: "IX_Scans_RecordsId",
                table: "Scans");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Scans");

            migrationBuilder.DropColumn(
                name: "RecordsId",
                table: "Scans");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Scans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "prescriptionId",
                table: "Records",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LabTestId",
                table: "Records",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                table: "Records",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScanId",
                table: "Records",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "frequency",
                table: "prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MedDate",
                table: "Midicine",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Records_ScanId",
                table: "Records",
                column: "ScanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Scans_ScanId",
                table: "Records",
                column: "ScanId",
                principalTable: "Scans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_invoices_InvoiceId",
                table: "Records",
                column: "InvoiceId",
                principalTable: "invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_labTests_LabTestId",
                table: "Records",
                column: "LabTestId",
                principalTable: "labTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_prescriptions_prescriptionId",
                table: "Records",
                column: "prescriptionId",
                principalTable: "prescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
