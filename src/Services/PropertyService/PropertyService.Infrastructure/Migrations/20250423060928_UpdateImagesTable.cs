using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Projects_EntityId",
                schema: "property",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Properties_EntityId",
                schema: "property",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_EntityId",
                schema: "property",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_EntityId_IsMain",
                schema: "property",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "EntityId",
                schema: "property",
                table: "Images");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                schema: "property",
                table: "Images",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PropertyId",
                schema: "property",
                table: "Images",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProjectId",
                schema: "property",
                table: "Images",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProjectId_IsMain",
                schema: "property",
                table: "Images",
                columns: new[] { "ProjectId", "IsMain" });

            migrationBuilder.CreateIndex(
                name: "IX_Images_PropertyId",
                schema: "property",
                table: "Images",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Projects_ProjectId",
                schema: "property",
                table: "Images",
                column: "ProjectId",
                principalSchema: "property",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Properties_PropertyId",
                schema: "property",
                table: "Images",
                column: "PropertyId",
                principalSchema: "property",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Projects_ProjectId",
                schema: "property",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Properties_PropertyId",
                schema: "property",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ProjectId",
                schema: "property",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ProjectId_IsMain",
                schema: "property",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_PropertyId",
                schema: "property",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                schema: "property",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                schema: "property",
                table: "Images");

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                schema: "property",
                table: "Images",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Images_EntityId",
                schema: "property",
                table: "Images",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_EntityId_IsMain",
                schema: "property",
                table: "Images",
                columns: new[] { "EntityId", "IsMain" });

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
    }
}
