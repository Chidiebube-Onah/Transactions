using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transactions.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TransactionHash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TransactionStatus = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TransactionHashUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToAddressUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromAddressUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Network = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TransactionTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionUpdateRequests",
                columns: table => new
                {
                    RequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TransactionHash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WalletAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionUpdateRequests", x => x.RequestId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreatedAt",
                table: "Transactions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Currency",
                table: "Transactions",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FromAddress",
                table: "Transactions",
                column: "FromAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Network",
                table: "Transactions",
                column: "Network");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionHash",
                table: "Transactions",
                column: "TransactionHash");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionStatus",
                table: "Transactions",
                column: "TransactionStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionType",
                table: "Transactions",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionUpdateRequests_ClientId",
                table: "TransactionUpdateRequests",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionUpdateRequests_TransactionHash",
                table: "TransactionUpdateRequests",
                column: "TransactionHash");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionUpdateRequests_WalletAddress",
                table: "TransactionUpdateRequests",
                column: "WalletAddress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionUpdateRequests");
        }
    }
}
