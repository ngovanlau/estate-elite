using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePropertyAndProjectTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Images_Projects_EntityId",
                schema: "property",
                table: "Images",
                column: "EntityId",
                principalSchema: "property",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Properties_EntityId",
                schema: "property",
                table: "Images",
                column: "EntityId",
                principalSchema: "property",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Projects_EntityId",
                schema: "property",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Properties_EntityId",
                schema: "property",
                table: "Images");
        }
    }
}
