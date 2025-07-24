using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class OfferRedesign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_offers_users_HostId",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "Accomodation",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "offers");

            migrationBuilder.RenameColumn(
                name: "skills",
                table: "offers",
                newName: "Skills");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "offers",
                newName: "SpecialConditions");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "offers",
                newName: "Requirements");

            migrationBuilder.RenameColumn(
                name: "city",
                table: "offers",
                newName: "PossibleLocations");

            migrationBuilder.RenameColumn(
                name: "accomodationsuitable",
                table: "offers",
                newName: "LodgingType");

            migrationBuilder.RenameColumn(
                name: "ImageMimeType",
                table: "offers",
                newName: "GroupProperties");

            migrationBuilder.RenameColumn(
                name: "HostId",
                table: "offers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Contact",
                table: "offers",
                newName: "AdditionalLodgingProperties");

            migrationBuilder.RenameIndex(
                name: "IX_offers_HostId",
                table: "offers",
                newName: "IX_offers_UserId");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "offers",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "offers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FromDate",
                table: "offers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "GroupSize",
                table: "offers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mobility",
                table: "offers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ModifiedAt",
                table: "offers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "OfferType",
                table: "offers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PictureId",
                table: "offers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "offers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ToDate",
                table: "offers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateTable(
                name: "pictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Hash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Width = table.Column<int>(type: "int", nullable: false),
                    ImageData = table.Column<byte[]>(type: "longblob", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pictures_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_offers_PictureId",
                table: "offers",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_pictures_UserId",
                table: "pictures",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_offers_pictures_PictureId",
                table: "offers",
                column: "PictureId",
                principalTable: "pictures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_offers_users_UserId",
                table: "offers",
                column: "UserId",
                principalTable: "users",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_offers_pictures_PictureId",
                table: "offers");

            migrationBuilder.DropForeignKey(
                name: "FK_offers_users_UserId",
                table: "offers");

            migrationBuilder.DropTable(
                name: "pictures");

            migrationBuilder.DropIndex(
                name: "IX_offers_PictureId",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "GroupSize",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "Mobility",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "OfferType",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "offers");

            migrationBuilder.RenameColumn(
                name: "Skills",
                table: "offers",
                newName: "skills");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "offers",
                newName: "HostId");

            migrationBuilder.RenameColumn(
                name: "SpecialConditions",
                table: "offers",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "Requirements",
                table: "offers",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "PossibleLocations",
                table: "offers",
                newName: "city");

            migrationBuilder.RenameColumn(
                name: "LodgingType",
                table: "offers",
                newName: "accomodationsuitable");

            migrationBuilder.RenameColumn(
                name: "GroupProperties",
                table: "offers",
                newName: "ImageMimeType");

            migrationBuilder.RenameColumn(
                name: "AdditionalLodgingProperties",
                table: "offers",
                newName: "Contact");

            migrationBuilder.RenameIndex(
                name: "IX_offers_UserId",
                table: "offers",
                newName: "IX_offers_HostId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "offers",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "Accomodation",
                table: "offers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "offers",
                type: "longblob",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_offers_users_HostId",
                table: "offers",
                column: "HostId",
                principalTable: "users",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
