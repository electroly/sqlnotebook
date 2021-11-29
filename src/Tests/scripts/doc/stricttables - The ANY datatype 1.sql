-- https://sqlite.org/stricttables.html
CREATE TABLE t1(a ANY) STRICT;
INSERT INTO t1 VALUES('000123');
SELECT typeof(a), quote(a) FROM t1;
-- result: text '000123'
--output--
typeof ( a ),quote ( a )
text,'000123'
-
