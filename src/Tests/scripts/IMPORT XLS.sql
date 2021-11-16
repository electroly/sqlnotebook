-- No worksheet specified
IMPORT XLS '<FILES>\excel.xls' INTO table1;
SELECT * FROM table1;

-- Worksheet number
IMPORT XLS '<FILES>\excel.xls' WORKSHEET 2 INTO table2;
SELECT * FROM table2;

-- Worksheet name
IMPORT XLS '<FILES>\excel.xls' WORKSHEET 'SecondSheet' INTO table3;
SELECT * FROM table3;

-- Source columns
IMPORT XLS '<FILES>\excel.xls' INTO table4 (bar, foo);
SELECT * FROM table4;

-- Renamed columns
IMPORT XLS '<FILES>\excel.xls' INTO table5 (bar AS aaa, foo AS bbb);
SELECT * FROM table5;

--output--
foo,bar
111,HELLO
222,WORLD
-
colA,colB
111,222
333,444
555,666
777,888
-
colA,colB
111,222
333,444
555,666
777,888
-
bar,foo
HELLO,111
WORLD,222
-
aaa,bbb
HELLO,111
WORLD,222
-
