using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class nullfixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_prescriptions_prescriptionId",
                table: "Records");

            migrationBuilder.AlterColumn<int>(
                name: "prescriptionId",
                table: "Records",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_prescriptions_prescriptionId",
                table: "Records",
                column: "prescriptionId",
                principalTable: "prescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_prescriptions_prescriptionId",
                table: "Records");

            migrationBuilder.AlterColumn<int>(
                name: "prescriptionId",
                table: "Records",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_prescriptions_prescriptionId",
                table: "Records",
                column: "prescriptionId",
                principalTable: "prescriptions",
                principalColumn: "Id");
        }
    }
}
