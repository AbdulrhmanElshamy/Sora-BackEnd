using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sofra.Api.Migrations
{
    /// <inheritdoc />
    public partial class MealPhotoCascaiding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPhotos_Meals_MealId",
                table: "MealPhotos");

            migrationBuilder.AddForeignKey(
                name: "FK_MealPhotos_Meals_MealId",
                table: "MealPhotos",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPhotos_Meals_MealId",
                table: "MealPhotos");

            migrationBuilder.AddForeignKey(
                name: "FK_MealPhotos_Meals_MealId",
                table: "MealPhotos",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
