-- Auto-detected encoding
SELECT * FROM READ_CSV('<FILES>\utf8.csv');
SELECT * FROM READ_CSV('<FILES>\utf16.csv');

-- has-header-row = 1
SELECT * FROM READ_CSV('<FILES>\utf8.csv', 1);

-- has-header-row = 0
SELECT * FROM READ_CSV('<FILES>\utf8.csv', 0);

-- skip-lines = 1
SELECT * FROM READ_CSV('<FILES>\utf8.csv', 1, 1);

-- file-encoding = 932 (Shift-JIS)
SELECT * FROM READ_CSV('<FILES>\shiftjis.csv', 1, 0, 932);

-- file encoding = 65001 (UTF-8)
SELECT * FROM READ_CSV('<FILES>\utf8.csv', 1, 0, 65001);

-- file encoding = 1200 (UTF-16)
SELECT * FROM READ_CSV('<FILES>\utf16.csv', 1, 0, 1200);
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
column1,column2
foo,bar
111,HELLO
222,WORLD
-
111,HELLO
222,WORLD
-
日,本,語
いつ神戸に来るのか教えて下さい,ＡＢＣ社のガードナー氏は,２月２０日から２７日までマリオットホテルに滞在中で
ぜひあなたに会いたいとのことです,彼は、詩人ではなくて小説家だ,３人のうちの１人が芝刈り機で私の庭を大雑把にさっと刈り
-
foo,bar
111,HELLO
222,WORLD
-
foo,bar
111,HELLO
222,WORLD
-
