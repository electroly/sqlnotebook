-- Both first and last column
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table4 OPTIONS (FIRST_COLUMN: 'B', LAST_COLUMN: 'C');
SELECT * FROM table4;

--output--
second,third
1,0
2,1
3,10
4,11
5,100
-
