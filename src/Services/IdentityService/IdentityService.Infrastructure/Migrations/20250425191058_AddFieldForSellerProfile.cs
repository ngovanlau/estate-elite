using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldForSellerProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AcceptsPayPal",
                schema: "identity",
                table: "SellerProfiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PayPalEmail",
                schema: "identity",
                table: "SellerProfiles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayPalMerchantId",
                schema: "identity",
                table: "SellerProfiles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptsPayPal",
                schema: "identity",
                table: "SellerProfiles");

            migrationBuilder.DropColumn(
                name: "PayPalEmail",
                schema: "identity",
                table: "SellerProfiles");

            migrationBuilder.DropColumn(
                name: "PayPalMerchantId",
                schema: "identity",
                table: "SellerProfiles");
        }
    }
}
