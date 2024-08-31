using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPreviousColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            //migrationBuilder.AddColumn<string>(
            //   name: "Link_RS",
            //   table: "users",
            //   type: "longtext",
            //   nullable: true)
            //   .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddColumn<string>(
            //   name: "Link_VS",
            //   table: "users",
            //   type: "longtext",
            //   nullable: true)
            //   .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Link",
            //    table: "users");
        }
    }
}
