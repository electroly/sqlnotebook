-- Omitted = header
IMPORT XLS '<FILES>\excel.xls' INTO table1;
SELECT * FROM table1;

-- 1 = header
IMPORT XLS '<FILES>\excel.xls' INTO table2 OPTIONS (HEADER_ROW: 1);
SELECT * FROM table2;

-- 0 = no header
IMPORT XLS '<FILES>\excel.xls' INTO table3 OPTIONS (HEADER_ROW: 0);
SELECT * FROM table3;

--output--
foo,bar
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
222,WORLD
-
column1,column2
foo,bar
111,HELLO
222,WORLD
-
