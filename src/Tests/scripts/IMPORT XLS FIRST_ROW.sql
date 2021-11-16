-- No row range
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 2 INTO table1;
SELECT * FROM table1;

-- First row but no last row
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 2 INTO table2 OPTIONS (FIRST_ROW: 2);
SELECT * FROM table2;

-- Both first and last row
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 2 INTO table3 OPTIONS (FIRST_ROW: 2, LAST_ROW: 3);
SELECT * FROM table3;

-- First row is beyond the end of the data
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 2 INTO table4 OPTIONS (FIRST_ROW: 200);
SELECT * FROM table4;

--output--
colA,colB
111,222
333,444
555,666
777,888
-
111,222
333,444
555,666
777,888
-
111,222
333,444
-
column1
-
