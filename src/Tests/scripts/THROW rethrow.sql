BEGIN TRY
    BEGIN TRY
        THROW 'Oops';
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END TRY
BEGIN CATCH
    PRINT ERROR_MESSAGE();
END CATCH

--output--
Oops
