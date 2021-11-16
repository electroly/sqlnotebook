-- Returns a table listing all the worksheets.
SELECT * FROM LIST_XLS_WORKSHEETS('<FILES>\excel.xls');

-- Returns the name of the second worksheet, or an
-- empty table if there are fewer than 2 worksheets
-- in the workbook.
SELECT name FROM LIST_XLS_WORKSHEETS('<FILES>\excel.xls') WHERE number = 2;

--output--
number,name
1,FirstSheet
2,SecondSheet
3,ThirdSheet
-
name
SecondSheet
-
