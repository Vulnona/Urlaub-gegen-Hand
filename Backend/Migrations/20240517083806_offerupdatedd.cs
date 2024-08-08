using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class offerupdatedd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_offers_accomodationsuitables_accomodationsuitableId",
            //    table: "offers");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_offers_accomodations_AccomodationId",
            //    table: "offers");

            //migrationBuilder.RenameColumn(
            //    name: "accomodationsuitableId",
            //    table: "offers",
            //    newName: "User_Id");

            //migrationBuilder.RenameColumn(
            //    name: "AccomodationId",
            //    table: "offers",
            //    newName: "Region_ID");

            //migrationBuilder.RenameIndex(
            //    name: "IX_offers_accomodationsuitableId",
            //    table: "offers",
            //    newName: "IX_offers_User_Id");

            //migrationBuilder.RenameIndex(
            //    name: "IX_offers_AccomodationId",
            //    table: "offers",
            //    newName: "IX_offers_Region_ID");

            //migrationBuilder.AddColumn<string>(
            //    name: "Accomodation",
            //    table: "offers",
            //    type: "longtext",
            //    nullable: false)
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddColumn<string>(
            //    name: "accomodationsuitable",
            //    table: "offers",
            //    type: "longtext",
            //    nullable: false)
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddColumn<string>(
            //    name: "Contact",
            //    table: "offers",
            //    type: "longtext",
            //    nullable: false)
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_offers_regions_Region_ID",
            //    table: "offers",
            //    column: "Region_ID",
            //    principalTable: "regions",
            //    principalColumn: "Region_ID",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_offers_users_User_Id",
            //    table: "offers",
            //    column: "User_Id",
            //    principalTable: "users",
            //    principalColumn: "User_Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_offers_regions_Region_ID",
            //    table: "offers");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_offers_users_User_Id",
            //    table: "offers");

            //migrationBuilder.DropColumn(
            //    name: "Accomodation",
            //    table: "offers");

            //migrationBuilder.DropColumn(
            //    name: "accomodationsuitable",
            //    table: "offers");

            //migrationBuilder.DropColumn(
            //    name: "Contact",
            //    table: "offers");

            //migrationBuilder.RenameColumn(
            //    name: "User_Id",
            //    table: "offers",
            //    newName: "accomodationsuitableId");

            //migrationBuilder.RenameColumn(
            //    name: "Region_ID",
            //    table: "offers",
            //    newName: "AccomodationId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_offers_User_Id",
            //    table: "offers",
            //    newName: "IX_offers_accomodationsuitableId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_offers_Region_ID",
            //    table: "offers",
            //    newName: "IX_offers_AccomodationId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_offers_accomodationsuitables_accomodationsuitableId",
            //    table: "offers",
            //    column: "accomodationsuitableId",
            //    principalTable: "accomodationsuitables",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_offers_accomodations_AccomodationId",
            //    table: "offers",
            //    column: "AccomodationId",
            //    principalTable: "accomodations",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
