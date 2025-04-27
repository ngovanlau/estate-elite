using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSellerProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayPalMerchantId",
                schema: "identity",
                table: "SellerProfiles",
                newName: "PaypalMerchantId");

            migrationBuilder.RenameColumn(
                name: "PayPalEmail",
                schema: "identity",
                table: "SellerProfiles",
                newName: "PaypalEmail");

            migrationBuilder.RenameColumn(
                name: "AcceptsPayPal",
                schema: "identity",
                table: "SellerProfiles",
                newName: "AcceptsPaypal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaypalMerchantId",
                schema: "identity",
                table: "SellerProfiles",
                newName: "PayPalMerchantId");

            migrationBuilder.RenameColumn(
                name: "PaypalEmail",
                schema: "identity",
                table: "SellerProfiles",
                newName: "PayPalEmail");

            migrationBuilder.RenameColumn(
                name: "AcceptsPaypal",
                schema: "identity",
                table: "SellerProfiles",
                newName: "AcceptsPayPal");
        }
    }
}
