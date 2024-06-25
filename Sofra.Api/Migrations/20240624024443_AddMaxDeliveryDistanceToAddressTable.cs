using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sofra.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMaxDeliveryDistanceToAddressTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MaxDeliveryDistance",
                table: "Kitchens",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddCheckConstraint(
                name: "CK_MaxDeliveryDistance",
                table: "Kitchens",
                sql: "MaxDeliveryDistance >= 1.0 AND MaxDeliveryDistance <= 10");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_MaxDeliveryDistance",
                table: "Kitchens");

            migrationBuilder.DropColumn(
                name: "MaxDeliveryDistance",
                table: "Kitchens");
        }
    }
}
