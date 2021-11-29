-- https://sqlite.org/lang_upsert.html
CREATE TABLE t1 (x PRIMARY KEY, y);
CREATE TABLE t2 (x PRIMARY KEY, y);

INSERT INTO t1 SELECT * FROM t2 WHERE true
ON CONFLICT(x) DO UPDATE SET y=excluded.y;

--output--
