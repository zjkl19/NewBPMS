using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewBPMS.Data.Migrations
{
    public partial class AddAcceptUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptDateTime",
                table: "Contracts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "AcceptUserId",
                table: "Contracts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcceptUserName",
                table: "Contracts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_AcceptUserId",
                table: "Contracts",
                column: "AcceptUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_AspNetUsers_AcceptUserId",
                table: "Contracts",
                column: "AcceptUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_AspNetUsers_AcceptUserId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_AcceptUserId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "AcceptDateTime",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "AcceptUserId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "AcceptUserName",
                table: "Contracts");
        }
    }
}
