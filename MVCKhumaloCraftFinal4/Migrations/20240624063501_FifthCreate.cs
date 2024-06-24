using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCKhumaloCraftFinal4.Migrations
{
    /// <inheritdoc />
    public partial class FifthCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enable2FA",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorCode",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "TwoFactorCodeExpiration",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enable2FA",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TwoFactorCode",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TwoFactorCodeExpiration",
                table: "User");
        }
    }
}
