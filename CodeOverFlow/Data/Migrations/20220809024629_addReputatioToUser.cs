﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeOverFlow.Data.Migrations
{
    public partial class addReputatioToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Reputation",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reputation",
                table: "AspNetUsers");
        }
    }
}
