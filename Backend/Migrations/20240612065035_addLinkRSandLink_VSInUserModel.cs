using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class addLinkRSandLinkVSInUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Link",
                table: "users",
                newName: "Link_VS");

            migrationBuilder.AddColumn<string>(
                name: "Link_RS",
                table: "users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link_RS",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "Link_VS",
                table: "users",
                newName: "Link");
        }
    }
}
