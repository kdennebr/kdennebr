using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace loyalty_application.Migrations
{
    /// <inheritdoc />
    public partial class AddPromotions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PointsSpent",
                table: "Redemptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RedemptionId",
                table: "PointLedgerEntries",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    PromotionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    PromotionType = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: true),
                    Multiplier = table.Column<decimal>(type: "TEXT", nullable: true),
                    BonusPoints = table.Column<int>(type: "INTEGER", nullable: true),
                    MinimumSpend = table.Column<decimal>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.PromotionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointLedgerEntries_RedemptionId",
                table: "PointLedgerEntries",
                column: "RedemptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointLedgerEntries_Redemptions_RedemptionId",
                table: "PointLedgerEntries",
                column: "RedemptionId",
                principalTable: "Redemptions",
                principalColumn: "RedemptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointLedgerEntries_Redemptions_RedemptionId",
                table: "PointLedgerEntries");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_PointLedgerEntries_RedemptionId",
                table: "PointLedgerEntries");

            migrationBuilder.DropColumn(
                name: "PointsSpent",
                table: "Redemptions");

            migrationBuilder.DropColumn(
                name: "RedemptionId",
                table: "PointLedgerEntries");
        }
    }
}
