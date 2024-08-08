using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class EmailVerificator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_users_memberships_CurrentMembershipMembershipID",
            //    table: "users");

            //migrationBuilder.AlterColumn<int>(
            //    name: "CurrentMembershipMembershipID",
            //    table: "users",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.CreateTable(
            //    name: "emailverificators",
            //    columns: table => new
            //    {
            //        verivicationID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        userId = table.Column<int>(name: "user_Id", type: "int", nullable: false),
            //        verificationToken = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
            //        requestDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_emailverificators", x => x.verivicationID);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_users_memberships_CurrentMembershipMembershipID",
            //    table: "users",
            //    column: "CurrentMembershipMembershipID",
            //    principalTable: "memberships",
            //    principalColumn: "MembershipID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_users_memberships_CurrentMembershipMembershipID",
            //    table: "users");

            //migrationBuilder.DropTable(
            //    name: "emailverificators");

            //migrationBuilder.AlterColumn<int>(
            //    name: "CurrentMembershipMembershipID",
            //    table: "users",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_users_memberships_CurrentMembershipMembershipID",
            //    table: "users",
            //    column: "CurrentMembershipMembershipID",
            //    principalTable: "memberships",
            //    principalColumn: "MembershipID",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
