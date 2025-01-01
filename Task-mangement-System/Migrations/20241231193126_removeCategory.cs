using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_mangement_System.Migrations
{
    /// <inheritdoc />
    public partial class removeCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tasks_categories_CategoryId",
                table: "tasks");

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_categories_CategoryId",
                table: "tasks",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tasks_categories_CategoryId",
                table: "tasks");

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_categories_CategoryId",
                table: "tasks",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id");
        }
    }
}
