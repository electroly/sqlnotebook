CREATE TABLE foo (a, b);
INSERT INTO foo VALUES ('hello', 'world');

EXPORT TXT '<TEMP>\file.txt'
FROM (SELECT * FROM foo);

DELETE FROM foo;
INSERT INTO foo VALUES ('aaa', 'bbb');

EXPORT TXT '<TEMP>\file.txt'
FROM (SELECT * FROM foo)
OPTIONS (TRUNCATE_EXISTING_FILE: 1);

IMPORT TXT '<TEMP>\file.txt' INTO foo2 (num, line);

SELECT * FROM foo2;
--output--
num,line
1,aaabbb
-
