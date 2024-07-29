using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class changeinPostReviewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReviewPost",
                table: "Postreviews",
                newName: "OfferUserReviewPost");

            migrationBuilder.AddColumn<string>(
                name: "LoginUserReviewPost",
                table: "Postreviews",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginUserReviewPost",
                table: "Postreviews");

            migrationBuilder.RenameColumn(
                name: "OfferUserReviewPost",
                table: "Postreviews",
                newName: "ReviewPost");
        }
    }
}
