-- By default, permanent table.
IMPORT CSV '<FILES>\utf8.csv' INTO table1;
SELECT * FROM table1;
SELECT COUNT(*) AS count FROM sqlite_master WHERE name = 'table1'; -- present

-- 0 = permanent table
IMPORT CSV '<FILES>\utf8.csv' INTO table2 OPTIONS (TEMPORARY_TABLE: 0);
SELECT * FROM table2;
SELECT COUNT(*) AS count FROM sqlite_master WHERE name = 'table2'; -- present

-- 1 = temporary table
IMPORT CSV '<FILES>\utf8.csv' INTO table3 OPTIONS (TEMPORARY_TABLE: 1);
SELECT * FROM table3;
SELECT COUNT(*) AS count FROM sqlite_master WHERE name = 'table3'; -- not present

--output--
foo,bar
111,HELLO
222,WORLD
-
count
1
-
foo,bar
111,HELLO
222,WORLD
-
count
1
-
foo,bar
111,HELLO
222,WORLD
-
count
0
-
