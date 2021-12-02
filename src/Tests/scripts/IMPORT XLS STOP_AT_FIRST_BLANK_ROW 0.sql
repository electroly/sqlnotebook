IMPORT XLS '<FILES>\excel-blank-row.xlsx' INTO 'excel' OPTIONS (STOP_AT_FIRST_BLANK_ROW: 0);
SELECT * FROM excel;
--output--
a,b
111,222
333,444
null,null
555,666
-
