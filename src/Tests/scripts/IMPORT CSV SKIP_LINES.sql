-- Omitted = don't skip anything
IMPORT CSV '<FILES>\utf8.csv' INTO table1;
SELECT * FROM table1;

-- 0 = don't skip anything
IMPORT CSV '<FILES>\utf8.csv' INTO table2 OPTIONS (SKIP_LINES: 0)
SELECT * FROM table2;

-- Test skipping some lines and returning the rest.
IMPORT CSV '<FILES>\utf8.csv' INTO table3 OPTIONS (SKIP_LINES: 1)
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
111,HELLO
222,WORLD
-
