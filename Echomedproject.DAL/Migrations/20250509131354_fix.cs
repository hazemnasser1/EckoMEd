using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicine_prescriptions_prescriptionId",
                table: "Medicine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Medicine",
                table: "Medicine");

            migrationBuilder.RenameTable(
                name: "Medicine",
                newName: "Midicine");

            migrationBuilder.RenameIndex(
                name: "IX_Medicine_prescriptionId",
                table: "Midicine",
                newName: "IX_Midicine_prescriptionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Midicine",
                table: "Midicine",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Midicine_prescriptions_prescriptionId",
                table: "Midicine",
                column: "prescriptionId",
                principalTable: "prescriptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Midicine_prescriptions_prescriptionId",
                table: "Midicine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Midicine",
                table: "Midicine");

            migrationBuilder.RenameTable(
                name: "Midicine",
                newName: "Medicine");

            migrationBuilder.RenameIndex(
                name: "IX_Midicine_prescriptionId",
                table: "Medicine",
                newName: "IX_Medicine_prescriptionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medicine",
                table: "Medicine",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicine_prescriptions_prescriptionId",
                table: "Medicine",
                column: "prescriptionId",
                principalTable: "prescriptions",
                principalColumn: "Id");
        }
    }
}
