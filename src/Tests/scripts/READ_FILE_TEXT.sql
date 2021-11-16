SELECT READ_FILE_TEXT('<FILES>\utf8.txt') AS line;

SELECT READ_FILE_TEXT('<FILES>\utf8.txt', 0) AS line;

SELECT READ_FILE_TEXT('<FILES>\utf8.txt', 65001) AS line;

SELECT READ_FILE_TEXT('<FILES>\utf16.txt') AS line;

SELECT READ_FILE_TEXT('<FILES>\utf16.txt', 0) AS line;

SELECT READ_FILE_TEXT('<FILES>\utf16.txt', 1200) AS line;

SELECT READ_FILE_TEXT('<FILES>\shiftjis.txt', 932) AS line;

--output--
line
abc
def
ghi
jkl
mno
-
line
abc
def
ghi
jkl
mno
-
line
abc
def
ghi
jkl
mno
-
line
abc
def
ghi
jkl
mno
-
line
abc
def
ghi
jkl
mno
-
line
abc
def
ghi
jkl
mno
-
line
日本語
-
