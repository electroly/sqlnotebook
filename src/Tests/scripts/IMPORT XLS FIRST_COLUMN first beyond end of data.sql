-- First column is beyond the end of the data
BEGIN TRY
    IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table6 OPTIONS (FIRST_COLUMN: 100);
END TRY
BEGIN CATCH
    SELECT ERROR_MESSAGE() AS message;
END CATCH    

--output--
message
No columns chosen for import.
-
