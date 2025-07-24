using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedShopItemandtransactiontableupdatedcoupontable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "coupons");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "coupons");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "coupons",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "coupons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MembershipId",
                table: "coupons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "shopitems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    PriceAmount = table.Column<decimal>(name: "Price_Amount", type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PriceCurrency = table.Column<string>(name: "Price_Currency", type: "varchar(3)", maxLength: 3, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopitems", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "transaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TransactionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AmountValue = table.Column<decimal>(name: "Amount_Value", type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    AmountCurrency = table.Column<string>(name: "Amount_Currency", type: "varchar(3)", maxLength: 3, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShopItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TransactionId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CouponId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transaction_coupons_CouponId",
                        column: x => x.CouponId,
                        principalTable: "coupons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_transaction_shopitems_ShopItemId",
                        column: x => x.ShopItemId,
                        principalTable: "shopitems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transaction_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_coupons_CreatedBy",
                table: "coupons",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_coupons_MembershipId",
                table: "coupons",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_CouponId",
                table: "transaction",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_ShopItemId",
                table: "transaction",
                column: "ShopItemId");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_UserId",
                table: "transaction",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_coupons_memberships_MembershipId",
                table: "coupons",
                column: "MembershipId",
                principalTable: "memberships",
                principalColumn: "MembershipID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_coupons_users_CreatedBy",
                table: "coupons",
                column: "CreatedBy",
                principalTable: "users",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_coupons_memberships_MembershipId",
                table: "coupons");

            migrationBuilder.DropForeignKey(
                name: "FK_coupons_users_CreatedBy",
                table: "coupons");

            migrationBuilder.DropTable(
                name: "transaction");

            migrationBuilder.DropTable(
                name: "shopitems");

            migrationBuilder.DropIndex(
                name: "IX_coupons_CreatedBy",
                table: "coupons");

            migrationBuilder.DropIndex(
                name: "IX_coupons_MembershipId",
                table: "coupons");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "coupons");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "coupons");

            migrationBuilder.DropColumn(
                name: "MembershipId",
                table: "coupons");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "coupons",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "coupons",
                type: "datetime(6)",
                nullable: true);
        }
    }
}
