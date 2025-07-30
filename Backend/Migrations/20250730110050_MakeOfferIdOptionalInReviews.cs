using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class MakeOfferIdOptionalInReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_offers_pictures_PictureId",
                table: "offers");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_offers_OfferId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_users_addresses_AddressId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_offers_PictureId",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "offers");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FailedBackupCodeAttempts",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBackupCodeLocked",
                table: "users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastFailedBackupCodeAttempt",
                table: "users",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OfferId",
                table: "reviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OfferId1",
                table: "reviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OfferId",
                table: "pictures",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_reviews_OfferId1",
                table: "reviews",
                column: "OfferId1");

            migrationBuilder.CreateIndex(
                name: "IX_pictures_OfferId",
                table: "pictures",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_pictures_offers_OfferId",
                table: "pictures",
                column: "OfferId",
                principalTable: "offers",
                principalColumn: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_users_addresses_AddressId",
                table: "users",
                column: "AddressId",
                principalTable: "addresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pictures_offers_OfferId",
                table: "pictures");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_offers_OfferId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_offers_OfferId1",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_users_addresses_AddressId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_reviews_OfferId1",
                table: "reviews");

            migrationBuilder.DropIndex(
                name: "IX_pictures_OfferId",
                table: "pictures");

            migrationBuilder.DropColumn(
                name: "FailedBackupCodeAttempts",
                table: "users");

            migrationBuilder.DropColumn(
                name: "IsBackupCodeLocked",
                table: "users");

            migrationBuilder.DropColumn(
                name: "LastFailedBackupCodeAttempt",
                table: "users");

            migrationBuilder.DropColumn(
                name: "OfferId1",
                table: "reviews");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "pictures");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OfferId",
                table: "reviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PictureId",
                table: "offers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_offers_PictureId",
                table: "offers",
                column: "PictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_offers_pictures_PictureId",
                table: "offers",
                column: "PictureId",
                principalTable: "pictures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_offers_OfferId",
                table: "reviews",
                column: "OfferId",
                principalTable: "offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_addresses_AddressId",
                table: "users",
                column: "AddressId",
                principalTable: "addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
