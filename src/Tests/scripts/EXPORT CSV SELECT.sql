EXPORT CSV '<TEMP>\file.csv' FROM (
    SELECT 111 AS a, 222 AS b, 333 AS c
    UNION ALL
    SELECT 'AAA', 'BBB', 'CCC'
);
PRINT '[' || READ_FILE_TEXT('<TEMP>\file.csv') || ']';
--output--
[a,b,c
111,222,333
AAA,BBB,CCC
]
