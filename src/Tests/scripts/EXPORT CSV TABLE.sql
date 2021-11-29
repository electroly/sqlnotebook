CREATE TABLE foo (a, b, c);
INSERT INTO foo VALUES (111, 222, 333), ('AAA', 'BBB', 'CCC');
EXPORT CSV '<TEMP>\file.csv' FROM TABLE foo;
PRINT '[' || READ_FILE_TEXT('<TEMP>\file.csv') || ']';

--output--
[a,b,c
111,222,333
AAA,BBB,CCC
]
