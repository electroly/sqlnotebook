PRINT TO_DATE('2016/5/9 5:39 PM');

BEGIN TRY
    PRINT TO_DATE('foobar')
END TRY
BEGIN CATCH
    PRINT ERROR_MESSAGE();
END CATCH

--output--
2016-05-09
TO_DATE: The "input" argument must be a valid date string.
