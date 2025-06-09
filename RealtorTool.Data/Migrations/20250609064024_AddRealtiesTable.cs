using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtorTool.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRealtiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientTags_DictionaryValues_clientTag_tag",
                table: "ClientTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientTags_Persons_clientTag_client",
                table: "ClientTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_DictionaryValues_RoleId",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_ClientTags_clientTag_client",
                table: "ClientTags");

            migrationBuilder.DropIndex(
                name: "IX_ClientTags_clientTag_tag",
                table: "ClientTags");

            migrationBuilder.DropColumn(
                name: "clientTag_client",
                table: "ClientTags");

            migrationBuilder.DropColumn(
                name: "clientTag_tag",
                table: "ClientTags");

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    District = table.Column<string>(type: "text", nullable: true),
                    Street = table.Column<string>(type: "text", nullable: true),
                    HouseNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Application",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Desctiption = table.Column<string>(type: "text", nullable: false),
                    DesiredCost = table.Column<decimal>(type: "numeric", nullable: false),
                    TypeDealId = table.Column<string>(type: "text", nullable: false),
                    StatusId = table.Column<string>(type: "text", nullable: false),
                    ClientId = table.Column<string>(type: "text", nullable: false),
                    RealtorId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Application", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Application_DictionaryValues_StatusId",
                        column: x => x.StatusId,
                        principalTable: "DictionaryValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Application_DictionaryValues_TypeDealId",
                        column: x => x.TypeDealId,
                        principalTable: "DictionaryValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Application_Persons_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Application_Persons_RealtorId",
                        column: x => x.RealtorId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Houses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TotalNumberFloors = table.Column<int>(type: "integer", nullable: false),
                    YearConstruction = table.Column<int>(type: "integer", nullable: false),
                    СonstructionTypeId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Houses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Houses_DictionaryValues_СonstructionTypeId",
                        column: x => x.СonstructionTypeId,
                        principalTable: "DictionaryValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TotalSqare = table.Column<decimal>(type: "numeric", nullable: false),
                    HasHouse = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Areas_Addresses_Id",
                        column: x => x.Id,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Realties",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TypeId = table.Column<string>(type: "text", nullable: false),
                    ApplicationId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Realties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Realties_Application_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Application",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Realties_DictionaryValues_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DictionaryValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flats",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Floor = table.Column<int>(type: "integer", nullable: false),
                    CeilingHeight = table.Column<decimal>(type: "numeric", nullable: false),
                    BathroomTypeId = table.Column<string>(type: "text", nullable: false),
                    RepairId = table.Column<string>(type: "text", nullable: false),
                    UsedTypeId = table.Column<string>(type: "text", nullable: false),
                    HouseId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flats_DictionaryValues_BathroomTypeId",
                        column: x => x.BathroomTypeId,
                        principalTable: "DictionaryValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flats_DictionaryValues_RepairId",
                        column: x => x.RepairId,
                        principalTable: "DictionaryValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flats_DictionaryValues_UsedTypeId",
                        column: x => x.UsedTypeId,
                        principalTable: "DictionaryValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flats_Houses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrivateHomes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CeilingHeight = table.Column<decimal>(type: "numeric", nullable: false),
                    Sqare = table.Column<decimal>(type: "numeric", nullable: false),
                    RoomCount = table.Column<int>(type: "integer", nullable: false),
                    FloorsCount = table.Column<int>(type: "integer", nullable: false),
                    AddressNumber = table.Column<string>(type: "text", nullable: true),
                    AreaId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateHomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateHomes_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ImageData = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Realties_Id",
                        column: x => x.Id,
                        principalTable: "Realties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientTags_ClientId",
                table: "ClientTags",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTags_TagId",
                table: "ClientTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Application_ClientId",
                table: "Application",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Application_RealtorId",
                table: "Application",
                column: "RealtorId");

            migrationBuilder.CreateIndex(
                name: "IX_Application_StatusId",
                table: "Application",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Application_TypeDealId",
                table: "Application",
                column: "TypeDealId");

            migrationBuilder.CreateIndex(
                name: "IX_Flats_BathroomTypeId",
                table: "Flats",
                column: "BathroomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Flats_HouseId",
                table: "Flats",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Flats_RepairId",
                table: "Flats",
                column: "RepairId");

            migrationBuilder.CreateIndex(
                name: "IX_Flats_UsedTypeId",
                table: "Flats",
                column: "UsedTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Houses_СonstructionTypeId",
                table: "Houses",
                column: "СonstructionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateHomes_AreaId",
                table: "PrivateHomes",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Realties_ApplicationId",
                table: "Realties",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Realties_TypeId",
                table: "Realties",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientTags_DictionaryValues_TagId",
                table: "ClientTags",
                column: "TagId",
                principalTable: "DictionaryValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientTags_Persons_ClientId",
                table: "ClientTags",
                column: "ClientId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_DictionaryValues_RoleId",
                table: "Persons",
                column: "RoleId",
                principalTable: "DictionaryValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientTags_DictionaryValues_TagId",
                table: "ClientTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientTags_Persons_ClientId",
                table: "ClientTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_DictionaryValues_RoleId",
                table: "Persons");

            migrationBuilder.DropTable(
                name: "Flats");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "PrivateHomes");

            migrationBuilder.DropTable(
                name: "Houses");

            migrationBuilder.DropTable(
                name: "Realties");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Application");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_ClientTags_ClientId",
                table: "ClientTags");

            migrationBuilder.DropIndex(
                name: "IX_ClientTags_TagId",
                table: "ClientTags");

            migrationBuilder.AddColumn<string>(
                name: "clientTag_client",
                table: "ClientTags",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "clientTag_tag",
                table: "ClientTags",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientTags_clientTag_client",
                table: "ClientTags",
                column: "clientTag_client");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTags_clientTag_tag",
                table: "ClientTags",
                column: "clientTag_tag");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientTags_DictionaryValues_clientTag_tag",
                table: "ClientTags",
                column: "clientTag_tag",
                principalTable: "DictionaryValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientTags_Persons_clientTag_client",
                table: "ClientTags",
                column: "clientTag_client",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_DictionaryValues_RoleId",
                table: "Persons",
                column: "RoleId",
                principalTable: "DictionaryValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
