using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class ClassesInitially : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_users",
            //    table: "users");

            //migrationBuilder.DropColumn(
            //    name: "IsVerified",
            //    table: "users");

            //migrationBuilder.DropColumn(
            //    name: "MembershipActive",
            //    table: "users");

            //migrationBuilder.DropColumn(
            //    name: "MembershipExireDate",
            //    table: "users");

            //migrationBuilder.RenameColumn(
            //    name: "UserName",
            //    table: "users",
            //    newName: "Name");

            //migrationBuilder.RenameColumn(
            //    name: "UserId",
            //    table: "users",
            //    newName: "VerificationState");

            //migrationBuilder.AlterColumn<int>(
            //    name: "VerificationState",
            //    table: "users",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<int>(
            //    name: "User_Id",
            //    table: "users",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0)
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<string>(
            //    name: "Adress",
            //    table: "users",
            //    type: "longtext",
            //    nullable: false)
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddColumn<int>(
            //    name: "CurrentMembershipMembershipID",
            //    table: "users",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "Email_Address",
            //    table: "users",
            //    type: "longtext",
            //    nullable: false)
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_users",
            //    table: "users",
            //    column: "User_Id");

            //migrationBuilder.CreateTable(
            //    name: "Membership",
            //    columns: table => new
            //    {
            //        MembershipID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Expiration = table.Column<DateTime>(type: "datetime(6)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Membership", x => x.MembershipID);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateIndex(
            //    name: "IX_users_CurrentMembershipMembershipID",
            //    table: "users",
            //    column: "CurrentMembershipMembershipID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_users_Membership_CurrentMembershipMembershipID",
            //    table: "users",
            //    column: "CurrentMembershipMembershipID",
            //    principalTable: "Membership",
            //    principalColumn: "MembershipID",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_users_Membership_CurrentMembershipMembershipID",
            //    table: "users");

            //migrationBuilder.DropTable(
            //    name: "Membership");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_users",
            //    table: "users");

            //migrationBuilder.DropIndex(
            //    name: "IX_users_CurrentMembershipMembershipID",
            //    table: "users");

            //migrationBuilder.DropColumn(
            //    name: "User_Id",
            //    table: "users");

            //migrationBuilder.DropColumn(
            //    name: "Adress",
            //    table: "users");

            //migrationBuilder.DropColumn(
            //    name: "CurrentMembershipMembershipID",
            //    table: "users");

            //migrationBuilder.DropColumn(
            //    name: "Email_Address",
            //    table: "users");

            //migrationBuilder.RenameColumn(
            //    name: "VerificationState",
            //    table: "users",
            //    newName: "UserId");

            //migrationBuilder.RenameColumn(
            //    name: "Name",
            //    table: "users",
            //    newName: "UserName");

            //migrationBuilder.AlterColumn<int>(
            //    name: "UserId",
            //    table: "users",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsVerified",
            //    table: "users",
            //    type: "tinyint(1)",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "MembershipActive",
            //    table: "users",
            //    type: "tinyint(1)",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "MembershipExireDate",
            //    table: "users",
            //    type: "datetime(6)",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_users",
            //    table: "users",
            //    column: "UserId");
        }
    }
}
