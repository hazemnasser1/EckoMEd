using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class requests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notifications_pharmacyAccs_PharmacyAccId",
                table: "notifications");

            migrationBuilder.DropIndex(
                name: "IX_notifications_PharmacyAccId",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "PharmacyAccId",
                table: "notifications");

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pharmacyAccId = table.Column<int>(type: "int", nullable: false),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    MedicineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    qty = table.Column<int>(type: "int", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Request_Users_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Request_pharmacyAccs_pharmacyAccId",
                        column: x => x.pharmacyAccId,
                        principalTable: "pharmacyAccs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Request_AppUserId",
                table: "Request",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_pharmacyAccId",
                table: "Request",
                column: "pharmacyAccId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.AddColumn<int>(
                name: "PharmacyAccId",
                table: "notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_notifications_PharmacyAccId",
                table: "notifications",
                column: "PharmacyAccId");

            migrationBuilder.AddForeignKey(
                name: "FK_notifications_pharmacyAccs_PharmacyAccId",
                table: "notifications",
                column: "PharmacyAccId",
                principalTable: "pharmacyAccs",
                principalColumn: "Id");
        }
    }
}
