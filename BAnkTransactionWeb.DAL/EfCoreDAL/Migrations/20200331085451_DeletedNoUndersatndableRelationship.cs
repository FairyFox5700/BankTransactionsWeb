using Microsoft.EntityFrameworkCore.Migrations;

namespace BankTransactionWeb.DAL.Migrations
{
    public partial class DeletedNoUndersatndableRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_AccountDestinationId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_AccountSourceId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AccountDestinationId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AccountSourceId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "SourceAccountId",
                table: "Transactions",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AccountDestinationId", "AccountSourceId" },
                values: new object[] { 2, 1 });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AccountDestinationId", "AccountSourceId" },
                values: new object[] { 2, 1 });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AccountDestinationId", "AccountSourceId" },
                values: new object[] { 2, 2 });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AccountDestinationId", "AccountSourceId" },
                values: new object[] { 2, 1 });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AccountDestinationId", "AccountSourceId" },
                values: new object[] { 2, 4 });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AccountDestinationId", "AccountSourceId" },
                values: new object[] { 2, 3 });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SourceAccountId",
                table: "Transactions",
                column: "SourceAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_SourceAccountId",
                table: "Transactions",
                column: "SourceAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_SourceAccountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SourceAccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SourceAccountId",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AccountDestinationId", "AccountSourceId", "Number" },
                values: new object[] { null, null, "00000000" });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AccountDestinationId", "AccountSourceId", "Number" },
                values: new object[] { null, null, "00000001" });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AccountDestinationId", "AccountSourceId", "Number" },
                values: new object[] { null, null, "00000002" });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AccountDestinationId", "AccountSourceId", "Number" },
                values: new object[] { null, null, "00000003" });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AccountDestinationId", "AccountSourceId", "Number" },
                values: new object[] { null, null, "00000004" });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AccountDestinationId", "AccountSourceId", "Number" },
                values: new object[] { null, null, "00000005" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountDestinationId",
                table: "Transactions",
                column: "AccountDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountSourceId",
                table: "Transactions",
                column: "AccountSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_AccountDestinationId",
                table: "Transactions",
                column: "AccountDestinationId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_AccountSourceId",
                table: "Transactions",
                column: "AccountSourceId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
