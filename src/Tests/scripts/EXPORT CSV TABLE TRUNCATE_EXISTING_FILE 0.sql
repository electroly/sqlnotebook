EXPORT TXT '<TEMP>\file.csv' FROM (SELECT 'hello');

CREATE TABLE foo (a, b, c);
INSERT INTO foo VALUES (111, 222, 333), ('AAA', 'BBB', 'CCC');
EXPORT CSV '<TEMP>\file.csv' FROM TABLE foo OPTIONS (TRUNCATE_EXISTING_FILE: 0);
PRINT '[' || READ_FILE_TEXT('<TEMP>\file.csv') || ']';

--output--
[hello
a,b,c
111,222,333
AAA,BBB,CCC
]
