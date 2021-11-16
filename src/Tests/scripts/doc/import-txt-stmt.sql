IMPORT TXT '<FILES>\utf8.txt' INTO table1;
SELECT * FROM table1;

-- Read the first line only.
IMPORT TXT '<FILES>\utf8.txt' INTO table2
OPTIONS (TAKE_LINES: 1);
SELECT * FROM table2;

-- Read the fourth and fifth lines.
IMPORT TXT '<FILES>\utf8.txt' INTO table3
OPTIONS (SKIP_LINES: 3, TAKE_LINES: 2);
SELECT * FROM table3;

-- Read a file in Shift-JIS encoding.
IMPORT TXT '<FILES>\shiftjis.txt' INTO table4
OPTIONS (FILE_ENCODING: 932);
SELECT * FROM table4;

--output--
number,line
1,abc
2,def
3,ghi
4,jkl
5,mno
-
number,line
1,abc
-
number,line
1,jkl
2,mno
-
number,line
1,日本語
-
