using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RealtorTool.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "'Россия'::character varying"),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    District = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Street = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    HouseNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BuildingNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ApartmentNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric(10,8)", precision: 10, scale: 8, nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric(11,8)", precision: 11, scale: 8, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Addresses_pkey", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Position = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Department = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Salary = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    CommissionRate = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Login = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying", nullable: false),
                    Salt = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Employees_pkey", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    BuildingId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AddressId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Floors = table.Column<int>(type: "integer", nullable: true),
                    YearBuilt = table.Column<int>(type: "integer", nullable: true),
                    ConstructionMaterial = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HasElevator = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    HasParking = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Buildings_pkey", x => x.BuildingId);
                    table.ForeignKey(
                        name: "Buildings_AddressId_fkey",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PassportSeries = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    PassportNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    RegistrationAddressId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Clients_pkey", x => x.ClientId);
                    table.ForeignKey(
                        name: "Clients_RegistrationAddressId_fkey",
                        column: x => x.RegistrationAddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                });

            migrationBuilder.CreateTable(
                name: "LandPlots",
                columns: table => new
                {
                    LandPlotId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AddressId = table.Column<int>(type: "integer", nullable: false),
                    CadastralNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Area = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    ZoningType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HasUtilities = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    UtilitiesDescription = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LandPlots_pkey", x => x.LandPlotId);
                    table.ForeignKey(
                        name: "LandPlots_AddressId_fkey",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                });

            migrationBuilder.CreateTable(
                name: "Apartments",
                columns: table => new
                {
                    ApartmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BuildingId = table.Column<int>(type: "integer", nullable: false),
                    ApartmentNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Floor = table.Column<int>(type: "integer", nullable: true),
                    RoomsCount = table.Column<int>(type: "integer", nullable: true),
                    TotalArea = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    LivingArea = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    KitchenArea = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    HasBalcony = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    HasLoggia = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    RenovationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Apartments_pkey", x => x.ApartmentId);
                    table.ForeignKey(
                        name: "Apartments_BuildingId_fkey",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    RequestType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PropertyType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    MinArea = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    MaxArea = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    MinRooms = table.Column<int>(type: "integer", nullable: true),
                    MaxRooms = table.Column<int>(type: "integer", nullable: true),
                    MinPrice = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: true),
                    MaxPrice = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: true),
                    PreferredDistricts = table.Column<List<string>>(type: "text[]", nullable: true),
                    MinFloor = table.Column<int>(type: "integer", nullable: true),
                    MaxFloor = table.Column<int>(type: "integer", nullable: true),
                    HasBalcony = table.Column<bool>(type: "boolean", nullable: true),
                    HasParking = table.Column<bool>(type: "boolean", nullable: true),
                    OtherPreferences = table.Column<string>(type: "text", nullable: true),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_DATE"),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Active'::character varying"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EmployeeId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ClientRequests_pkey", x => x.RequestId);
                    table.ForeignKey(
                        name: "ClientRequests_ClientId_fkey",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "ClientRequests_EmployeeId_fkey",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateTable(
                name: "PrivateHouses",
                columns: table => new
                {
                    PrivateHouseId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LandPlotId = table.Column<int>(type: "integer", nullable: false),
                    RoomsCount = table.Column<int>(type: "integer", nullable: true),
                    TotalArea = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    FloorsCount = table.Column<int>(type: "integer", nullable: true),
                    HasGarage = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    HasBasement = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    HeatingType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ConstructionMaterial = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    YearBuilt = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrivateHouses_pkey", x => x.PrivateHouseId);
                    table.ForeignKey(
                        name: "PrivateHouses_LandPlotId_fkey",
                        column: x => x.LandPlotId,
                        principalTable: "LandPlots",
                        principalColumn: "LandPlotId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyListings",
                columns: table => new
                {
                    ListingId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    TransactionType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true, defaultValueSql: "'RUB'::character varying"),
                    Commission = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    IsExclusive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    ListingDate = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_DATE"),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Active'::character varying"),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ApartmentId = table.Column<int>(type: "integer", nullable: true),
                    PrivateHouseId = table.Column<int>(type: "integer", nullable: true),
                    LandPlotId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EmployeeId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PropertyListings_pkey", x => x.ListingId);
                    table.ForeignKey(
                        name: "PropertyListings_ApartmentId_fkey",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "ApartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PropertyListings_ClientId_fkey",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "PropertyListings_EmployeeId_fkey",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "PropertyListings_LandPlotId_fkey",
                        column: x => x.LandPlotId,
                        principalTable: "LandPlots",
                        principalColumn: "LandPlotId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PropertyListings_PrivateHouseId_fkey",
                        column: x => x.PrivateHouseId,
                        principalTable: "PrivateHouses",
                        principalColumn: "PrivateHouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deals",
                columns: table => new
                {
                    DealId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ListingId = table.Column<int>(type: "integer", nullable: false),
                    RequestId = table.Column<int>(type: "integer", nullable: true),
                    BuyerId = table.Column<int>(type: "integer", nullable: false),
                    SellerId = table.Column<int>(type: "integer", nullable: false),
                    DealDate = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_DATE"),
                    FinalPrice = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: false),
                    CommissionAmount = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: true),
                    DealStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'InProgress'::character varying"),
                    ContractNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EmployeeId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Deals_pkey", x => x.DealId);
                    table.ForeignKey(
                        name: "Deals_BuyerId_fkey",
                        column: x => x.BuyerId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "Deals_EmployeeId_fkey",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "Deals_ListingId_fkey",
                        column: x => x.ListingId,
                        principalTable: "PropertyListings",
                        principalColumn: "ListingId");
                    table.ForeignKey(
                        name: "Deals_RequestId_fkey",
                        column: x => x.RequestId,
                        principalTable: "ClientRequests",
                        principalColumn: "RequestId");
                    table.ForeignKey(
                        name: "Deals_SellerId_fkey",
                        column: x => x.SellerId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                });

            migrationBuilder.CreateTable(
                name: "AssignmentHistory",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    ListingId = table.Column<int>(type: "integer", nullable: true),
                    RequestId = table.Column<int>(type: "integer", nullable: true),
                    DealId = table.Column<int>(type: "integer", nullable: true),
                    AssignedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UnassignedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AssignmentType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AssignmentHistory_pkey", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "AssignmentHistory_DealId_fkey",
                        column: x => x.DealId,
                        principalTable: "Deals",
                        principalColumn: "DealId");
                    table.ForeignKey(
                        name: "AssignmentHistory_EmployeeId_fkey",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "AssignmentHistory_ListingId_fkey",
                        column: x => x.ListingId,
                        principalTable: "PropertyListings",
                        principalColumn: "ListingId");
                    table.ForeignKey(
                        name: "AssignmentHistory_RequestId_fkey",
                        column: x => x.RequestId,
                        principalTable: "ClientRequests",
                        principalColumn: "RequestId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_BuildingId",
                table: "Apartments",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentHistory_DealId",
                table: "AssignmentHistory",
                column: "DealId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentHistory_EmployeeId",
                table: "AssignmentHistory",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentHistory_ListingId",
                table: "AssignmentHistory",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentHistory_RequestId",
                table: "AssignmentHistory",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_AddressId",
                table: "Buildings",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_ClientId",
                table: "ClientRequests",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_EmployeeId",
                table: "ClientRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_PropertyType",
                table: "ClientRequests",
                column: "PropertyType");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_Status",
                table: "ClientRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_RegistrationAddressId",
                table: "Clients",
                column: "RegistrationAddressId");

            migrationBuilder.CreateIndex(
                name: "Unique_Email",
                table: "Clients",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Unique_Phone",
                table: "Clients",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deals_BuyerId",
                table: "Deals",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_EmployeeId",
                table: "Deals",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_ListingId",
                table: "Deals",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_RequestId",
                table: "Deals",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_SellerId",
                table: "Deals",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "Unique_Employee_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Unique_Employee_Phone",
                table: "Employees",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandPlots_AddressId",
                table: "LandPlots",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "LandPlots_CadastralNumber_key",
                table: "LandPlots",
                column: "CadastralNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateHouses_LandPlotId",
                table: "PrivateHouses",
                column: "LandPlotId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyListings_ActiveRents",
                table: "PropertyListings",
                column: "ListingId",
                filter: "(((\"Status\")::text = 'Active'::text) AND ((\"TransactionType\")::text = 'Rent'::text))");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyListings_ActiveSales",
                table: "PropertyListings",
                column: "ListingId",
                filter: "(((\"Status\")::text = 'Active'::text) AND ((\"TransactionType\")::text = 'Sale'::text))");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyListings_ApartmentId",
                table: "PropertyListings",
                column: "ApartmentId",
                filter: "(\"ApartmentId\" IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyListings_ClientId",
                table: "PropertyListings",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyListings_EmployeeId",
                table: "PropertyListings",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyListings_LandPlotId",
                table: "PropertyListings",
                column: "LandPlotId",
                filter: "(\"LandPlotId\" IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyListings_PrivateHouseId",
                table: "PropertyListings",
                column: "PrivateHouseId",
                filter: "(\"PrivateHouseId\" IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyListings_Status",
                table: "PropertyListings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyListings_TransactionType",
                table: "PropertyListings",
                column: "TransactionType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentHistory");

            migrationBuilder.DropTable(
                name: "Deals");

            migrationBuilder.DropTable(
                name: "PropertyListings");

            migrationBuilder.DropTable(
                name: "ClientRequests");

            migrationBuilder.DropTable(
                name: "Apartments");

            migrationBuilder.DropTable(
                name: "PrivateHouses");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "LandPlots");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
