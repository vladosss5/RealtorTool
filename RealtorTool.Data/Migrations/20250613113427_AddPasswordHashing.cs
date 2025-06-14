using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtorTool.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordHashing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "AuthorizationData",
                newName: "Salt");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Addresses",
                newName: "StreetId");

            migrationBuilder.RenameColumn(
                name: "District",
                table: "Addresses",
                newName: "RegionId");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "AuthorizationData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CityId",
                table: "Addresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictId",
                table: "Addresses",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityId",
                table: "Addresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_DistrictId",
                table: "Addresses",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_RegionId",
                table: "Addresses",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_StreetId",
                table: "Addresses",
                column: "StreetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_DictionaryValues_CityId",
                table: "Addresses",
                column: "CityId",
                principalTable: "DictionaryValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_DictionaryValues_DistrictId",
                table: "Addresses",
                column: "DistrictId",
                principalTable: "DictionaryValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_DictionaryValues_RegionId",
                table: "Addresses",
                column: "RegionId",
                principalTable: "DictionaryValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_DictionaryValues_StreetId",
                table: "Addresses",
                column: "StreetId",
                principalTable: "DictionaryValues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_DictionaryValues_CityId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_DictionaryValues_DistrictId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_DictionaryValues_RegionId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_DictionaryValues_StreetId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CityId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_DistrictId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_RegionId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_StreetId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "AuthorizationData");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Salt",
                table: "AuthorizationData",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "StreetId",
                table: "Addresses",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                table: "Addresses",
                newName: "District");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Addresses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Addresses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
