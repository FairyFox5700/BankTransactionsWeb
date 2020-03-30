using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankTransactionWeb.DAL.Migrations
{
    public partial class SeedDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "DateOfCreation", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2001, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "AbstractBank" },
                    { 2, new DateTime(1997, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aval" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "DataOfBirth", "LastName", "Name", "Surname" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Володимирович", "Андрій", "Коваленко" },
                    { 2, new DateTime(1930, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Volodymirovich", "Vasil", "Kondratyuk" },
                    { 3, new DateTime(1987, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Olegivna", "Masha", "Koshova" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "AccountDestinationId", "AccountSourceId", "Amount", "DateOftransfering", "Number" },
                values: new object[,]
                {
                    { 1, null, null, 300m, new DateTime(2004, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "00000000" },
                    { 2, null, null, 70m, new DateTime(2006, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "00000001" },
                    { 3, null, null, 70m, new DateTime(2009, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "00000002" },
                    { 4, null, null, 34m, new DateTime(2004, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "00000003" },
                    { 5, null, null, 900m, new DateTime(2004, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "00000004" },
                    { 6, null, null, 800m, new DateTime(2004, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "00000005" }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Balance", "Number", "PersonId" },
                values: new object[,]
                {
                    { 1, 23m, "9235286739025463", 1 },
                    { 2, 2000m, "3289456729015682", 2 },
                    { 3, 300m, "3782569106739028", 2 },
                    { 4, 0m, "2345678567891253", 3 }
                });

            migrationBuilder.InsertData(
                table: "Shareholders",
                columns: new[] { "Id", "CompanyId", "PersonId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 },
                    { 3, 2, 3 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Shareholders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Shareholders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Shareholders",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
