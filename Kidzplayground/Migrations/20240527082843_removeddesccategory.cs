﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kidzplayground.Migrations
{
    /// <inheritdoc />
    public partial class removeddesccategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
