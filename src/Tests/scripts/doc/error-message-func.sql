-- Prints "Message".
BEGIN TRY  
    THROW 'Message';
END TRY  
BEGIN CATCH  
    PRINT ERROR_MESSAGE();
END CATCH;
--output--
Message
