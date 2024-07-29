using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class changeinModelReviewOfferUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Postreviews_reviewloginusers_reviewloginusersID",
                table: "Postreviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Postreviews_reviewofferusers_reviewofferusersID",
                table: "Postreviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviewofferusers_users_UserId",
                table: "reviewofferusers");

            migrationBuilder.DropIndex(
                name: "IX_reviewofferusers_UserId",
                table: "reviewofferusers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "reviewofferusers");

            migrationBuilder.AlterColumn<int>(
                name: "reviewofferusersID",
                table: "Postreviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "reviewloginusersID",
                table: "Postreviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Postreviews_reviewloginusers_reviewloginusersID",
                table: "Postreviews",
                column: "reviewloginusersID",
                principalTable: "reviewloginusers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Postreviews_reviewofferusers_reviewofferusersID",
                table: "Postreviews",
                column: "reviewofferusersID",
                principalTable: "reviewofferusers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Postreviews_reviewloginusers_reviewloginusersID",
                table: "Postreviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Postreviews_reviewofferusers_reviewofferusersID",
                table: "Postreviews");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "reviewofferusers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "reviewofferusersID",
                table: "Postreviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "reviewloginusersID",
                table: "Postreviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_reviewofferusers_UserId",
                table: "reviewofferusers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Postreviews_reviewloginusers_reviewloginusersID",
                table: "Postreviews",
                column: "reviewloginusersID",
                principalTable: "reviewloginusers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Postreviews_reviewofferusers_reviewofferusersID",
                table: "Postreviews",
                column: "reviewofferusersID",
                principalTable: "reviewofferusers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reviewofferusers_users_UserId",
                table: "reviewofferusers",
                column: "UserId",
                principalTable: "users",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
