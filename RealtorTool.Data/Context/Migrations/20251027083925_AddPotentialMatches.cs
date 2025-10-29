using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtorTool.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddPotentialMatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DesiredRealtyType",
                table: "ClientRequests",
                type: "integer",
                nullable: true);
            
            var sqlScript = File.ReadAllText(@"Migrations/Scripts/CreatePotentialMatchesView.sql");
            migrationBuilder.Sql(sqlScript);

            migrationBuilder.Sql(@"
                CREATE VIEW PotentialMatches AS
                SELECT 
                    buy_req.""Id"" AS BuyRequestId,
                    sell_req.""Id"" AS SellRequestId,
                    buy_req.""Type"" AS BuyType,
                    sell_req.""Type"" AS SellType,
                    buy_req.""MaxPrice"" AS MaxPrice,
                    listing.""Price"" AS ListingPrice,
                    realty.""RealtyType"" AS RealtyType,
                    realty.""TotalArea"" AS TotalArea,
                    address.""City"" AS City,
                    address.""District"" AS District,
                    buy_req.""MinRooms"" AS MinRooms,
                    buy_req.""MinArea"" AS MinArea,
                    buy_req.""MaxArea"" AS MaxArea,
                    buy_req.""DesiredLocation"" AS DesiredLocation,
                    
                    CASE 
                        WHEN realty.""RealtyType"" = 0 THEN apartment.""RoomsCount""
                        WHEN realty.""RealtyType"" = 2 THEN private_house.""RoomsCount""
                        ELSE NULL
                    END AS RoomsCount

                FROM ""ClientRequests"" AS buy_req
                INNER JOIN ""ClientRequests"" AS sell_req ON 
                    ((buy_req.""Type"" = 2 AND sell_req.""Type"" = 3) OR (buy_req.""Type"" = 0 AND sell_req.""Type"" = 1))
                    AND buy_req.""Status"" IN (0, 1)  -- New, InProgress
                    AND sell_req.""Status"" IN (0, 1)

                INNER JOIN ""Listings"" AS listing ON sell_req.""ListingId"" = listing.""Id""
                INNER JOIN ""Realties"" AS realty ON listing.""RealtyId"" = realty.""Id""
                INNER JOIN ""Addresses"" AS address ON realty.""AddressId"" = address.""Id""

                LEFT JOIN ""Apartments"" AS apartment ON realty.""Id"" = apartment.""Id"" AND realty.""RealtyType"" = 0
                LEFT JOIN ""PrivateHouses"" AS private_house ON realty.""Id"" = private_house.""Id"" AND realty.""RealtyType"" = 2
                LEFT JOIN ""Areas"" AS area ON realty.""Id"" = area.""Id"" AND realty.""RealtyType"" = 1

                WHERE 
                    realty.""IsActive"" = true
                    AND (
                        (buy_req.""DesiredRealtyType"" = 0 AND realty.""RealtyType"" IN (0, 2))
                        OR
                        (buy_req.""DesiredRealtyType"" = 2 AND realty.""RealtyType"" = 2)
                        OR
                        (buy_req.""DesiredRealtyType"" = 1 AND realty.""RealtyType"" = 1)
                    )
                    AND (buy_req.""MaxPrice"" IS NULL OR listing.""Price"" <= buy_req.""MaxPrice"")
                    AND (buy_req.""MinArea"" IS NULL OR realty.""TotalArea"" >= buy_req.""MinArea"")
                    AND (buy_req.""MaxArea"" IS NULL OR realty.""TotalArea"" <= buy_req.""MaxArea"")
                    AND (
                        buy_req.""MinRooms"" IS NULL 
                        OR 
                        (realty.""RealtyType"" = 0 AND apartment.""RoomsCount"" >= buy_req.""MinRooms"")
                        OR 
                        (realty.""RealtyType"" = 2 AND private_house.""RoomsCount"" >= buy_req.""MinRooms"")
                    )
                    AND (
                        buy_req.""DesiredLocation"" IS NULL 
                        OR 
                        address.""City"" ILIKE '%' || buy_req.""DesiredLocation"" || '%'
                        OR 
                        address.""District"" ILIKE '%' || buy_req.""DesiredLocation"" || '%'
                    );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS potential_matches");
            
            migrationBuilder.DropColumn(
                name: "DesiredRealtyType",
                table: "ClientRequests");
        }
    }
}
