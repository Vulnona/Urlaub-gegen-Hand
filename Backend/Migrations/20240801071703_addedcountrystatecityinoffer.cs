using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class addedcountrystatecityinoffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "offers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "offers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "offers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.Sql(@"
                INSERT INTO `db`.`states` (`Id`, `Name`, `CountryId`) VALUES 
                ('1', 'Baden-Württemberg', '1'),
                ('2', 'Bavaria (Bayern)', '1'),
                ('3', 'Berlin', '1'),
                ('4', 'Brandenburg', '1'),
                ('5', 'Bremen', '1'),
                ('6', 'Hamburg', '1'),
                ('7', 'Hesse (Hessen)', '1'),
                ('8', 'Lower Saxony (Niedersachsen)', '1'),
                ('9', 'Mecklenburg-Vorpommern', '1'),
                ('10', 'North Rhine-Westphalia (Nordrhein-Westfalen)', '1'),
                ('11', 'Rhineland-Palatinate (Rheinland-Pfalz)', '1'),
                ('12', 'Saarland', '1'),
                ('13', 'Saxony (Sachsen)', '1'),
                ('14', 'Saxony-Anhalt (Sachsen-Anhalt)', '1'),
                ('15', 'Schleswig-Holstein', '1'),
                ('16', 'Thuringia (Thüringen)', '1');
            ");

            migrationBuilder.Sql(@"
                INSERT INTO `db`.`states` (`Id`, `Name`, `CountryId`) VALUES 
                ('17', 'Aargau', '2'),
                ('18', 'Appenzell Ausserrhoden', '2'),
                ('19', 'Appenzell Innerrhoden', '2'),
                ('20', 'Basel-Landschaft', '2'),
                ('21', 'Basel-Stadt', '2'),
                ('22', 'Bern', '2'),
                ('23', 'Fribourg (Freiburg)', '2'),
                ('24', 'Geneva (Genève)', '2'),
                ('25', 'Glarus', '2'),
                ('26', 'Graubünden (Grisons)', '2'),
                ('27', 'Jura', '2'),
                ('28', 'Lucerne (Luzern)', '2'),
                ('29', 'Neuchâtel', '2'),
                ('30', 'Nidwalden', '2'),
                ('31', 'Obwalden', '2'),
                ('32', 'Schaffhausen', '2'),
                ('33', 'Schwyz', '2'),
                ('34', 'Solothurn', '2'),
                ('35', 'St. Gallen', '2'),
                ('36', 'Thurgau', '2'),
                ('37', 'Ticino (Tessin)', '2'),
                ('38', 'Uri', '2'),
                ('39', 'Valais (Wallis)', '2'),
                ('40', 'Vaud', '2'),
                ('41', 'Zug', '2'),
                ('42', 'Zurich (Zürich)', '2');
            ");

            migrationBuilder.Sql(@"
                INSERT INTO `db`.`states` (`Id`, `Name`, `CountryId`) VALUES 
                ('43', 'Burgenland', '3'),
                ('44', 'Carinthia (Kärnten)', '3'),
                ('45', 'Lower Austria (Niederösterreich)', '3'),
                ('46', 'Upper Austria (Oberösterreich)', '3'),
                ('47', 'Salzburg', '3'),
                ('48', 'Styria (Steiermark)', '3'),
                ('49', 'Tyrol (Tirol)', '3'),
                ('50', 'Vorarlberg', '3'),
                ('51', 'Vienna (Wien)', '3');
            ");

            migrationBuilder.Sql(@"
                INSERT INTO `db`.`countries` (`Country_ID`, `CountryName`) VALUES 
                ('1', 'Germany'),
                ('2', 'Switzerland'),
                ('3', 'Austria');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "city",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "country",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "state",
                table: "offers");

            migrationBuilder.Sql(@"DELETE FROM `db`.`states` WHERE `CountryId` IN ('1', '2', '3');");
            migrationBuilder.Sql(@"DELETE FROM `db`.`countries` WHERE `Country_ID` IN ('1', '2', '3');");
        }
    }
}
