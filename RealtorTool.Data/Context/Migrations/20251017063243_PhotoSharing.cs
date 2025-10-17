using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtorTool.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class PhotoSharing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientRequests_Realties_RealtyId",
                table: "ClientRequests");

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

            migrationBuilder.DropIndex(
                name: "IX_Photos_EntityId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_EntityType_EntityId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PhotoId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Deals_ClientRequestId",
                table: "Deals");

            migrationBuilder.DropIndex(
                name: "IX_Clients_PhotoId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ClientRequestId",
                table: "Deals");

            migrationBuilder.CreateTable(
                name: "ClientPhotos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ClientId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientPhotos_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientPhotos_Photos_Id",
                        column: x => x.Id,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealParticipant",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    DealId = table.Column<string>(type: "text", nullable: false),
                    ClientRequestId = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealParticipant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealParticipant_ClientRequests_ClientRequestId",
                        column: x => x.ClientRequestId,
                        principalTable: "ClientRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealParticipant_Deals_DealId",
                        column: x => x.DealId,
                        principalTable: "Deals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePhotos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    EmployeeId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeePhotos_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeePhotos_Photos_Id",
                        column: x => x.Id,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RealtyPhotos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    RealtyId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealtyPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RealtyPhotos_Photos_Id",
                        column: x => x.Id,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RealtyPhotos_Realties_RealtyId",
                        column: x => x.RealtyId,
                        principalTable: "Realties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Realties_RealtyType",
                table: "Realties",
                column: "RealtyType");

            migrationBuilder.CreateIndex(
                name: "IX_Realties_TotalArea",
                table: "Realties",
                column: "TotalArea");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateHouses_RoomsCount",
                table: "PrivateHouses",
                column: "RoomsCount");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_StatusId_CreatedDate",
                table: "Listings",
                columns: new[] { "StatusId", "CreatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryValues_DictionaryId_Value",
                table: "DictionaryValues",
                columns: new[] { "DictionaryId", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deals_StatusId_DealDate",
                table: "Deals",
                columns: new[] { "StatusId", "DealDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_DealId",
                table: "ClientRequests",
                column: "DealId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_Status_Type",
                table: "ClientRequests",
                columns: new[] { "Status", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_City_District_Street_HouseNumber_BuildingNumber",
                table: "Addresses",
                columns: new[] { "City", "District", "Street", "HouseNumber", "BuildingNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientPhotos_ClientId",
                table: "ClientPhotos",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealParticipant_ClientRequestId",
                table: "DealParticipant",
                column: "ClientRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_DealParticipant_DealId_ClientRequestId",
                table: "DealParticipant",
                columns: new[] { "DealId", "ClientRequestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealParticipant_Role",
                table: "DealParticipant",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePhotos_EmployeeId",
                table: "EmployeePhotos",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealtyPhotos_RealtyId",
                table: "RealtyPhotos",
                column: "RealtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientRequests_Deals_DealId",
                table: "ClientRequests",
                column: "DealId",
                principalTable: "Deals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientRequests_Realties_RealtyId",
                table: "ClientRequests",
                column: "RealtyId",
                principalTable: "Realties",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientRequests_Deals_DealId",
                table: "ClientRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientRequests_Realties_RealtyId",
                table: "ClientRequests");

            migrationBuilder.DropTable(
                name: "ClientPhotos");

            migrationBuilder.DropTable(
                name: "DealParticipant");

            migrationBuilder.DropTable(
                name: "EmployeePhotos");

            migrationBuilder.DropTable(
                name: "RealtyPhotos");

            migrationBuilder.DropIndex(
                name: "IX_Realties_RealtyType",
                table: "Realties");

            migrationBuilder.DropIndex(
                name: "IX_Realties_TotalArea",
                table: "Realties");

            migrationBuilder.DropIndex(
                name: "IX_PrivateHouses_RoomsCount",
                table: "PrivateHouses");

            migrationBuilder.DropIndex(
                name: "IX_Listings_StatusId_CreatedDate",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_DictionaryValues_DictionaryId_Value",
                table: "DictionaryValues");

            migrationBuilder.DropIndex(
                name: "IX_Deals_StatusId_DealDate",
                table: "Deals");

            migrationBuilder.DropIndex(
                name: "IX_ClientRequests_DealId",
                table: "ClientRequests");

            migrationBuilder.DropIndex(
                name: "IX_ClientRequests_Status_Type",
                table: "ClientRequests");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_City_District_Street_HouseNumber_BuildingNumber",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "ClientRequestId",
                table: "Deals",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EntityId",
                table: "Photos",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EntityType_EntityId",
                table: "Photos",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PhotoId",
                table: "Employees",
                column: "PhotoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deals_ClientRequestId",
                table: "Deals",
                column: "ClientRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_PhotoId",
                table: "Clients",
                column: "PhotoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientRequests_Realties_RealtyId",
                table: "ClientRequests",
                column: "RealtyId",
                principalTable: "Realties",
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
    }
}
