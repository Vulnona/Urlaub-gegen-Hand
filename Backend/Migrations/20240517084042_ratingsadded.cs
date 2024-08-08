using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class ratingsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "reviews");

            //migrationBuilder.CreateTable(
            //    name: "ratings",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Rating = table.Column<int>(type: "int", nullable: false),
            //        ReviewText = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        SubmissionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //        UserId = table.Column<int>(name: "User_Id", type: "int", nullable: false),
            //        OfferId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ratings", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ratings_offers_OfferId",
            //            column: x => x.OfferId,
            //            principalTable: "offers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ratings_users_User_Id",
            //            column: x => x.UserId,
            //            principalTable: "users",
            //            principalColumn: "User_Id",
            //            onDelete: ReferentialAction.Cascade);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ratings_OfferId",
            //    table: "ratings",
            //    column: "OfferId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ratings_User_Id",
            //    table: "ratings",
            //    column: "User_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "ratings");

            //migrationBuilder.CreateTable(
            //    name: "reviews",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        OfferId = table.Column<int>(type: "int", nullable: false),
            //        UserId = table.Column<int>(name: "User_Id", type: "int", nullable: false),
            //        Rating = table.Column<int>(type: "int", nullable: false),
            //        ReviewText = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        SubmissionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_reviews", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_reviews_offers_OfferId",
            //            column: x => x.OfferId,
            //            principalTable: "offers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_reviews_users_User_Id",
            //            column: x => x.UserId,
            //            principalTable: "users",
            //            principalColumn: "User_Id",
            //            onDelete: ReferentialAction.Cascade);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateIndex(
            //    name: "IX_reviews_OfferId",
            //    table: "reviews",
            //    column: "OfferId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_reviews_User_Id",
            //    table: "reviews",
            //    column: "User_Id");
        }
    }
}
