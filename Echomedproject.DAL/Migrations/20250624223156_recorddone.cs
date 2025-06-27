using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class recorddone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_labTests_LabTestId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_prescriptions_prescriptionId",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_LabTestId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "LabTestId",
                table: "Records");

            migrationBuilder.AddColumn<string>(
                name: "bodypart",
                table: "Scans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "prescriptionId",
                table: "Records",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "notes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "notes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "dateTime",
                table: "notes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Timing",
                table: "Midicine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "labTests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "labTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "labTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RecordsId",
                table: "labTests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "labTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_labTests_RecordsId",
                table: "labTests",
                column: "RecordsId");

            migrationBuilder.AddForeignKey(
                name: "FK_labTests_Records_RecordsId",
                table: "labTests",
                column: "RecordsId",
                principalTable: "Records",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_prescriptions_prescriptionId",
                table: "Records",
                column: "prescriptionId",
                principalTable: "prescriptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_labTests_Records_RecordsId",
                table: "labTests");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_prescriptions_prescriptionId",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_labTests_RecordsId",
                table: "labTests");

            migrationBuilder.DropColumn(
                name: "bodypart",
                table: "Scans");

            migrationBuilder.DropColumn(
                name: "dateTime",
                table: "notes");

            migrationBuilder.DropColumn(
                name: "Timing",
                table: "Midicine");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "labTests");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "labTests");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "labTests");

            migrationBuilder.DropColumn(
                name: "RecordsId",
                table: "labTests");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "labTests");

            migrationBuilder.AlterColumn<int>(
                name: "prescriptionId",
                table: "Records",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LabTestId",
                table: "Records",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "notes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "notes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Records_LabTestId",
                table: "Records",
                column: "LabTestId");

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
