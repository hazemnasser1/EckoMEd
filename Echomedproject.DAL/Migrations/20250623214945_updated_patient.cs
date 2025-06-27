using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updated_patient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_hospitals_HospitalsId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_HospitalsId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "HospitalsId",
                table: "Patients");

            migrationBuilder.AddColumn<int>(
                name: "HospitalsId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "patientHospital",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_HospitalsId",
                table: "Users",
                column: "HospitalsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_hospitals_HospitalsId",
                table: "Users",
                column: "HospitalsId",
                principalTable: "hospitals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_hospitals_HospitalsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_HospitalsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HospitalsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "patientHospital");

            migrationBuilder.AddColumn<int>(
                name: "HospitalsId",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_HospitalsId",
                table: "Patients",
                column: "HospitalsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_hospitals_HospitalsId",
                table: "Patients",
                column: "HospitalsId",
                principalTable: "hospitals",
                principalColumn: "Id");
        }
    }
}
