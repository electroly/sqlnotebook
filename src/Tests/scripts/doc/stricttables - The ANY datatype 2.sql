-- https://sqlite.org/stricttables.html
CREATE TABLE t1(a ANY);
INSERT INTO t1 VALUES('000123');
SELECT typeof(a), quote(a) FROM t1;
-- result: integer 123
--output--
typeof ( a ),quote ( a )
integer,123
-
