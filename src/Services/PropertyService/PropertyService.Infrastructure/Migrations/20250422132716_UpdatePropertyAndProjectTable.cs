using System;
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
            migrationBuilder.AddColumn<Guid>(
                name: "PropertyId1",
                schema: "property",
                table: "PropertyRooms",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId1",
                schema: "property",
                table: "PropertyRooms",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyRooms_PropertyId1",
                schema: "property",
                table: "PropertyRooms",
                column: "PropertyId1");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyRooms_RoomId1",
                schema: "property",
                table: "PropertyRooms",
                column: "RoomId1");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyRooms_Properties_PropertyId1",
                schema: "property",
                table: "PropertyRooms",
                column: "PropertyId1",
                principalSchema: "property",
                principalTable: "Properties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyRooms_Rooms_RoomId1",
                schema: "property",
                table: "PropertyRooms",
                column: "RoomId1",
                principalSchema: "property",
                principalTable: "Rooms",
                principalColumn: "Id");
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

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyRooms_Properties_PropertyId1",
                schema: "property",
                table: "PropertyRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyRooms_Rooms_RoomId1",
                schema: "property",
                table: "PropertyRooms");

            migrationBuilder.DropIndex(
                name: "IX_PropertyRooms_PropertyId1",
                schema: "property",
                table: "PropertyRooms");

            migrationBuilder.DropIndex(
                name: "IX_PropertyRooms_RoomId1",
                schema: "property",
                table: "PropertyRooms");

            migrationBuilder.DropColumn(
                name: "PropertyId1",
                schema: "property",
                table: "PropertyRooms");

            migrationBuilder.DropColumn(
                name: "RoomId1",
                schema: "property",
                table: "PropertyRooms");
        }
    }
}
