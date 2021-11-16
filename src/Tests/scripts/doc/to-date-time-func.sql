PRINT TO_DATETIME('2016/5/9 5:39 PM');

BEGIN TRY
    PRINT TO_DATETIME('foobar')
END TRY
BEGIN CATCH
    PRINT ERROR_MESSAGE();
END CATCH

--output--
2016-05-09 17:39:00.000
TO_DATETIME: The "input" argument must be a valid date string.
