using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtorTool.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class RestructBdForProceses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Photos_PhotoId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Photos_PhotoId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Realties_RealtyId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_EntityType",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_RealtyId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PhotoId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Clients_PhotoId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "TotalArea",
                table: "PrivateHouses");

            migrationBuilder.DropColumn(
                name: "RealtyId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "TotalArea",
                table: "Apartments");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Realties",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RealtyId",
                table: "Realties",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RealtyType",
                table: "Realties",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalArea",
                table: "Realties",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Listings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ClientRequestId",
                table: "Deals",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasUtilities",
                table: "Areas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LandCategoryId",
                table: "Areas",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClientRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClientId = table.Column<string>(type: "text", nullable: false),
                    EmployeeId = table.Column<string>(type: "text", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    MinRooms = table.Column<int>(type: "integer", nullable: true),
                    MinArea = table.Column<decimal>(type: "numeric", nullable: true),
                    MaxArea = table.Column<decimal>(type: "numeric", nullable: true),
                    DesiredLocation = table.Column<string>(type: "text", nullable: true),
                    AdditionalRequirements = table.Column<string>(type: "text", nullable: true),
                    ListingId = table.Column<string>(type: "text", nullable: true),
                    MatchedRequestId = table.Column<string>(type: "text", nullable: true),
                    DealId = table.Column<string>(type: "text", nullable: true),
                    RealtyId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientRequests_ClientRequests_MatchedRequestId",
                        column: x => x.MatchedRequestId,
                        principalTable: "ClientRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientRequests_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientRequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientRequests_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientRequests_Realties_RealtyId",
                        column: x => x.RealtyId,
                        principalTable: "Realties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Realties_IsActive",
                table: "Realties",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateHouses_FloorsCount",
                table: "PrivateHouses",
                column: "FloorsCount");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateHouses_YearBuilt",
                table: "PrivateHouses",
                column: "YearBuilt");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EntityType_EntityId",
                table: "Photos",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CreatedDate",
                table: "Listings",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_Price",
                table: "Listings",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PhotoId",
                table: "Employees",
                column: "PhotoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryValues_Value",
                table: "DictionaryValues",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_ClientRequestId",
                table: "Deals",
                column: "ClientRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deals_DealDate",
                table: "Deals",
                column: "DealDate");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_FinalPrice",
                table: "Deals",
                column: "FinalPrice");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_PhotoId",
                table: "Clients",
                column: "PhotoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_LandCategoryId",
                table: "Areas",
                column: "LandCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_Square",
                table: "Areas",
                column: "Square");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_Floor",
                table: "Apartments",
                column: "Floor");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_RoomsCount",
                table: "Apartments",
                column: "RoomsCount");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_City",
                table: "Addresses",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_District",
                table: "Addresses",
                column: "District");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PostalCode",
                table: "Addresses",
                column: "PostalCode");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_ClientId",
                table: "ClientRequests",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_CreatedDate",
                table: "ClientRequests",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_EmployeeId",
                table: "ClientRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_ListingId",
                table: "ClientRequests",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_MatchedRequestId",
                table: "ClientRequests",
                column: "MatchedRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_RealtyId",
                table: "ClientRequests",
                column: "RealtyId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_Status",
                table: "ClientRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_Type",
                table: "ClientRequests",
                column: "Type");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_DictionaryValues_LandCategoryId",
                table: "Areas",
                column: "LandCategoryId",
                principalTable: "DictionaryValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Photos_PhotoId",
                table: "Clients",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_ClientRequests_ClientRequestId",
                table: "Deals",
                column: "ClientRequestId",
                principalTable: "ClientRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Photos_PhotoId",
                table: "Employees",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Realties_EntityId",
                table: "Photos",
                column: "EntityId",
                principalTable: "Realties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_DictionaryValues_LandCategoryId",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Photos_PhotoId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_ClientRequests_ClientRequestId",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Photos_PhotoId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Realties_EntityId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "ClientRequests");

            migrationBuilder.DropIndex(
                name: "IX_Realties_IsActive",
                table: "Realties");

            migrationBuilder.DropIndex(
                name: "IX_PrivateHouses_FloorsCount",
                table: "PrivateHouses");

            migrationBuilder.DropIndex(
                name: "IX_PrivateHouses_YearBuilt",
                table: "PrivateHouses");

            migrationBuilder.DropIndex(
                name: "IX_Photos_EntityType_EntityId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Listings_CreatedDate",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_Price",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PhotoId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_DictionaryValues_Value",
                table: "DictionaryValues");

            migrationBuilder.DropIndex(
                name: "IX_Deals_ClientRequestId",
                table: "Deals");

            migrationBuilder.DropIndex(
                name: "IX_Deals_DealDate",
                table: "Deals");

            migrationBuilder.DropIndex(
                name: "IX_Deals_FinalPrice",
                table: "Deals");

            migrationBuilder.DropIndex(
                name: "IX_Clients_PhotoId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Areas_LandCategoryId",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Areas_Square",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_Floor",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_RoomsCount",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_City",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_District",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_PostalCode",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Realties");

            migrationBuilder.DropColumn(
                name: "RealtyId",
                table: "Realties");

            migrationBuilder.DropColumn(
                name: "RealtyType",
                table: "Realties");

            migrationBuilder.DropColumn(
                name: "TotalArea",
                table: "Realties");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "ClientRequestId",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "HasUtilities",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "LandCategoryId",
                table: "Areas");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalArea",
                table: "PrivateHouses",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RealtyId",
                table: "Photos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalArea",
                table: "Apartments",
                type: "numeric",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EntityType",
                table: "Photos",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_RealtyId",
                table: "Photos",
                column: "RealtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PhotoId",
                table: "Employees",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_PhotoId",
                table: "Clients",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Photos_PhotoId",
                table: "Clients",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Photos_PhotoId",
                table: "Employees",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Realties_RealtyId",
                table: "Photos",
                column: "RealtyId",
                principalTable: "Realties",
                principalColumn: "Id");
        }
    }
}
