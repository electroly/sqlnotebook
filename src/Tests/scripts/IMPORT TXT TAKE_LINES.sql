-- Happy path
IMPORT TXT '<FILES>\utf8.txt' INTO table1
OPTIONS (TAKE_LINES: 2);
SELECT * FROM table1;

-- Take more lines than are present.
IMPORT TXT '<FILES>\utf8.txt' INTO table2
OPTIONS (TAKE_LINES: 100);
SELECT * FROM table2;

-- Pass -1, which takes the whole file.
IMPORT TXT '<FILES>\utf8.txt' INTO table3
OPTIONS (TAKE_LINES: -1);
SELECT * FROM table3;

--output--
number,line
1,abc
2,def
-
number,line
1,abc
2,def
3,ghi
4,jkl
5,mno
-
number,line
1,abc
2,def
3,ghi
4,jkl
5,mno
-
