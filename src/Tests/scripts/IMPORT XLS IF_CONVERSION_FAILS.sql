-- Default = import as plain text
IMPORT XLS '<FILES>\int-convert-fail.xlsx' INTO table1 (foo INTEGER, bar TEXT);
SELECT *, TYPEOF(foo) AS footype FROM table1;

-- 1 = import as plain text
IMPORT XLS '<FILES>\int-convert-fail.xlsx' INTO table2 (foo INTEGER, bar TEXT) OPTIONS (IF_CONVERSION_FAILS: 1);
SELECT *, TYPEOF(foo) AS footype FROM table2;

-- 2 = skip the data row
IMPORT XLS '<FILES>\int-convert-fail.xlsx' INTO table3 (foo INTEGER, bar TEXT) OPTIONS (IF_CONVERSION_FAILS: 2);
SELECT *, TYPEOF(foo) AS footype FROM table3;

-- 3 = abort with an error
BEGIN TRY
    IMPORT XLS '<FILES>\int-convert-fail.xlsx' INTO table4 (foo INTEGER, bar TEXT) OPTIONS (IF_CONVERSION_FAILS: 3);
END TRY
BEGIN CATCH
    SELECT ERROR_MESSAGE() AS message
END CATCH

--output--
foo,bar,footype
111,HELLO,integer
ZZZ,WORLD,text
-
foo,bar,footype
111,HELLO,integer
ZZZ,WORLD,text
-
foo,bar,footype
111,HELLO,integer
-
message
Failed to parse input value as type "Integer". Value: "ZZZ".
-
