using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewBPMS.Data.Migrations
{
    public partial class UpgradeContractField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CheckDateTime",
                table: "Contracts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CheckStatus",
                table: "Contracts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CheckUserName",
                table: "Contracts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewDateTime",
                table: "Contracts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ReviewUserName",
                table: "Contracts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckDateTime",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CheckStatus",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CheckUserName",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ReviewDateTime",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ReviewUserName",
                table: "Contracts");
        }
    }
}
