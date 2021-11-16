SELECT * FROM READ_FILE('<FILES>\utf8.txt');

SELECT * FROM READ_FILE('<FILES>\utf8.txt', 0);

SELECT * FROM READ_FILE('<FILES>\utf8.txt', 65001);

SELECT * FROM READ_FILE('<FILES>\shiftjis.txt', 932);

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
1,日本語
-
