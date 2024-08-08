using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPostReviewModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Postreviews",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        reviewofferusersID = table.Column<int>(type: "int", nullable: false),
            //        reviewloginusersID = table.Column<int>(type: "int", nullable: false),
            //        ReviewPost = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Postreviews", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Postreviews_reviewloginusers_reviewloginusersID",
            //            column: x => x.reviewloginusersID,
            //            principalTable: "reviewloginusers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Postreviews_reviewofferusers_reviewofferusersID",
            //            column: x => x.reviewofferusersID,
            //            principalTable: "reviewofferusers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Postreviews_reviewloginusersID",
            //    table: "Postreviews",
            //    column: "reviewloginusersID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Postreviews_reviewofferusersID",
            //    table: "Postreviews",
            //    column: "reviewofferusersID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Postreviews");
        }
    }
}
