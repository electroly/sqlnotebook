-- Omitted = auto-detect. Test UTF-8 without BOM.
IMPORT TXT '<FILES>\utf8.txt' INTO table1;
SELECT *, LENGTH(line) AS linelen FROM table1;

-- Omitted = auto-detect. Test UTF-8 with BOM.
IMPORT TXT '<FILES>\utf8bom.txt' INTO table2;
SELECT *, LENGTH(line) AS linelen FROM table2;

-- Omitted = auto-detect. Test UTF-16.
IMPORT TXT '<FILES>\utf16.txt' INTO table3;
SELECT *, LENGTH(line) AS linelen FROM table3;

-- 0 = auto-detect. Test UTF-8 without BOM.
IMPORT TXT '<FILES>\utf8.txt' INTO table4 OPTIONS (FILE_ENCODING: 0);
SELECT *, LENGTH(line) AS linelen FROM table4;

-- 0 = auto-detect. Test UTF-8 with BOM.
IMPORT TXT '<FILES>\utf8bom.txt' INTO table5 OPTIONS (FILE_ENCODING: 0);
SELECT *, LENGTH(line) AS linelen FROM table5;

-- 0 = auto-detect. Test UTF-16.
IMPORT TXT '<FILES>\utf16.txt' INTO table6 OPTIONS (FILE_ENCODING: 0);
SELECT *, LENGTH(line) AS linelen FROM table6;

-- 65001 = UTF-8. Test without BOM.
IMPORT TXT '<FILES>\utf8.txt' INTO table7 OPTIONS (FILE_ENCODING: 65001);
SELECT *, LENGTH(line) AS linelen FROM table7;

-- 65001 = UTF-8. Test with BOM.
IMPORT TXT '<FILES>\utf8bom.txt' INTO table8 OPTIONS (FILE_ENCODING: 65001);
SELECT *, LENGTH(line) AS linelen FROM table8;

-- 1200 = UTF-16
IMPORT TXT '<FILES>\utf16.txt' INTO table9 OPTIONS (FILE_ENCODING: 1200);
SELECT *, LENGTH(line) AS linelen FROM table9;

-- 932 = Shift-JIS
IMPORT TXT '<FILES>\shiftjis.txt' INTO table10 OPTIONS (FILE_ENCODING: 932);
SELECT *, LENGTH(line) AS linelen FROM table10;

--output--
number,line,linelen
1,abc,3
2,def,3
3,ghi,3
4,jkl,3
5,mno,3
-
number,line,linelen
1,abc,3
2,def,3
3,ghi,3
4,jkl,3
5,mno,3
-
number,line,linelen
1,abc,3
2,def,3
3,ghi,3
4,jkl,3
5,mno,3
-
number,line,linelen
1,abc,3
2,def,3
3,ghi,3
4,jkl,3
5,mno,3
-
number,line,linelen
1,abc,3
2,def,3
3,ghi,3
4,jkl,3
5,mno,3
-
number,line,linelen
1,abc,3
2,def,3
3,ghi,3
4,jkl,3
5,mno,3
-
number,line,linelen
1,abc,3
2,def,3
3,ghi,3
4,jkl,3
5,mno,3
-
number,line,linelen
1,abc,3
2,def,3
3,ghi,3
4,jkl,3
5,mno,3
-
number,line,linelen
1,abc,3
2,def,3
3,ghi,3
4,jkl,3
5,mno,3
-
number,line,linelen
1,日本語,3
-
