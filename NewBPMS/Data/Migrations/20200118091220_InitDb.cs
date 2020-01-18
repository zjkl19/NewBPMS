using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewBPMS.Data.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "StaffId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StaffName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StaffNo",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    No = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    TotalAmount = table.Column<decimal>(type: "money", nullable: false),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    SignedDate = table.Column<DateTime>(nullable: false),
                    Deadline = table.Column<int>(nullable: false),
                    PromisedDeadline = table.Column<int>(nullable: false),
                    JobContent = table.Column<string>(nullable: true),
                    ProjectLocation = table.Column<string>(nullable: true),
                    Client = table.Column<string>(nullable: true),
                    ClientContactPerson = table.Column<string>(nullable: true),
                    ClientContactPersonPhone = table.Column<string>(nullable: true),
                    AcceptWay = table.Column<int>(nullable: false),
                    SignStatus = table.Column<int>(nullable: false),
                    SubmitStatus = table.Column<int>(nullable: false),
                    SubmitDateTime = table.Column<DateTime>(nullable: false),
                    ChangeLog = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserContracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Labor = table.Column<int>(nullable: false),
                    Ratio = table.Column<decimal>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    ContractId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContracts_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContracts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_UserId",
                table: "Contracts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContracts_ContractId",
                table: "UserContracts",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContracts_UserId",
                table: "UserContracts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserContracts");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StaffName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StaffNo",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
