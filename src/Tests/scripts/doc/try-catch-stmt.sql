-- Prints "Foo".
BEGIN TRY
    THROW 'Foo';
END TRY
BEGIN CATCH
    PRINT ERROR_MESSAGE();
END CATCH
--output--
Foo
