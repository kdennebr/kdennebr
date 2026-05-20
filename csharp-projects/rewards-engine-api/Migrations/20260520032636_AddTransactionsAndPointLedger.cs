using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace loyalty_application.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionsAndPointLedger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseTransactions",
                columns: table => new
                {
                    PurchaseTransactionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExternalTransactionId = table.Column<string>(type: "TEXT", nullable: false),
                    Subtotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    BasePointsEarned = table.Column<int>(type: "INTEGER", nullable: false),
                    BonusPointsEarned = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPointsEarned = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseTransactions", x => x.PurchaseTransactionId);
                    table.ForeignKey(
                        name: "FK_PurchaseTransactions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PointLedgerEntries",
                columns: table => new
                {
                    PointLedgerEntryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    PointsChanged = table.Column<int>(type: "INTEGER", nullable: false),
                    PurchaseTransactionId = table.Column<int>(type: "INTEGER", nullable: true),
                    Reason = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointLedgerEntries", x => x.PointLedgerEntryId);
                    table.ForeignKey(
                        name: "FK_PointLedgerEntries_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointLedgerEntries_PurchaseTransactions_PurchaseTransactionId",
                        column: x => x.PurchaseTransactionId,
                        principalTable: "PurchaseTransactions",
                        principalColumn: "PurchaseTransactionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointLedgerEntries_CustomerId",
                table: "PointLedgerEntries",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PointLedgerEntries_PurchaseTransactionId",
                table: "PointLedgerEntries",
                column: "PurchaseTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseTransactions_CustomerId",
                table: "PurchaseTransactions",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointLedgerEntries");

            migrationBuilder.DropTable(
                name: "PurchaseTransactions");
        }
    }
}
