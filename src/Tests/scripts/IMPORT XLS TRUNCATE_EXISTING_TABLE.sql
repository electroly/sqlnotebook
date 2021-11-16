-- Option not specified = append
CREATE TABLE table1 (foo, bar);
INSERT INTO table1 VALUES (999, 'ZZZ');
IMPORT XLS '<FILES>\excel.xlsx' INTO table1;
SELECT * FROM table1;

-- 0 = Append
CREATE TABLE table2 (foo, bar);
INSERT INTO table2 VALUES (999, 'ZZZ');
IMPORT XLS '<FILES>\excel.xlsx' INTO table2 OPTIONS (TRUNCATE_EXISTING_TABLE: 0);
SELECT * FROM table2;

-- 1 = Truncate
CREATE TABLE table3 (foo, bar);
INSERT INTO table3 VALUES (999, 'ZZZ');
IMPORT XLS '<FILES>\excel.xlsx' INTO table3 OPTIONS (TRUNCATE_EXISTING_TABLE: 1);
SELECT * FROM table3;

-- 0 has no effect if the table doesn't exist.
IMPORT XLS '<FILES>\excel.xlsx' INTO table4 OPTIONS (TRUNCATE_EXISTING_TABLE: 0);
SELECT * FROM table4;

-- 1 has no effect if the table doesn't exist.
IMPORT XLS '<FILES>\excel.xlsx' INTO table5 OPTIONS (TRUNCATE_EXISTING_TABLE: 1);
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
