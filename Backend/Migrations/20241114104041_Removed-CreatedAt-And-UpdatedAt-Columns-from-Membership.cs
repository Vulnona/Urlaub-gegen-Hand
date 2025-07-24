using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UGHApi.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCreatedAtAndUpdatedAtColumnsfromMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_memberships_MembershipId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "memberships");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "memberships");

            migrationBuilder.RenameColumn(
                name: "DurationMonths",
                table: "memberships",
                newName: "DurationDays");

            migrationBuilder.AlterColumn<int>(
                name: "MembershipId",
                table: "users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_users_memberships_MembershipId",
                table: "users",
                column: "MembershipId",
                principalTable: "memberships",
                principalColumn: "MembershipID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_memberships_MembershipId",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "DurationDays",
                table: "memberships",
                newName: "DurationMonths");

            migrationBuilder.AlterColumn<int>(
                name: "MembershipId",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "memberships",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "memberships",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_users_memberships_MembershipId",
                table: "users",
                column: "MembershipId",
                principalTable: "memberships",
                principalColumn: "MembershipID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
