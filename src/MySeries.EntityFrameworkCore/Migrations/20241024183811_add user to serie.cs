using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySeries.Migrations
{
    /// <inheritdoc />
    public partial class addusertoserie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Creator",
                table: "Appseries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Appseries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Appseries");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Appseries");
        }
    }
}
