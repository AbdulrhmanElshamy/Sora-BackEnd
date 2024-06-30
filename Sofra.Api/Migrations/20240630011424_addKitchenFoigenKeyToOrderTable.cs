using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sofra.Api.Migrations
{
    /// <inheritdoc />
    public partial class addKitchenFoigenKeyToOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KitchenId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_KitchenId",
                table: "Orders",
                column: "KitchenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Kitchens_KitchenId",
                table: "Orders",
                column: "KitchenId",
                principalTable: "Kitchens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Kitchens_KitchenId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_KitchenId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "KitchenId",
                table: "Orders");
        }
    }
}
