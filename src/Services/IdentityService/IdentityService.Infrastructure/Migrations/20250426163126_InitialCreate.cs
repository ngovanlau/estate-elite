using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityService.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "identity");

        migrationBuilder.CreateTable(
            name: "Users",
            schema: "identity",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Username = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                FullName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                PasswordHash = table.Column<string>(type: "text", nullable: false),
                Role = table.Column<string>(type: "text", nullable: false),
                Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                Address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                Avatar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                Background = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                IsActive = table.Column<bool>(type: "boolean", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                CreatedBy = table.Column<Guid>(type: "uuid", maxLength: 50, nullable: false),
                ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                ModifiedBy = table.Column<Guid>(type: "uuid", maxLength: 50, nullable: true),
                IsDelete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SellerProfiles",
            schema: "identity",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                CompanyName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                LicenseNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                TaxIdentificationNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                ProfessionalLicense = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                Biography = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                EstablishedYear = table.Column<int>(type: "integer", nullable: false),
                IsVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                VerifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                CreatedBy = table.Column<Guid>(type: "uuid", maxLength: 50, nullable: false),
                ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                ModifiedBy = table.Column<Guid>(type: "uuid", maxLength: 50, nullable: true),
                IsDelete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                PayPalEmail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                PayPalMerchantId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                AcceptsPayPal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SellerProfiles", x => x.UserId);
                table.ForeignKey(
                    name: "FK_SellerProfiles_Users_UserId",
                    column: x => x.UserId,
                    principalSchema: "identity",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_SellerProfiles_CompanyName",
            schema: "identity",
            table: "SellerProfiles",
            column: "CompanyName");

        migrationBuilder.CreateIndex(
            name: "IX_SellerProfiles_TaxIdentificationNumber",
            schema: "identity",
            table: "SellerProfiles",
            column: "TaxIdentificationNumber",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            schema: "identity",
            table: "Users",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Users_Role",
            schema: "identity",
            table: "Users",
            column: "Role");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Username",
            schema: "identity",
            table: "Users",
            column: "Username",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SellerProfiles",
            schema: "identity");

        migrationBuilder.DropTable(
            name: "Users",
            schema: "identity");
    }
}
