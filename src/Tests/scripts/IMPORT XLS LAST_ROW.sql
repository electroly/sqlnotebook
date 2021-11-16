-- Last row but no first row
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 2 INTO table1 OPTIONS (LAST_ROW: 3);
SELECT * FROM table1;

-- Last row is beyond the end of the data
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 2 INTO table2 OPTIONS (LAST_ROW: 200);
SELECT * FROM table2;

-- 0 = whole file
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 2 INTO table3 OPTIONS (LAST_ROW: 0);
SELECT * FROM table3;

--output--
colA,colB
111,222
333,444
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
