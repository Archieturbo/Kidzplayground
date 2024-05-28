using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kidzplayground.Migrations
{
    /// <inheritdoc />
    public partial class fkmessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SendTo",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SendFrom",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SendFrom",
                table: "Messages",
                column: "SendFrom");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SendTo",
                table: "Messages",
                column: "SendTo");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SendFrom",
                table: "Messages",
                column: "SendFrom",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SendTo",
                table: "Messages",
                column: "SendTo",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SendFrom",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SendTo",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SendFrom",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SendTo",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "SendTo",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SendFrom",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
