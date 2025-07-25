using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class FixAddressId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "offers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "offers");
        }
    }
}
