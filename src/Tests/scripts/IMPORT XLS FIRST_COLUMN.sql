-- No column range
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table1;
SELECT * FROM table1;

-- First column but no last column
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table2 OPTIONS (FIRST_COLUMN: 'B');
SELECT * FROM table2;

-- First column but no last column
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table3 OPTIONS (FIRST_COLUMN: 2);
SELECT * FROM table3;

-- Both first and last column
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table4 OPTIONS (FIRST_COLUMN: 'B', LAST_COLUMN: 'C');
SELECT * FROM table4;

-- Both first and last column
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table5 OPTIONS (FIRST_COLUMN: 2, LAST_COLUMN: 3);
SELECT * FROM table5;

-- First column is beyond the end of the data
BEGIN TRY
    IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table6 OPTIONS (FIRST_COLUMN: 100);
END TRY
BEGIN CATCH
    SELECT ERROR_MESSAGE() AS message;
END CATCH    

--output--
first,second,third,fourth
A,1,0,111
B,2,1,222
C,3,10,333
D,4,11,444
E,5,100,555
-
second,third,fourth
1,0,111
2,1,222
3,10,333
4,11,444
5,100,555
-
second,third,fourth
1,0,111
2,1,222
3,10,333
4,11,444
5,100,555
-
second,third
1,0
2,1
3,10
4,11
5,100
-
second,third
1,0
2,1
3,10
4,11
5,100
-
message
No columns chosen for import.
-
