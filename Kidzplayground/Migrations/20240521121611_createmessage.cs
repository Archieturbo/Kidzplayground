using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kidzplayground.Migrations
{
    /// <inheritdoc />
    public partial class createmessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
