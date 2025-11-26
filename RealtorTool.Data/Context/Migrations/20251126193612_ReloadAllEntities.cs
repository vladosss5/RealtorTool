using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtorTool.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class ReloadAllEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlDropPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Context", "Migrations", "Scripts", "DropPotentialMatchesView.sql");
            var sqlDropScript = File.ReadAllText(sqlDropPath);
            migrationBuilder.Sql(sqlDropScript);
            
            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_DictionaryValues_BathroomTypeId",
                table: "Apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_DictionaryValues_RenovationTypeId",
                table: "Apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_Areas_DictionaryValues_LandCategoryId",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientRequests_Deals_DealId",
                table: "ClientRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Photos_PhotoId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_DealParticipants_Deals_DealId",
                table: "DealParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_DictionaryValues_DealTypeId",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Photos_PhotoId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_DictionaryValues_CurrencyId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_DictionaryValues_ListingTypeId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_DictionaryValues_StatusId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateHouses_DictionaryValues_ConstructionMaterialId",
                table: "PrivateHouses");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateHouses_DictionaryValues_HeatingTypeId",
                table: "PrivateHouses");

            migrationBuilder.DropIndex(
                name: "IX_Realties_IsActive",
                table: "Realties");

            migrationBuilder.DropIndex(
                name: "IX_Realties_TotalArea",
                table: "Realties");

            migrationBuilder.DropIndex(
                name: "IX_PrivateHouses_FloorsCount",
                table: "PrivateHouses");

            migrationBuilder.DropIndex(
                name: "IX_PrivateHouses_RoomsCount",
                table: "PrivateHouses");

            migrationBuilder.DropIndex(
                name: "IX_PrivateHouses_YearBuilt",
                table: "PrivateHouses");

            migrationBuilder.DropIndex(
                name: "IX_Photos_CreatedDate",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_EntityType",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_IsMain",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_SortOrder",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Listings_Price",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_StatusId_CreatedDate",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_DictionaryValues_DictionaryId_Value",
                table: "DictionaryValues");

            migrationBuilder.DropIndex(
                name: "IX_DictionaryValues_Value",
                table: "DictionaryValues");

            migrationBuilder.DropIndex(
                name: "IX_Dictionaries_Type",
                table: "Dictionaries");

            migrationBuilder.DropIndex(
                name: "IX_Deals_FinalPrice",
                table: "Deals");

            migrationBuilder.DropIndex(
                name: "IX_Deals_StatusId_DealDate",
                table: "Deals");

            migrationBuilder.DropIndex(
                name: "IX_DealParticipants_DealId_ClientRequestId",
                table: "DealParticipants");

            migrationBuilder.DropIndex(
                name: "IX_DealParticipants_Role",
                table: "DealParticipants");

            migrationBuilder.DropIndex(
                name: "IX_Clients_Phone",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_ClientRequests_CreatedDate",
                table: "ClientRequests");

            migrationBuilder.DropIndex(
                name: "IX_ClientRequests_Status_Type",
                table: "ClientRequests");

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
                name: "IX_Addresses_City_District_Street_HouseNumber_BuildingNumber",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_District",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_PostalCode",
                table: "Addresses");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Realties",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Realties",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Photos",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "Photos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Terms",
                table: "Listings",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ListingTypeId",
                table: "Listings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "Employees",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Employees",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Employees",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Employees",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Employees",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Employees",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "DictionaryValues",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Dictionaries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationAddress",
                table: "Clients",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Clients",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PassportSeries",
                table: "Clients",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PassportNumber",
                table: "Clients",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Clients",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Clients",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Clients",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DesiredLocation",
                table: "ClientRequests",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalRequirements",
                table: "ClientRequests",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApartmentNumber",
                table: "Apartments",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "Addresses",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "Addresses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HouseNumber",
                table: "Addresses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "Addresses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BuildingNumber",
                table: "Addresses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EntityType_EntityId",
                table: "Photos",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Login",
                table: "Employees",
                column: "Login");

            migrationBuilder.CreateIndex(
                name: "IX_DealParticipants_DealId",
                table: "DealParticipants",
                column: "DealId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Phone",
                table: "Clients",
                column: "Phone");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_DictionaryValues_BathroomTypeId",
                table: "Apartments",
                column: "BathroomTypeId",
                principalTable: "DictionaryValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_DictionaryValues_RenovationTypeId",
                table: "Apartments",
                column: "RenovationTypeId",
                principalTable: "DictionaryValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_DictionaryValues_LandCategoryId",
                table: "Areas",
                column: "LandCategoryId",
                principalTable: "DictionaryValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientRequests_Deals_DealId",
                table: "ClientRequests",
                column: "DealId",
                principalTable: "Deals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Photos_PhotoId",
                table: "Clients",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DealParticipants_Deals_DealId",
                table: "DealParticipants",
                column: "DealId",
                principalTable: "Deals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_DictionaryValues_DealTypeId",
                table: "Deals",
                column: "DealTypeId",
                principalTable: "DictionaryValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Photos_PhotoId",
                table: "Employees",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_DictionaryValues_CurrencyId",
                table: "Listings",
                column: "CurrencyId",
                principalTable: "DictionaryValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_DictionaryValues_ListingTypeId",
                table: "Listings",
                column: "ListingTypeId",
                principalTable: "DictionaryValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_DictionaryValues_StatusId",
                table: "Listings",
                column: "StatusId",
                principalTable: "DictionaryValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateHouses_DictionaryValues_ConstructionMaterialId",
                table: "PrivateHouses",
                column: "ConstructionMaterialId",
                principalTable: "DictionaryValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateHouses_DictionaryValues_HeatingTypeId",
                table: "PrivateHouses",
                column: "HeatingTypeId",
                principalTable: "DictionaryValues",
                principalColumn: "Id");
            
            var sqlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Context", "Migrations", "Scripts", "CreatePotentialMatchesView.sql");
            var sqlScript = File.ReadAllText(sqlPath);
            migrationBuilder.Sql(sqlScript);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sqlDropPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Context", "Migrations", "Scripts", "DropPotentialMatchesView.sql");
            var sqlDropScript = File.ReadAllText(sqlDropPath);
            migrationBuilder.Sql(sqlDropScript);
            
            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_DictionaryValues_BathroomTypeId",
                table: "Apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_DictionaryValues_RenovationTypeId",
                table: "Apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_Areas_DictionaryValues_LandCategoryId",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientRequests_Deals_DealId",
                table: "ClientRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Photos_PhotoId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_DealParticipants_Deals_DealId",
                table: "DealParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_DictionaryValues_DealTypeId",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Photos_PhotoId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_DictionaryValues_CurrencyId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_DictionaryValues_ListingTypeId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_DictionaryValues_StatusId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateHouses_DictionaryValues_ConstructionMaterialId",
                table: "PrivateHouses");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateHouses_DictionaryValues_HeatingTypeId",
                table: "PrivateHouses");

            migrationBuilder.DropIndex(
                name: "IX_Photos_EntityType_EntityId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Login",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PhotoId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_DealParticipants_DealId",
                table: "DealParticipants");

            migrationBuilder.DropIndex(
                name: "IX_Clients_Phone",
                table: "Clients");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Realties",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Realties",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Photos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "Photos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Terms",
                table: "Listings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ListingTypeId",
                table: "Listings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "Employees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Employees",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Employees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Employees",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Employees",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Employees",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "DictionaryValues",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Dictionaries",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationAddress",
                table: "Clients",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Clients",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "PassportSeries",
                table: "Clients",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PassportNumber",
                table: "Clients",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Clients",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Clients",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Clients",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "DesiredLocation",
                table: "ClientRequests",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalRequirements",
                table: "ClientRequests",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApartmentNumber",
                table: "Apartments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "Addresses",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HouseNumber",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BuildingNumber",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Realties_IsActive",
                table: "Realties",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Realties_TotalArea",
                table: "Realties",
                column: "TotalArea");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateHouses_FloorsCount",
                table: "PrivateHouses",
                column: "FloorsCount");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateHouses_RoomsCount",
                table: "PrivateHouses",
                column: "RoomsCount");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateHouses_YearBuilt",
                table: "PrivateHouses",
                column: "YearBuilt");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_CreatedDate",
                table: "Photos",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EntityType",
                table: "Photos",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_IsMain",
                table: "Photos",
                column: "IsMain");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_SortOrder",
                table: "Photos",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_Price",
                table: "Listings",
                column: "Price");

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
                name: "IX_DictionaryValues_Value",
                table: "DictionaryValues",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_Dictionaries_Type",
                table: "Dictionaries",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deals_FinalPrice",
                table: "Deals",
                column: "FinalPrice");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_StatusId_DealDate",
                table: "Deals",
                columns: new[] { "StatusId", "DealDate" });

            migrationBuilder.CreateIndex(
                name: "IX_DealParticipants_DealId_ClientRequestId",
                table: "DealParticipants",
                columns: new[] { "DealId", "ClientRequestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealParticipants_Role",
                table: "DealParticipants",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Phone",
                table: "Clients",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_CreatedDate",
                table: "ClientRequests",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_Status_Type",
                table: "ClientRequests",
                columns: new[] { "Status", "Type" });

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
                name: "IX_Addresses_City_District_Street_HouseNumber_BuildingNumber",
                table: "Addresses",
                columns: new[] { "City", "District", "Street", "HouseNumber", "BuildingNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_District",
                table: "Addresses",
                column: "District");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PostalCode",
                table: "Addresses",
                column: "PostalCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_DictionaryValues_BathroomTypeId",
                table: "Apartments",
                column: "BathroomTypeId",
                principalTable: "DictionaryValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_DictionaryValues_RenovationTypeId",
                table: "Apartments",
                column: "RenovationTypeId",
                principalTable: "DictionaryValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_DictionaryValues_LandCategoryId",
                table: "Areas",
                column: "LandCategoryId",
                principalTable: "DictionaryValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientRequests_Deals_DealId",
                table: "ClientRequests",
                column: "DealId",
                principalTable: "Deals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Photos_PhotoId",
                table: "Clients",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DealParticipants_Deals_DealId",
                table: "DealParticipants",
                column: "DealId",
                principalTable: "Deals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_DictionaryValues_DealTypeId",
                table: "Deals",
                column: "DealTypeId",
                principalTable: "DictionaryValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Photos_PhotoId",
                table: "Employees",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_DictionaryValues_CurrencyId",
                table: "Listings",
                column: "CurrencyId",
                principalTable: "DictionaryValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_DictionaryValues_ListingTypeId",
                table: "Listings",
                column: "ListingTypeId",
                principalTable: "DictionaryValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_DictionaryValues_StatusId",
                table: "Listings",
                column: "StatusId",
                principalTable: "DictionaryValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateHouses_DictionaryValues_ConstructionMaterialId",
                table: "PrivateHouses",
                column: "ConstructionMaterialId",
                principalTable: "DictionaryValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateHouses_DictionaryValues_HeatingTypeId",
                table: "PrivateHouses",
                column: "HeatingTypeId",
                principalTable: "DictionaryValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            
            var sqlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Context", "Migrations", "Scripts", "CreatePotentialMatchesView.sql");
            var sqlScript = File.ReadAllText(sqlPath);
            migrationBuilder.Sql(sqlScript);
        }
    }
}
