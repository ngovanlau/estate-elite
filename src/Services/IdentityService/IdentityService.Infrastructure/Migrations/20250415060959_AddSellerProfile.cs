using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    VerifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", maxLength: 50, nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uuid", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SellerProfiles",
                schema: "identity");
        }
    }
}
