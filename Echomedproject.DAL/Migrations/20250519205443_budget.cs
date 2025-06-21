﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echomedproject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class budget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "budget",
                table: "Departments",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "budget",
                table: "Departments");
        }
    }
}
