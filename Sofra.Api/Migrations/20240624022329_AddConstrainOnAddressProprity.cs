using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sofra.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddConstrainOnAddressProprity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Addresses",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: false,
                defaultValue: 0m,
                comment: "Longitude must be between 25.0 and 35.0 degrees.",
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Addresses",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: false,
                defaultValue: 0m,
                comment: "Latitude must be between 22.0 and 31.5 degrees.",
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Location_Latitude",
                table: "Addresses",
                sql: "Latitude >= 22.0 AND Latitude <= 31.5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Location_Longitude",
                table: "Addresses",
                sql: "Longitude >= 25.0 AND Longitude <= 35.0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Location_Latitude",
                table: "Addresses");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Location_Longitude",
                table: "Addresses");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Addresses",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6,
                oldDefaultValue: 0m,
                oldComment: "Longitude must be between 25.0 and 35.0 degrees.");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Addresses",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6,
                oldDefaultValue: 0m,
                oldComment: "Latitude must be between 22.0 and 31.5 degrees.");
        }
    }
}
