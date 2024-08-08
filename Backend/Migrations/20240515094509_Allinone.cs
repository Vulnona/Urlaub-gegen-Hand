using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class Allinone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            //migrationBuilder.CreateTable(
            //    name: "accomodations",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        NameAccomodationType = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_accomodations", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "accomodationsuitables",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Name = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_accomodationsuitables", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "offers",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Title = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Description = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Location = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Images = table.Column<byte[]>(type: "longblob", nullable: true),
            //        SkillID = table.Column<int>(name: "Skill_ID", type: "int", nullable: false),
            //        AccomodationId = table.Column<int>(type: "int", nullable: false),
            //        accomodationsuitableId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_offers", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_offers_accomodationsuitables_accomodationsuitableId",
            //            column: x => x.accomodationsuitableId,
            //            principalTable: "accomodationsuitables",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_offers_accomodations_AccomodationId",
            //            column: x => x.AccomodationId,
            //            principalTable: "accomodations",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_offers_skills_Skill_ID",
            //            column: x => x.SkillID,
            //            principalTable: "skills",
            //            principalColumn: "Skill_ID",
            //            onDelete: ReferentialAction.Cascade);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateIndex(
            //    name: "IX_offers_AccomodationId",
            //    table: "offers",
            //    column: "AccomodationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_offers_accomodationsuitableId",
            //    table: "offers",
            //    column: "accomodationsuitableId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_offers_Skill_ID",
            //    table: "offers",
            //    column: "Skill_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "offers");

            //migrationBuilder.DropTable(
            //    name: "accomodationsuitables");

            //migrationBuilder.DropTable(
            //    name: "accomodations");

            //migrationBuilder.DropColumn(
            //    name: "Idcard",
            //    table: "Profiles");

            //migrationBuilder.AddColumn<string>(
            //    name: "VisibleName",
            //    table: "users",
            //    type: "longtext",
            //    nullable: false)
            //    .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
