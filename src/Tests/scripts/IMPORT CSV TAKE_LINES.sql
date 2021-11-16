-- Omitted = whole file
IMPORT CSV '<FILES>\utf8.csv' INTO table1;
SELECT * FROM table1;

-- 0 with headers = read columns, no data rows
IMPORT CSV '<FILES>\utf8.csv' INTO table2 OPTIONS (TAKE_LINES: 0);
SELECT * FROM table2;

-- 0 without headers = no column names, no data rows
IMPORT CSV '<FILES>\utf8.csv' INTO table3 OPTIONS (TAKE_LINES: 0, HEADER_ROW: 0);
SELECT * FROM table3;

-- -1 = whole file
IMPORT CSV '<FILES>\utf8.csv' INTO table4 OPTIONS (TAKE_LINES: -1);
SELECT * FROM table4;

-- Test taking some lines and skipping others.
IMPORT CSV '<FILES>\utf8.csv' INTO table5 OPTIONS (TAKE_LINES: 1);
SELECT * FROM table5;

-- Test taking the whole file with a number > number of lines in the file.
IMPORT CSV '<FILES>\utf8.csv' INTO table6 OPTIONS (TAKE_LINES: 100);
SELECT * FROM table6;

--output--
foo,bar
111,HELLO
222,WORLD
-
foo,bar
-
column1,column2
-
foo,bar
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
-
foo,bar
111,HELLO
222,WORLD
-
