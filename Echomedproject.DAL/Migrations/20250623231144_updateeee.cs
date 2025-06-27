using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateeee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmitted",
                table: "patientHospital");

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "patientHospital",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "patientHospital",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "recordId",
                table: "patientHospital",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_patientHospital_recordId",
                table: "patientHospital",
                column: "recordId");

            migrationBuilder.AddForeignKey(
                name: "FK_patientHospital_Records_recordId",
                table: "patientHospital",
                column: "recordId",
                principalTable: "Records",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patientHospital_Records_recordId",
                table: "patientHospital");

            migrationBuilder.DropIndex(
                name: "IX_patientHospital_recordId",
                table: "patientHospital");

            migrationBuilder.DropColumn(
                name: "State",
                table: "patientHospital");

            migrationBuilder.DropColumn(
                name: "recordId",
                table: "patientHospital");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "patientHospital",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmitted",
                table: "patientHospital",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
