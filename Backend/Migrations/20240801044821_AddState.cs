using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class AddState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Postreviews");

            //migrationBuilder.DropTable(
            //    name: "Profiles");

            //migrationBuilder.AddColumn<string>(
            //    name: "Facebook_link",
            //    table: "users",
            //    type: "longtext",
            //    nullable: true)
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddColumn<string>(
            //    name: "Link_RS",
            //    table: "users",
            //    type: "longtext",
            //    nullable: true)
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddColumn<string>(
            //    name: "Link_VS",
            //    table: "users",
            //    type: "longtext",
            //    nullable: true)
            //    .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "ratinghostlogins",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        UserRating = table.Column<int>(type: "int", nullable: false),
            //        SubmissionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //        UserId = table.Column<int>(name: "User_Id", type: "int", nullable: false),
            //        OfferId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ratinghostlogins", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ratinghostlogins_offers_OfferId",
            //            column: x => x.OfferId,
            //            principalTable: "offers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ratinghostlogins_users_User_Id",
            //            column: x => x.UserId,
            //            principalTable: "users",
            //            principalColumn: "User_Id",
            //            onDelete: ReferentialAction.Cascade);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "ratinguserlogins",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        HostRating = table.Column<int>(type: "int", nullable: false),
            //        SubmissionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //        UserId = table.Column<int>(name: "User_Id", type: "int", nullable: false),
            //        OfferId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ratinguserlogins", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ratinguserlogins_offers_OfferId",
            //            column: x => x.OfferId,
            //            principalTable: "offers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ratinguserlogins_users_User_Id",
            //            column: x => x.UserId,
            //            principalTable: "users",
            //            principalColumn: "User_Id",
            //            onDelete: ReferentialAction.Cascade);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "reviewposts",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        ReviewOfferUserId = table.Column<int>(type: "int", nullable: false),
            //        ReviewLoginUserId = table.Column<int>(type: "int", nullable: false),
            //        OfferUserReviewPost = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        LoginUserReviewPost = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_reviewposts", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_reviewposts_reviewloginusers_ReviewLoginUserId",
            //            column: x => x.ReviewLoginUserId,
            //            principalTable: "reviewloginusers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_reviewposts_reviewofferusers_ReviewOfferUserId",
            //            column: x => x.ReviewOfferUserId,
            //            principalTable: "reviewofferusers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "userprofiles",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        UserId = table.Column<int>(name: "User_Id", type: "int", nullable: false),
            //        UserPic = table.Column<byte[]>(type: "longblob", nullable: true),
            //        Options = table.Column<int>(type: "int", nullable: false),
            //        Hobbies = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Token = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_userprofiles", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_userprofiles_users_User_Id",
            //            column: x => x.UserId,
            //            principalTable: "users",
            //            principalColumn: "User_Id",
            //            onDelete: ReferentialAction.Cascade);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ratinghostlogins_OfferId",
            //    table: "ratinghostlogins",
            //    column: "OfferId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ratinghostlogins_User_Id",
            //    table: "ratinghostlogins",
            //    column: "User_Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ratinguserlogins_OfferId",
            //    table: "ratinguserlogins",
            //    column: "OfferId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ratinguserlogins_User_Id",
            //    table: "ratinguserlogins",
            //    column: "User_Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_reviewposts_ReviewLoginUserId",
            //    table: "reviewposts",
            //    column: "ReviewLoginUserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_reviewposts_ReviewOfferUserId",
            //    table: "reviewposts",
            //    column: "ReviewOfferUserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_userprofiles_User_Id",
            //    table: "userprofiles",
            //    column: "User_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //    migrationBuilder.DropTable(
            //        name: "ratinghostlogins");

            //    migrationBuilder.DropTable(
            //        name: "ratinguserlogins");

            //    migrationBuilder.DropTable(
            //        name: "reviewposts");

            //    migrationBuilder.DropTable(
            //        name: "userprofiles");

            //    migrationBuilder.DropColumn(
            //        name: "Facebook_link",
            //        table: "users");

            //    migrationBuilder.DropColumn(
            //        name: "Link_RS",
            //        table: "users");

            //    migrationBuilder.DropColumn(
            //        name: "Link_VS",
            //        table: "users");

            migrationBuilder.DropColumn(
                name: "State",
                table: "users");

            //    migrationBuilder.CreateTable(
            //        name: "Postreviews",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //            reviewloginusersID = table.Column<int>(type: "int", nullable: true),
            //            reviewofferusersID = table.Column<int>(type: "int", nullable: true),
            //            CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
            //            ReviewPost = table.Column<string>(type: "longtext", nullable: true)
            //                .Annotation("MySql:CharSet", "utf8mb4")
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_Postreviews", x => x.Id);
            //            table.ForeignKey(
            //                name: "FK_Postreviews_reviewloginusers_reviewloginusersID",
            //                column: x => x.reviewloginusersID,
            //                principalTable: "reviewloginusers",
            //                principalColumn: "Id");
            //            table.ForeignKey(
            //                name: "FK_Postreviews_reviewofferusers_reviewofferusersID",
            //                column: x => x.reviewofferusersID,
            //                principalTable: "reviewofferusers",
            //                principalColumn: "Id");
            //        })
            //        .Annotation("MySql:CharSet", "utf8mb4");

            //    migrationBuilder.CreateTable(
            //        name: "Profiles",
            //        columns: table => new
            //        {
            //            ProfileID = table.Column<int>(name: "Profile_ID", type: "int", nullable: false)
            //                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //            Idcard = table.Column<string>(type: "longtext", nullable: true)
            //                .Annotation("MySql:CharSet", "utf8mb4"),
            //            NickName = table.Column<string>(type: "longtext", nullable: true)
            //                .Annotation("MySql:CharSet", "utf8mb4")
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_Profiles", x => x.ProfileID);
            //        })
            //        .Annotation("MySql:CharSet", "utf8mb4");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Postreviews_reviewloginusersID",
            //        table: "Postreviews",
            //        column: "reviewloginusersID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Postreviews_reviewofferusersID",
            //        table: "Postreviews",
            //        column: "reviewofferusersID");
        }
    }
}
