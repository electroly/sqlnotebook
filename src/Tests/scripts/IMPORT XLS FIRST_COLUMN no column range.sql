-- No column range
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table1;
SELECT * FROM table1;

--output--
first,second,third,fourth
A,1,0,111
B,2,1,222
C,3,10,333
D,4,11,444
E,5,100,555
-
