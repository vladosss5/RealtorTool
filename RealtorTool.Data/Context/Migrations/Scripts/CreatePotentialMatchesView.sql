-- =============================================
-- УЛУЧШЕННЫЕ ФУНКЦИИ ДЛЯ ОПТИМИЗАЦИИ ПОИСКА
-- =============================================

-- Более гибкая функция для поиска по локации
CREATE OR REPLACE FUNCTION "AddressMatch"(
    "DesiredLocation" TEXT,
    "City" TEXT,
    "District" TEXT
) RETURNS BOOLEAN AS $$
BEGIN
    RETURN (
        "DesiredLocation" IS NULL OR
        "DesiredLocation" = '' OR
        "City" ILIKE '%' || "DesiredLocation" || '%' OR
        "District" ILIKE '%' || "DesiredLocation" || '%' OR
        "DesiredLocation" ILIKE '%' || "City" || '%' OR
        "DesiredLocation" ILIKE '%' || "District" || '%'
        );
END;
$$ LANGUAGE plpgsql IMMUTABLE;

-- =============================================
-- УЛУЧШЕННОЕ ПРЕДСТАВЛЕНИЕ ДЛЯ МАТЧИНГА
-- =============================================

CREATE OR REPLACE VIEW "PotentialMatches" AS
WITH "RequestPairs" AS (
    -- Все возможные пары заявок покупка-продажа и аренда-сдача
    SELECT
        buy_req."Id" AS "BuyRequestId",
        sell_req."Id" AS "SellRequestId",
        buy_req."Type" AS "BuyType",
        sell_req."Type" AS "SellType",
        buy_req."ClientId" AS "BuyerId",
        sell_req."ClientId" AS "SellerId",
        buy_req."MaxPrice" AS "MaxPrice",
        buy_req."MinRooms" AS "MinRooms",
        buy_req."MinArea" AS "MinArea",
        buy_req."MaxArea" AS "MaxArea",
        buy_req."DesiredLocation" AS "DesiredLocation",
        buy_req."DesiredRealtyType" AS "DesiredRealtyType",
        sell_req."ListingId" AS "SellListingId"

    FROM "ClientRequests" buy_req
             CROSS JOIN "ClientRequests" sell_req
    WHERE
      -- Покупка ↔ Продажа ИЛИ Аренда ↔ Сдача
        (
            (buy_req."Type" = 2 AND sell_req."Type" = 3) OR  -- Покупка ↔ Продажа
            (buy_req."Type" = 0 AND sell_req."Type" = 1)     -- Аренда ↔ Сдача
            )
      AND buy_req."Status" IN (0, 1)  -- Активные заявки на покупку/аренду
      AND sell_req."Status" IN (0, 1)  -- Активные заявки на продажу/сдачу
      AND buy_req."ClientId" != sell_req."ClientId"  -- Исключаем самоматчинг
),
     "ListingData" AS (
         SELECT
             rp.*,
             listing."Price" AS "ListingPrice",
             listing."RealtyId" AS "RealtyId",
             realty."RealtyType" AS "RealtyType",
             realty."TotalArea" AS "TotalArea",
             realty."Name" AS "RealtyName",
             realty."IsActive" AS "IsActive",
             address."City" AS "City",
             address."District" AS "District",
             -- Детали по типам недвижимости
             CASE
                 WHEN realty."RealtyType" = 0 THEN apartment."RoomsCount"
                 WHEN realty."RealtyType" = 2 THEN private_house."RoomsCount"
                 ELSE NULL
                 END AS "ActualRoomsCount"

         FROM "RequestPairs" rp
                  INNER JOIN "Listings" listing ON rp."SellListingId" = listing."Id"
                  INNER JOIN "Realties" realty ON listing."RealtyId" = realty."Id" AND realty."IsActive" = true
                  INNER JOIN "Addresses" address ON realty."AddressId" = address."Id"
                  LEFT JOIN "Apartments" apartment ON realty."Id" = apartment."Id" AND realty."RealtyType" = 0
                  LEFT JOIN "PrivateHouses" private_house ON realty."Id" = private_house."Id" AND realty."RealtyType" = 2
                  LEFT JOIN "Areas" area ON realty."Id" = area."Id" AND realty."RealtyType" = 1
     ),
     "TypeMatches" AS (
         SELECT *,
                CASE
                    -- Если хотят квартиру - показываем квартиры И дома
                    WHEN "DesiredRealtyType" = 0 AND "RealtyType" IN (0, 2) THEN true
                    -- Если хотят дом - показываем только дома  
                    WHEN "DesiredRealtyType" = 2 AND "RealtyType" = 2 THEN true
                    -- Если хотят участок - показываем только участки
                    WHEN "DesiredRealtyType" = 1 AND "RealtyType" = 1 THEN true
                    -- Если тип не указан - показываем всё
                    WHEN "DesiredRealtyType" IS NULL THEN true
                    ELSE false
                    END AS "TypeMatch"
         FROM "ListingData"
     )
SELECT
    "BuyRequestId",
    "SellRequestId",
    "BuyType",
    "SellType",
    "BuyerId",
    "SellerId",
    "MaxPrice",
    "ListingPrice",
    "RealtyId",
    "SellListingId",
    "RealtyType",
    "TotalArea",
    "RealtyName",
    "City",
    "District",
    "MinRooms",
    "MinArea",
    "MaxArea",
    "DesiredLocation",
    "DesiredRealtyType",
    "ActualRoomsCount",
    "TypeMatch",

    -- Рейтинг совпадения (0-4 балла)
    CASE WHEN "MaxPrice" IS NULL OR "ListingPrice" <= "MaxPrice" THEN 1 ELSE 0 END +
    CASE WHEN "MinArea" IS NULL OR "TotalArea" >= "MinArea" THEN 1 ELSE 0 END +
    CASE WHEN "MaxArea" IS NULL OR "TotalArea" <= "MaxArea" THEN 1 ELSE 0 END +
    CASE WHEN "MinRooms" IS NULL OR "ActualRoomsCount" >= "MinRooms" THEN 1 ELSE 0 END AS "MatchScore",

    -- Флаги совпадения по критериям
    CASE WHEN "MaxPrice" IS NULL OR "ListingPrice" <= "MaxPrice" THEN true ELSE false END AS "PriceMatch",
    CASE WHEN "MinArea" IS NULL OR "TotalArea" >= "MinArea" THEN true ELSE false END AS "MinAreaMatch",
    CASE WHEN "MaxArea" IS NULL OR "TotalArea" <= "MaxArea" THEN true ELSE false END AS "MaxAreaMatch",
    CASE WHEN "MinRooms" IS NULL OR "ActualRoomsCount" >= "MinRooms" THEN true ELSE false END AS "RoomsMatch",
    CASE WHEN "DesiredLocation" IS NULL OR "DesiredLocation" = '' OR "AddressMatch"("DesiredLocation", "City", "District") THEN true ELSE false END AS "LocationMatch"

FROM "TypeMatches"
WHERE "TypeMatch" = true
ORDER BY "MatchScore" DESC, "ListingPrice" ASC;

-- =============================================
-- ПРЕДСТАВЛЕНИЕ С ДЕТАЛИЗАЦИЕЙ
-- =============================================

CREATE OR REPLACE VIEW "RequestMatches" AS
SELECT
    pm.*,
    buyer."FirstName" || ' ' || buyer."LastName" AS "BuyerName",
    buyer."Phone" AS "BuyerPhone",
    seller."FirstName" || ' ' || seller."LastName" AS "SellerName",
    seller."Phone" AS "SellerPhone",
    emp_buy."FirstName" || ' ' || emp_buy."LastName" AS "BuyerAgent",
    emp_sell."FirstName" || ' ' || emp_sell."LastName" AS "SellerAgent",
    listing."Terms" AS "ListingTerms",
    -- Описание типа недвижимости
    CASE
        WHEN pm."RealtyType" = 0 THEN 'Квартира'
        WHEN pm."RealtyType" = 1 THEN 'Земельный участок'
        WHEN pm."RealtyType" = 2 THEN 'Частный дом'
        END AS "RealtyTypeDescription",
    -- Описание типа сделки
    CASE
        WHEN pm."BuyType" = 0 THEN 'Аренда'
        WHEN pm."BuyType" = 2 THEN 'Покупка'
        END AS "DealTypeDescription"

FROM "PotentialMatches" pm
         INNER JOIN "Clients" buyer ON pm."BuyerId" = buyer."Id"
         INNER JOIN "Clients" seller ON pm."SellerId" = seller."Id"
         INNER JOIN "ClientRequests" buy_req ON pm."BuyRequestId" = buy_req."Id"
         INNER JOIN "ClientRequests" sell_req ON pm."SellRequestId" = sell_req."Id"
         INNER JOIN "Employees" emp_buy ON buy_req."EmployeeId" = emp_buy."Id"
         INNER JOIN "Employees" emp_sell ON sell_req."EmployeeId" = emp_sell."Id"
         INNER JOIN "Listings" listing ON pm."SellListingId" = listing."Id";

-- =============================================
-- ФУНКЦИЯ ДЛЯ ПОИСКА МАТЧЕЙ ПО ЗАЯВКЕ
-- =============================================

CREATE OR REPLACE FUNCTION "GetMatchesForRequest"("RequestId" TEXT)
    RETURNS TABLE(
                     "MatchType" TEXT,
                     "MatchedRequestId" TEXT,
                     "ClientName" TEXT,
                     "Phone" TEXT,
                     "Price" NUMERIC,
                     "RealtyType" TEXT,
                     "TotalArea" NUMERIC,
                     "City" TEXT,
                     "District" TEXT,
                     "RoomsCount" INTEGER,
                     "MatchScore" INTEGER,
                     "MatchDetails" TEXT
                 ) AS $$
BEGIN
    -- Матчи где RequestId является покупателем
    RETURN QUERY
        SELECT
            'SELL_OFFER' AS "MatchType",
            pm."SellRequestId" AS "MatchedRequestId",
            seller."FirstName" || ' ' || seller."LastName" AS "ClientName",
            seller."Phone" AS "Phone",
            pm."ListingPrice" AS "Price",
            CASE
                WHEN pm."RealtyType" = 0 THEN 'Квартира'
                WHEN pm."RealtyType" = 1 THEN 'Участок'
                WHEN pm."RealtyType" = 2 THEN 'Дом'
                END AS "RealtyType",
            pm."TotalArea" AS "TotalArea",
            pm."City" AS "City",
            pm."District" AS "District",
            pm."ActualRoomsCount" AS "RoomsCount",
            pm."MatchScore" AS "MatchScore",
            'Предложение от продавца' AS "MatchDetails"
        FROM "PotentialMatches" pm
                 INNER JOIN "Clients" seller ON pm."SellerId" = seller."Id"
        WHERE pm."BuyRequestId" = "RequestId"

        UNION ALL

        -- Матчи где RequestId является продавцом
        SELECT
            'BUY_REQUEST' AS "MatchType",
            pm."BuyRequestId" AS "MatchedRequestId",
            buyer."FirstName" || ' ' || buyer."LastName" AS "ClientName",
            buyer."Phone" AS "Phone",
            pm."MaxPrice" AS "Price",
            CASE
                WHEN pm."RealtyType" = 0 THEN 'Квартира'
                WHEN pm."RealtyType" = 1 THEN 'Участок'
                WHEN pm."RealtyType" = 2 THEN 'Дом'
                END AS "RealtyType",
            pm."TotalArea" AS "TotalArea",
            pm."City" AS "City",
            pm."District" AS "District",
            pm."ActualRoomsCount" AS "RoomsCount",
            pm."MatchScore" AS "MatchScore",
            'Запрос от покупателя' AS "MatchDetails"
        FROM "PotentialMatches" pm
                 INNER JOIN "Clients" buyer ON pm."BuyerId" = buyer."Id"
        WHERE pm."SellRequestId" = "RequestId";
END;
$$ LANGUAGE plpgsql;