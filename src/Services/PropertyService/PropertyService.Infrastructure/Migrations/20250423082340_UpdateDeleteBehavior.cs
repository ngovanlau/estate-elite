using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Addresses_AddressId",
                schema: "property",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyRooms_Rooms_RoomId",
                schema: "property",
                table: "PropertyRooms");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Addresses_AddressId",
                schema: "property",
                table: "Properties",
                column: "AddressId",
                principalSchema: "property",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyRooms_Rooms_RoomId",
                schema: "property",
                table: "PropertyRooms",
                column: "RoomId",
                principalSchema: "property",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Addresses_AddressId",
                schema: "property",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyRooms_Rooms_RoomId",
                schema: "property",
                table: "PropertyRooms");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Addresses_AddressId",
                schema: "property",
                table: "Properties",
                column: "AddressId",
                principalSchema: "property",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyRooms_Rooms_RoomId",
                schema: "property",
                table: "PropertyRooms",
                column: "RoomId",
                principalSchema: "property",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
