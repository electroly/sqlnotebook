-- First column but no last column
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table3 OPTIONS (FIRST_COLUMN: 2);
SELECT * FROM table3;

--output--
second,third,fourth
1,0,111
2,1,222
3,10,333
4,11,444
5,100,555
-
