BEGIN TRY
    PRINT SOUNDEX('🙂');
END TRY
BEGIN CATCH
    PRINT ERROR_MESSAGE();
END CATCH
--output--
argument should be ASCII string
