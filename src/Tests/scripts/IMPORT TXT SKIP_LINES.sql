-- Happy path
IMPORT TXT '<FILES>\utf8.txt' INTO table1
OPTIONS (SKIP_LINES: 3);
SELECT * FROM table1;

-- Skip past the end of the file.
IMPORT TXT '<FILES>\utf8.txt' INTO table2
OPTIONS (SKIP_LINES: 100);
SELECT * FROM table2;

--output--
number,line
1,jkl
2,mno
-
number,line
-
