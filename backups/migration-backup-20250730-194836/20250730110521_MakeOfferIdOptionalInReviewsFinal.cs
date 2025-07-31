using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class MakeOfferIdOptionalInReviewsFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reviews_offers_OfferId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_offers_OfferId1",
                table: "reviews");

            migrationBuilder.DropIndex(
                name: "IX_reviews_OfferId1",
                table: "reviews");

            migrationBuilder.DropColumn(
                name: "OfferId1",
                table: "reviews");

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_offers_OfferId",
                table: "reviews",
                column: "OfferId",
                principalTable: "offers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reviews_offers_OfferId",
                table: "reviews");

            migrationBuilder.AddColumn<int>(
                name: "OfferId1",
                table: "reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_reviews_OfferId1",
                table: "reviews",
                column: "OfferId1");

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_offers_OfferId",
                table: "reviews",
                column: "OfferId",
                principalTable: "offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_offers_OfferId1",
                table: "reviews",
                column: "OfferId1",
                principalTable: "offers",
                principalColumn: "Id");
        }
    }
}
