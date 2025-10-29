-- =============================================
-- УДАЛЕНИЕ ВСЕХ ЭЛЕМЕНТОВ СКРИПТА
-- =============================================
COMMENT ON VIEW "PotentialMatches" IS NULL;
COMMENT ON VIEW "RequestMatches" IS NULL;
COMMENT ON FUNCTION "AddressMatch" IS NULL;
COMMENT ON FUNCTION "GetMatchesForRequest" IS NULL;

DROP VIEW IF EXISTS "RequestMatches";
DROP VIEW IF EXISTS "PotentialMatches";

DROP FUNCTION IF EXISTS "GetMatchesForRequest"(TEXT);
DROP FUNCTION IF EXISTS "AddressMatch"(TEXT, TEXT, TEXT);