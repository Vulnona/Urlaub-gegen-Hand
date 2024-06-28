using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class revertchangeinModelReviewOfferUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "reviewofferusers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_reviewofferusers_UserId",
                table: "reviewofferusers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_reviewofferusers_users_UserId",
                table: "reviewofferusers",
                column: "UserId",
                principalTable: "users",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reviewofferusers_users_UserId",
                table: "reviewofferusers");

            migrationBuilder.DropIndex(
                name: "IX_reviewofferusers_UserId",
                table: "reviewofferusers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "reviewofferusers");
        }
    }
}
