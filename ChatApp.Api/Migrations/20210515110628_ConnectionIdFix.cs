using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApp.Api.Migrations
{
    public partial class ConnectionIdFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Connections",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "Connections");

            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "Connections",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Connections",
                table: "Connections",
                column: "ConnectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Connections",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Connections");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Connections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "Connections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Connections",
                table: "Connections",
                column: "Id");
        }
    }
}
