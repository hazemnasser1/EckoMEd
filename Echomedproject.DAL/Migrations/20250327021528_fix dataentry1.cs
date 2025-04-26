using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class fixdataentry1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_dataEntry_DataEntryID",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_hospitals_HospitalsHospitalID",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "HospitalsHospitalID",
                table: "Patients",
                newName: "HospitalsId");

            migrationBuilder.RenameIndex(
                name: "IX_Patients_HospitalsHospitalID",
                table: "Patients",
                newName: "IX_Patients_HospitalsId");

            migrationBuilder.RenameColumn(
                name: "HospitalID",
                table: "hospitals",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DataEntryID",
                table: "Departments",
                newName: "DataEntryId");

            migrationBuilder.RenameColumn(
                name: "DepartmentID",
                table: "Departments",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_DataEntryID",
                table: "Departments",
                newName: "IX_Departments_DataEntryId");

            migrationBuilder.RenameColumn(
                name: "DataEntryID",
                table: "dataEntry",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_dataEntry_DataEntryId",
                table: "Departments",
                column: "DataEntryId",
                principalTable: "dataEntry",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_hospitals_HospitalsId",
                table: "Patients",
                column: "HospitalsId",
                principalTable: "hospitals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_dataEntry_DataEntryId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_hospitals_HospitalsId",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "HospitalsId",
                table: "Patients",
                newName: "HospitalsHospitalID");

            migrationBuilder.RenameIndex(
                name: "IX_Patients_HospitalsId",
                table: "Patients",
                newName: "IX_Patients_HospitalsHospitalID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "hospitals",
                newName: "HospitalID");

            migrationBuilder.RenameColumn(
                name: "DataEntryId",
                table: "Departments",
                newName: "DataEntryID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Departments",
                newName: "DepartmentID");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_DataEntryId",
                table: "Departments",
                newName: "IX_Departments_DataEntryID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "dataEntry",
                newName: "DataEntryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_dataEntry_DataEntryID",
                table: "Departments",
                column: "DataEntryID",
                principalTable: "dataEntry",
                principalColumn: "DataEntryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_hospitals_HospitalsHospitalID",
                table: "Patients",
                column: "HospitalsHospitalID",
                principalTable: "hospitals",
                principalColumn: "HospitalID");
        }
    }
}
