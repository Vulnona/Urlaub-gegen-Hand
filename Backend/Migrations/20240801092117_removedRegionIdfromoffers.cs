using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class removedRegionIdfromoffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_offers_regions_Region_ID",
                table: "offers");

            migrationBuilder.DropIndex(
                name: "IX_offers_Region_ID",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "Region_ID",
                table: "offers");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Region_ID",
                table: "offers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_offers_Region_ID",
                table: "offers",
                column: "Region_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_offers_regions_Region_ID",
                table: "offers",
                column: "Region_ID",
                principalTable: "regions",
                principalColumn: "Region_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
