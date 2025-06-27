using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class requests2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notifications_Users_AppUsersId",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_Users_AppUserId",
                table: "Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_pharmacyAccs_pharmacyAccId",
                table: "Request");

            migrationBuilder.DropPrimaryKey(
                name: "PK_notifications",
                table: "notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Request",
                table: "Request");

            migrationBuilder.RenameTable(
                name: "notifications",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "Request",
                newName: "requests");

            migrationBuilder.RenameIndex(
                name: "IX_notifications_AppUsersId",
                table: "Notifications",
                newName: "IX_Notifications_AppUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_Request_pharmacyAccId",
                table: "requests",
                newName: "IX_requests_pharmacyAccId");

            migrationBuilder.RenameIndex(
                name: "IX_Request_AppUserId",
                table: "requests",
                newName: "IX_requests_AppUserId");

            migrationBuilder.AddColumn<string>(
                name: "PharmacyName",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_requests",
                table: "requests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_AppUsersId",
                table: "Notifications",
                column: "AppUsersId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_requests_Users_AppUserId",
                table: "requests",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_requests_pharmacyAccs_pharmacyAccId",
                table: "requests",
                column: "pharmacyAccId",
                principalTable: "pharmacyAccs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_AppUsersId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_requests_Users_AppUserId",
                table: "requests");

            migrationBuilder.DropForeignKey(
                name: "FK_requests_pharmacyAccs_pharmacyAccId",
                table: "requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_requests",
                table: "requests");

            migrationBuilder.DropColumn(
                name: "PharmacyName",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "notifications");

            migrationBuilder.RenameTable(
                name: "requests",
                newName: "Request");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_AppUsersId",
                table: "notifications",
                newName: "IX_notifications_AppUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_requests_pharmacyAccId",
                table: "Request",
                newName: "IX_Request_pharmacyAccId");

            migrationBuilder.RenameIndex(
                name: "IX_requests_AppUserId",
                table: "Request",
                newName: "IX_Request_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_notifications",
                table: "notifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Request",
                table: "Request",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_notifications_Users_AppUsersId",
                table: "notifications",
                column: "AppUsersId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Users_AppUserId",
                table: "Request",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_pharmacyAccs_pharmacyAccId",
                table: "Request",
                column: "pharmacyAccId",
                principalTable: "pharmacyAccs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
