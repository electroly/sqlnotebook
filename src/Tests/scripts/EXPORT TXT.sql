CREATE TABLE foo (a, b);
INSERT INTO foo VALUES ('hello', 'world'), ('aaa', 'bbb');

EXPORT TXT '<TEMP>\file.txt'
FROM (SELECT * FROM foo);

IMPORT TXT '<TEMP>\file.txt' INTO foo2 (num, line);

SELECT * FROM foo2;
--output--
num,line
1,helloworld
2,aaabbb
-
