BEGIN TRY
    PRINT LEVENSHTEIN('pickle', '🙂stick'); -- error: non-ASCII string
END TRY
BEGIN CATCH
    PRINT ERROR_MESSAGE();
END CATCH
--output--
arguments should be ASCII strings
