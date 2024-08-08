using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class ClassUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_users_Membership_CurrentMembershipMembershipID",
            //    table: "users");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Membership",
            //    table: "Membership");

            //migrationBuilder.RenameTable(
            //    name: "Membership",
            //    newName: "memberships");

            //migrationBuilder.RenameColumn(
            //    name: "Name",
            //    table: "users",
            //    newName: "VisibleName");

            //migrationBuilder.RenameColumn(
            //    name: "Adress",
            //    table: "users",
            //    newName: "Street");

            //migrationBuilder.AddColumn<string>(
            //    name: "City",
            //    table: "users",
            //    type: "longtext",
            //    nullable: false)
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddColumn<string>(
            //    name: "Country",
            //    table: "users",
            //    type: "longtext",
            //    nullable: false)
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddColumn<DateOnly>(
            //    name: "DateOfBirth",
            //    table: "users",
            //    type: "date",
            //    nullable: false,
            //    defaultValue: new DateOnly(1, 1, 1));

            //migrationBuilder.AddColumn<string>(
            //    name: "FirstName",
            //    table: "users",
            //    type: "longtext",
            //    nullable: false)
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddColumn<string>(
            //    name: "Gender",
            //    table: "users",
            //    type: "longtext",
            //    nullable: false)
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddColumn<string>(
            //    name: "HouseNumber",
            //    table: "users",
            //    type: "longtext",
            //    nullable: false)
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsEmailVerified",
            //    table: "users",
            //    type: "tinyint(1)",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "LastName",
            //    table: "users",
            //    type: "longtext",
            //    nullable: false)
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddColumn<string>(
            //    name: "PostCode",
            //    table: "users",
            //    type: "longtext",
            //    nullable: false)
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_memberships",
            //    table: "memberships",
            //    column: "MembershipID");

            //migrationBuilder.CreateTable(
            //    name: "Profiles",
            //    columns: table => new
            //    {
            //        ProfileID = table.Column<int>(name: "Profile_ID", type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        NickName = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Profiles", x => x.ProfileID);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_users_memberships_CurrentMembershipMembershipID",
            //    table: "users",
            //    column: "CurrentMembershipMembershipID",
            //    principalTable: "memberships",
            //    principalColumn: "MembershipID",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_users_memberships_CurrentMembershipMembershipID",
            //    table: "users");

            //migrationBuilder.DropTable(
            //    name: "Profiles");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_memberships",
            //    table: "memberships");

            //migrationBuilder.DropColumn(
            //    name: "City",
            //    table: "users");

            //migrationBuilder.DropColumn(
            //    name: "Country",
            //    table: "users");

            //migrationBuilder.DropColumn(
            //    name: "DateOfBirth",
            //    table: "users");

            //migrationBuilder.DropColumn(
            //    name: "FirstName",
            //    table: "users");

            //migrationBuilder.DropColumn(
            //    name: "Gender",
            //    table: "users");

            //migrationBuilder.DropColumn(
            //    name: "HouseNumber",
            //    table: "users");

            //migrationBuilder.DropColumn(
            //    name: "IsEmailVerified",
            //    table: "users");

            //migrationBuilder.DropColumn(
            //    name: "LastName",
            //    table: "users");

            //migrationBuilder.DropColumn(
            //    name: "PostCode",
            //    table: "users");

            //migrationBuilder.RenameTable(
            //    name: "memberships",
            //    newName: "Membership");

            //migrationBuilder.RenameColumn(
            //    name: "VisibleName",
            //    table: "users",
            //    newName: "Name");

            //migrationBuilder.RenameColumn(
            //    name: "Street",
            //    table: "users",
            //    newName: "Adress");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Membership",
            //    table: "Membership",
            //    column: "MembershipID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_users_Membership_CurrentMembershipMembershipID",
            //    table: "users",
            //    column: "CurrentMembershipMembershipID",
            //    principalTable: "Membership",
            //    principalColumn: "MembershipID",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
