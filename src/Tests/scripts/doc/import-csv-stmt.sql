-- All column imported as text.
IMPORT CSV '<FILES>\utf8.csv' INTO table1;

SELECT * FROM table1;

-- Selected columns imported as text.
IMPORT CSV '<FILES>\utf8.csv' INTO table2 (foo);

SELECT * FROM table2;

-- Selected columns imported with specified data conversions, falling
-- back to text for any value in the input data that can't be converted.
IMPORT CSV '<FILES>\utf8.csv' INTO table3 (foo INTEGER, bar TEXT);

SELECT * FROM table3;

-- No header row in the file. Default names "column1", "column2", etc.
-- can be renamed with "AS".
IMPORT CSV '<FILES>\utf8.csv' INTO table4 (column1 AS foo, column2 AS bar)
OPTIONS (HEADER_ROW: 0);

SELECT * FROM table4;

-- Semicolon-separated file.
IMPORT CSV '<FILES>\semicolon.csv' INTO table5 OPTIONS (SEPARATOR: ';');

SELECT * FROM table5;

--output--
foo,bar
111,HELLO
222,WORLD
-
foo
111
222
-
foo,bar
111,HELLO
222,WORLD
-
foo,bar
foo,bar
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
222,WORLD
-
