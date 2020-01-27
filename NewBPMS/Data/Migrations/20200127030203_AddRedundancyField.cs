using Microsoft.EntityFrameworkCore.Migrations;

namespace NewBPMS.Data.Migrations
{
    public partial class AddRedundancyField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContractName",
                table: "UserContracts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UserContracts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FinishStatus",
                table: "Contracts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReviewStatus",
                table: "Contracts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractName",
                table: "UserContracts");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "UserContracts");

            migrationBuilder.DropColumn(
                name: "FinishStatus",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ReviewStatus",
                table: "Contracts");
        }
    }
}
