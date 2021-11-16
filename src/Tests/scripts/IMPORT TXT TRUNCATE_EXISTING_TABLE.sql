-- Option not specified = append
CREATE TABLE table1 (numberZZZ, lineZZZ);
INSERT INTO table1 VALUES (-999, 'FOO');
IMPORT TXT '<FILES>\utf8.txt' INTO table1;
SELECT * FROM table1;

-- 0 = Append
CREATE TABLE table2 (numberZZZ, lineZZZ);
INSERT INTO table2 VALUES (-999, 'FOO');
IMPORT TXT '<FILES>\utf8.txt' INTO table2 OPTIONS (TRUNCATE_EXISTING_TABLE: 0);
SELECT * FROM table2;

-- 1 = Truncate
CREATE TABLE table3 (numberZZZ, lineZZZ);
INSERT INTO table3 VALUES (-999, 'FOO');
IMPORT TXT '<FILES>\utf8.txt' INTO table3 OPTIONS (TRUNCATE_EXISTING_TABLE: 1);
SELECT * FROM table3;

-- 0 has no effect if the table doesn't exist.
IMPORT TXT '<FILES>\utf8.txt' INTO table4 OPTIONS (TRUNCATE_EXISTING_TABLE: 0);
SELECT * FROM table4;

-- 1 has no effect if the table doesn't exist.
IMPORT TXT '<FILES>\utf8.txt' INTO table5 OPTIONS (TRUNCATE_EXISTING_TABLE: 0);
SELECT * FROM table5;

--output--
numberZZZ,lineZZZ
-999,FOO
1,abc
2,def
3,ghi
4,jkl
5,mno
-
numberZZZ,lineZZZ
-999,FOO
1,abc
2,def
3,ghi
4,jkl
5,mno
-
numberZZZ,lineZZZ
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
number,line
1,abc
2,def
3,ghi
4,jkl
5,mno
-
