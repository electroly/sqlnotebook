-- By default, permanent table.
IMPORT TXT '<FILES>\utf8.txt' INTO table1;
SELECT * FROM table1;
SELECT COUNT(*) AS count FROM sqlite_master WHERE name = 'table1'; -- present

-- 0 = permanent table
IMPORT TXT '<FILES>\utf8.txt' INTO table2 OPTIONS (TEMPORARY_TABLE: 0);
SELECT * FROM table2;
SELECT COUNT(*) AS count FROM sqlite_master WHERE name = 'table2'; -- present

-- 1 = temporary table
IMPORT TXT '<FILES>\utf8.txt' INTO table3 OPTIONS (TEMPORARY_TABLE: 1);
SELECT * FROM table3;
SELECT COUNT(*) AS count FROM sqlite_master WHERE name = 'table3'; -- not present

--output--
number,line
1,abc
2,def
3,ghi
4,jkl
5,mno
-
count
1
-
number,line
1,abc
2,def
3,ghi
4,jkl
5,mno
-
count
1
-
number,line
1,abc
2,def
3,ghi
4,jkl
5,mno
-
count
0
-
