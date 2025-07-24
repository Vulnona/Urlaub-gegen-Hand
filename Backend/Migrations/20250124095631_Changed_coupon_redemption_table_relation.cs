using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class Changedcouponredemptiontablerelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_redemptions_Coupons_CouponId",
                table: "redemptions"
            );

            // Drop the index
            migrationBuilder.DropIndex(name: "IX_redemptions_CouponId", table: "redemptions");

            // Create the new unique index
            migrationBuilder.CreateIndex(
                name: "IX_redemptions_CouponId",
                table: "redemptions",
                column: "CouponId",
                unique: true
            );

            // Add back the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_redemptions_Coupons_CouponId",
                table: "redemptions",
                column: "CouponId",
                principalTable: "Coupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the unique index
            migrationBuilder.DropIndex(name: "IX_redemptions_CouponId", table: "redemptions");

            // Recreate the original index
            migrationBuilder.CreateIndex(
                name: "IX_redemptions_CouponId",
                table: "redemptions",
                column: "CouponId"
            );

            // Add back the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_redemptions_Coupons_CouponId",
                table: "redemptions",
                column: "CouponId",
                principalTable: "Coupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
