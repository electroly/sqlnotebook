-- Omitted = append
CREATE TABLE table1 (foo, bar);
INSERT INTO table1 VALUES (999, 'ZZZ');
IMPORT CSV '<FILES>\utf8.csv' INTO table1 (foo, bar);
SELECT * FROM table1;

-- 0 = append
CREATE TABLE table2 (foo, bar);
INSERT INTO table2 VALUES (999, 'ZZZ');
IMPORT CSV '<FILES>\utf8.csv' INTO table2 (foo, bar) OPTIONS (TRUNCATE_EXISTING_TABLE: 0);
SELECT * FROM table2;

-- 0 when table doesn't exist
IMPORT CSV '<FILES>\utf8.csv' INTO table3 (foo, bar) OPTIONS (TRUNCATE_EXISTING_TABLE: 0);
SELECT * FROM table3;

-- 1 = truncate
CREATE TABLE table4 (foo, bar);
INSERT INTO table1 VALUES (999, 'ZZZ');
IMPORT CSV '<FILES>\utf8.csv' INTO table4 (foo, bar) OPTIONS (TRUNCATE_EXISTING_TABLE: 1);
SELECT * FROM table4;

-- 1 when table doesn't exist
IMPORT CSV '<FILES>\utf8.csv' INTO table5 (foo, bar) OPTIONS (TRUNCATE_EXISTING_TABLE: 1);
SELECT * FROM table5;

--output--
foo,bar
999,ZZZ
111,HELLO
222,WORLD
-
foo,bar
999,ZZZ
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
222,WORLD
-
