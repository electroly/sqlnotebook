-- Omitted = auto-detect. Test UTF-8 without BOM.
IMPORT CSV '<FILES>\utf8.csv' INTO table1;
SELECT * FROM table1;

-- Omitted = auto-detect. Test UTF-8 with BOM.
IMPORT CSV '<FILES>\utf8bom.csv' INTO table2;
SELECT * FROM table2;

-- Omitted = auto-detect. Test UTF-16.
IMPORT CSV '<FILES>\utf16.csv' INTO table3;
SELECT * FROM table3;

-- 0 = auto-detect. Test UTF-8 without BOM.
IMPORT CSV '<FILES>\utf8.csv' INTO table4 OPTIONS (FILE_ENCODING: 0);
SELECT * FROM table4;

-- 0 = auto-detect. Test UTF-8 with BOM.
IMPORT CSV '<FILES>\utf8bom.csv' INTO table5 OPTIONS (FILE_ENCODING: 0);
SELECT * FROM table5;

-- 0 = auto-detect. Test UTF-16.
IMPORT CSV '<FILES>\utf16.csv' INTO table6 OPTIONS (FILE_ENCODING: 0);
SELECT * FROM table6;

-- 65001 = UTF-8. Test without BOM.
IMPORT CSV '<FILES>\utf8.csv' INTO table7 OPTIONS (FILE_ENCODING: 65001);
SELECT * FROM table7;

-- 65001 = UTF-8. Test with BOM.
IMPORT CSV '<FILES>\utf8bom.csv' INTO table8 OPTIONS (FILE_ENCODING: 65001);
SELECT * FROM table8;

-- 1200 = UTF-16
IMPORT CSV '<FILES>\utf16.csv' INTO table9 OPTIONS (FILE_ENCODING: 1200);
SELECT * FROM table9;

-- 932 = Shift-JIS
IMPORT CSV '<FILES>\shiftjis.csv' INTO table10 OPTIONS (FILE_ENCODING: 932);
SELECT * FROM table10;

--output--
foo,bar
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
222,WORLD
-
日,本,語
いつ神戸に来るのか教えて下さい,ＡＢＣ社のガードナー氏は,２月２０日から２７日までマリオットホテルに滞在中で
ぜひあなたに会いたいとのことです,彼は、詩人ではなくて小説家だ,３人のうちの１人が芝刈り機で私の庭を大雑把にさっと刈り
-
