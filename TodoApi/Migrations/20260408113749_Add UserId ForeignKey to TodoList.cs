using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdForeignKeytoTodoList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TodoLists",
                newName: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_TodoLists_user_id",
                table: "TodoLists",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLists_AspNetUsers_user_id",
                table: "TodoLists",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoLists_AspNetUsers_user_id",
                table: "TodoLists");

            migrationBuilder.DropIndex(
                name: "IX_TodoLists_user_id",
                table: "TodoLists");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "TodoLists",
                newName: "UserId");
        }
    }
}
