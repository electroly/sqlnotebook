-- Omitted = header row
IMPORT CSV '<FILES>\utf8.csv' INTO table1;
SELECT * FROM table1;

-- 1 = header row
IMPORT CSV '<FILES>\utf8.csv' INTO table2 OPTIONS (HEADER_ROW: 1);
SELECT * FROM table2;

-- 1 with SKIP_LINES
IMPORT CSV '<FILES>\utf8.csv' INTO table3 OPTIONS (HEADER_ROW: 1, SKIP_LINES: 1);
SELECT * FROM table3;

-- 0 = no header row
IMPORT CSV '<FILES>\utf8.csv' INTO table4 OPTIONS (HEADER_ROW: 0);
SELECT * FROM table4;

-- 0 with SKIP_LINES
IMPORT CSV '<FILES>\utf8.csv' INTO table5 OPTIONS (HEADER_ROW: 0, SKIP_LINES: 1);
SELECT * FROM table5;

--output--
foo,bar
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
222,WORLD
-
111,HELLO
222,WORLD
-
column1,column2
foo,bar
111,HELLO
222,WORLD
-
column1,column2
111,HELLO
222,WORLD
-
