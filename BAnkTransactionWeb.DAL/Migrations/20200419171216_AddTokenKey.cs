using Microsoft.EntityFrameworkCore.Migrations;

namespace BankTransaction.DAL.Implementation.Migrations
{
    public partial class AddTokenKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TokenKey",
                table: "RefreshTokens",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenKey",
                table: "RefreshTokens");
        }
    }
}
