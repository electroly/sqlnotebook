-- Both first and last column
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table5 OPTIONS (FIRST_COLUMN: 2, LAST_COLUMN: 3);
SELECT * FROM table5;

--output--
second,third
1,0
2,1
3,10
4,11
5,100
-
