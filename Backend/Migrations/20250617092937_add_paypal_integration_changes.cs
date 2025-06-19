using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class addpaypalintegrationchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "transaction",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "redemptions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "redemptions");
        }
    }
}
