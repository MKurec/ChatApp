using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApp.Api.Migrations
{
    public partial class ListOfFriends : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserUser",
                columns: table => new
                {
                    UnconfirmedFriendsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserFriendsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUser", x => new { x.UnconfirmedFriendsId, x.UserFriendsId });
                    table.ForeignKey(
                        name: "FK_UserUser_Users_UnconfirmedFriendsId",
                        column: x => x.UnconfirmedFriendsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserUser_Users_UserFriendsId",
                        column: x => x.UserFriendsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserUser_UserFriendsId",
                table: "UserUser",
                column: "UserFriendsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserUser");
        }
    }
}
