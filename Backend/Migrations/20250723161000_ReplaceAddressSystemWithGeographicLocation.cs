using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceAddressSystemWithGeographicLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create new addresses table
            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Latitude = table.Column<double>(type: "double", nullable: false),
                    Longitude = table.Column<double>(type: "double", nullable: false),
                    DisplayName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HouseNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Road = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Suburb = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    County = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Postcode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CountryCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OsmId = table.Column<long>(type: "bigint", nullable: true),
                    OsmType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PlaceId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Add AddressId column to users table (nullable initially for data migration)
            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "users",
                type: "int",
                nullable: true);

            // Add AddressId column to offers table (nullable initially for data migration)
            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "offers",
                type: "int",
                nullable: true);

            // Create indexes for performance
            migrationBuilder.CreateIndex(
                name: "IX_users_AddressId",
                table: "users",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_offers_AddressId",
                table: "offers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_Latitude_Longitude",
                table: "addresses",
                columns: new[] { "Latitude", "Longitude" });

            // Create foreign key constraints
            migrationBuilder.AddForeignKey(
                name: "FK_users_addresses_AddressId",
                table: "users",
                column: "AddressId",
                principalTable: "addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_offers_addresses_AddressId",
                table: "offers",
                column: "AddressId",
                principalTable: "addresses",
                principalColumn: "Id");

            // === DATA MIGRATION SCRIPT ===
            // Migrate existing user addresses to new system
            migrationBuilder.Sql(@"
                INSERT INTO addresses (Latitude, Longitude, DisplayName, HouseNumber, Road, City, Postcode, Country, Type, CreatedAt)
                SELECT 
                    0.0 as Latitude, -- Default coordinates, will be updated via frontend
                    0.0 as Longitude,
                    CONCAT(COALESCE(Street, ''), ' ', COALESCE(HouseNumber, ''), ', ', COALESCE(City, ''), ', ', COALESCE(Country, '')) as DisplayName,
                    HouseNumber,
                    Street as Road,
                    City,
                    PostCode as Postcode,
                    Country,
                    0 as Type, -- Residential
                    NOW() as CreatedAt
                FROM users 
                WHERE Street IS NOT NULL AND Street != '' AND City IS NOT NULL AND City != '';

                -- Update users with their new address IDs
                UPDATE users u
                INNER JOIN addresses a ON 
                    a.Road = u.Street AND 
                    a.HouseNumber = u.HouseNumber AND 
                    a.City = u.City AND 
                    a.Country = u.Country
                SET u.AddressId = a.Id
                WHERE u.Street IS NOT NULL AND u.Street != '';
            ");

            // Migrate existing offer locations
            migrationBuilder.Sql(@"
                INSERT INTO addresses (Latitude, Longitude, DisplayName, City, Country, Type, CreatedAt)
                SELECT DISTINCT
                    0.0 as Latitude, -- Default coordinates
                    0.0 as Longitude,
                    COALESCE(Location, 'Unknown Location') as DisplayName,
                    SUBSTRING_INDEX(SUBSTRING_INDEX(Location, ',', -2), ',', 1) as City,
                    SUBSTRING_INDEX(Location, ',', -1) as Country,
                    2 as Type, -- Tourism
                    NOW() as CreatedAt
                FROM offers o
                WHERE o.Location IS NOT NULL AND o.Location != ''
                AND NOT EXISTS (
                    SELECT 1 FROM addresses a2 WHERE a2.DisplayName = o.Location
                );

                -- Update offers with their new address IDs
                UPDATE offers o
                INNER JOIN addresses a ON a.DisplayName = o.Location
                SET o.AddressId = a.Id
                WHERE o.Location IS NOT NULL AND o.Location != '';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove foreign key constraints
            migrationBuilder.DropForeignKey(
                name: "FK_users_addresses_AddressId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_offers_addresses_AddressId",
                table: "offers");

            // Remove indexes
            migrationBuilder.DropIndex(
                name: "IX_users_AddressId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_offers_AddressId",
                table: "offers");

            migrationBuilder.DropIndex(
                name: "IX_addresses_Latitude_Longitude",
                table: "addresses");

            // Remove AddressId columns
            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "offers");

            // Drop addresses table
            migrationBuilder.DropTable(
                name: "addresses");
        }
    }
}
