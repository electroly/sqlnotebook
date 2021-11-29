-- Be careful when editing this file -- we need to preserve the tab characters in the output.
CREATE TABLE foo (a, b, c);
INSERT INTO foo VALUES (111, 222, 333), ('AAA', 'BBB', 'CCC');
EXPORT CSV '<TEMP>\file.tsv' FROM TABLE foo OPTIONS (SEPARATOR: CHAR(9))
PRINT '[' || READ_FILE_TEXT('<TEMP>\file.tsv') || ']';

--output--
[a	b	c
111	222	333
AAA	BBB	CCC
]
