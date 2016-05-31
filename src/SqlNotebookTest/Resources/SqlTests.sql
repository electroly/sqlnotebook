-- SQL Notebook
-- Copyright (C) 2016 Brian Luft
--
-- Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
-- documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
-- rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
-- permit persons to whom the Software is furnished to do so, subject to the following conditions:
--
-- The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
-- Software.
--
-- THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
-- WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
-- OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
-- OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

CREATE TABLE tbl1 (a,b,c)
CREATE TABLE other.tbl1 (a, b, c)
SELECT 1
SELECT 1 FROM sqlite_master
SELECT 1 FROM sqlite_master CROSS JOIN sqlite_master
SELECT a.*, b.* FROM sqlite_master a CROSS JOIN sqlite_master b
SELECT a.*, b.* FROM sqlite_master AS a CROSS JOIN sqlite_master AS b
SELECT * FROM other.sqlite_master

-- alter-table-stmt
ALTER TABLE other.tbl1 RENAME TO tbl2
ALTER TABLE other.tbl2 RENAME TO tbl1
ALTER TABLE tbl1 RENAME TO tbl2
ALTER TABLE tbl2 RENAME TO tbl1

ALTER TABLE tbl1 ADD COLUMN aaa TEXT NULL
ALTER TABLE other.tbl1 ADD COLUMN aaa TEXT NULL

-- analyze-stmt
ANALYZE
ANALYZE other
ANALYZE tbl1
ANALYZE other.tbl1

-- attach-stmt
ATTACH DATABASE @other2_path AS other2
DETACH DATABASE other2
ATTACH @other2_path AS other2
DETACH other2

-- begin-stmt
BEGIN
ROLLBACK
BEGIN DEFERRED TRANSACTION
ROLLBACK
BEGIN IMMEDIATE TRANSACTION
ROLLBACK
BEGIN EXCLUSIVE TRANSACTION
ROLLBACK
BEGIN DEFERRED
ROLLBACK
BEGIN IMMEDIATE
ROLLBACK
BEGIN EXCLUSIVE
ROLLBACK

-- commit-stmt
BEGIN
END
BEGIN
END TRANSACTION
BEGIN
COMMIT
BEGIN
COMMIT TRANSACTION

-- rollback-stmt, savepoint-stmt
BEGIN
ROLLBACK
BEGIN
ROLLBACK TRANSACTION
SAVEPOINT test_savepoint
ROLLBACK TO test_savepoint
SAVEPOINT test_savepoint
ROLLBACK TO SAVEPOINT test_savepoint
SAVEPOINT test_savepoint
ROLLBACK TRANSACTION TO test_savepoint
SAVEPOINT test_savepoint
ROLLBACK TRANSACTION TO SAVEPOINT test_savepoint

-- release-stmt
SAVEPOINT test_savepoint
RELEASE test_savepoint
SAVEPOINT test_savepoint
RELEASE SAVEPOINT test_savepoint

-- create-index-stmt
CREATE TABLE tbl_index_1 (a NOT NULL, b)
CREATE TABLE other.tbl_index_1 (a NOT NULL, b)
CREATE INDEX _7c75b662 ON tbl_index_1 (a, b)
CREATE UNIQUE INDEX _98424d29 ON tbl_index_1 (a, b)
CREATE INDEX IF NOT EXISTS _416825cc ON tbl_index_1 (a, b)
CREATE INDEX other._20e93b5c ON tbl_index_1 (a, b)
CREATE INDEX _c5f18e11 ON tbl_index_1 (a, b) WHERE a = 5

-- indexed-column
CREATE TABLE tbl_idxcol (a, b)
CREATE INDEX _eda0dfb2 ON tbl_idxcol (a)
CREATE INDEX _cd219ffc ON tbl_idxcol (a + 5)
CREATE INDEX _358f44ad ON tbl_idxcol (a COLLATE BINARY)
CREATE INDEX _24854ab5 ON tbl_idxcol (a COLLATE NOCASE)
CREATE INDEX _e3c7d874 ON tbl_idxcol (a COLLATE RTRIM)
CREATE INDEX _da441b98 ON tbl_idxcol (a + 5 COLLATE BINARY)
CREATE INDEX _4f580735 ON tbl_idxcol (a ASC)
CREATE INDEX _3ebe7058 ON tbl_idxcol (a DESC)

-- create-table-stmt
<ERR> CREATE TABLE _aa82a134 ()
CREATE TABLE _0adb0e7c (a, b)
CREATE TEMP TABLE _18ef7bda (a, b)
CREATE TEMPORARY TABLE _bd838980 (a, b)
CREATE TABLE IF NOT EXISTS _4e7bbe86 (a, b)
CREATE TABLE other._dd572356 (a, b)
CREATE TABLE _8e6f8125 (a, b, PRIMARY KEY (a))
CREATE TABLE _5369d972 (a, b, PRIMARY KEY (a), UNIQUE (b))
CREATE TABLE _5fde254f (a TEXT NULL)
CREATE TABLE _04f138d5 (a PRIMARY KEY, b) WITHOUT ROWID
CREATE TABLE _6eedb00a AS SELECT * FROM sqlite_master

-- column-def, type-name
CREATE TABLE _68642c9c (a VARCHAR(100))
CREATE TABLE _5fd33d4c (a BLAH VARCHAR(100))
CREATE TABLE _a72057b5 (a BLAH DECIMAL(19, 2))
CREATE TABLE _aca99180 (a BLAH DECIMAL(19, 2))
<ERR> CREATE TABLE _a865023c (a BLAH DECIMAL(@int, 2))
<ERR> CREATE TABLE _8e4216c9 (a BLAH DECIMAL(3, @int))
CREATE TABLE _6fb46a08 (a MEANINGLESS STRING OF IDENTIFIER TOKENS)
CREATE TABLE _7d178f5e (a MEANINGLESS STRING IDENTIFIER TOKENS)
<ERR> CREATE TABLE _f88e618f (a CANNOT_HAVE_AN_ACTUAL_KEYWORD_LIKE CREATE)
CREATE TABLE _2697471e (a NOT NULL DEFAULT 123)

CREATE TABLE _00d0def8 (a Explain)
CREATE TABLE _df75011a (a Query)
CREATE TABLE _5e0c0329 (a Plan)
CREATE TABLE _ade21710 (a Begin)
CREATE TABLE _8e70aba9 (a Deferred)
CREATE TABLE _e2f5522b (a Immediate)
CREATE TABLE _7c747357 (a Exclusive)
CREATE TABLE _4f5ed451 (a End)
CREATE TABLE _c08cefb4 (a Rollback)
CREATE TABLE _96bebee3 (a Savepoint)
CREATE TABLE _ed46a40b (a Release)
CREATE TABLE _faaade88 (a If)
CREATE TABLE _58eac441 (a Temp)
CREATE TABLE _524fb0b0 (a Without)
CREATE TABLE _36e9d2ac (a Abort)
CREATE TABLE _2f47e0b1 (a Action)
CREATE TABLE _38c850b5 (a After)
CREATE TABLE _25dfde2e (a Analyze)
CREATE TABLE _6a0505a8 (a Asc)
CREATE TABLE _8cf5bc02 (a Attach)
CREATE TABLE _c50ad9e2 (a Before)
CREATE TABLE _444955bb (a By)
CREATE TABLE _b6d45c6f (a Cascade)
CREATE TABLE _36e6cf14 (a Cast)
CREATE TABLE _ed9756bf (a Column)
CREATE TABLE _d1519e94 (a Conflict)
CREATE TABLE _f93f075f (a Database)
CREATE TABLE _f825269f (a Desc)
CREATE TABLE _55d994f5 (a Detach)
CREATE TABLE _c62c4d07 (a Each)
CREATE TABLE _a5478fc1 (a Fail)
CREATE TABLE _a0da0fc4 (a For)
CREATE TABLE _042babb1 (a Ignore)
CREATE TABLE _66490c3c (a Initially)
CREATE TABLE _915e833a (a Instead)
CREATE TABLE _b98c9e9f (a Like)
CREATE TABLE _feafdb2c (a Match)
CREATE TABLE _30941de1 (a No)
CREATE TABLE _74e582d0 (a Key)
CREATE TABLE _e7e8af3a (a Of)
CREATE TABLE _d24aeab1 (a Offset)
CREATE TABLE _94d9d671 (a Pragma)
CREATE TABLE _7d9ee897 (a Raise)
CREATE TABLE _fa50e1d3 (a Recursive)
CREATE TABLE _5f5bb97e (a Replace)
CREATE TABLE _bb23de06 (a Restrict)
CREATE TABLE _51d896f7 (a Row)
CREATE TABLE _b2032ac7 (a Trigger)
CREATE TABLE _8e9e2f93 (a Vacuum)
CREATE TABLE _6f96f051 (a View)
CREATE TABLE _d8db5e75 (a Virtual)
CREATE TABLE _7b49976a (a With)
CREATE TABLE _8fe46f59 (a Reindex)
CREATE TABLE _860465e2 (a Rename)
CREATE TABLE _668dda0c (a Ctime)
CREATE TABLE _06f48f84 (a Any)
CREATE TABLE _0ebb8591 (a Rem)
CREATE TABLE _2505921a (a Concat)
CREATE TABLE _d248269a (a Null)
CREATE TABLE _e348e1b5 (a Unique)
CREATE TABLE _474347f5 (a Autoincr)
CREATE TABLE _3d935c94 (a Deferrable)
CREATE TABLE _39e336db (a ToText)
CREATE TABLE _70976d78 (a ToBlob)
CREATE TABLE _c0639578 (a ToNumeric)
CREATE TABLE _848c26ee (a ToInt)
CREATE TABLE _55678662 (a ToReal)
CREATE TABLE _d4256c93 (a IsNot)
CREATE TABLE _1b52c458 (a Function)
CREATE TABLE _54273ea3 (a Sum)
CREATE TABLE _a6fde2d0 (a Register)

<ERR> CREATE TABLE _6910e59c (a Transaction)
<ERR> CREATE TABLE _191ba180 (a Commit)
<ERR> CREATE TABLE _74e6d92c (a To)
<ERR> CREATE TABLE _44822e85 (a Table)
<ERR> CREATE TABLE _241993ed (a Create)
<ERR> CREATE TABLE _8be3c486 (a Not)
<ERR> CREATE TABLE _a552398d (a Exists)
<ERR> CREATE TABLE _1d45659f (a As)
<ERR> CREATE TABLE _111d3c54 (a Indexed)
<ERR> CREATE TABLE _5c0a0e7c (a Or)
<ERR> CREATE TABLE _479d8b0e (a And)
<ERR> CREATE TABLE _d3977cef (a Is)
<ERR> CREATE TABLE _f442c6cf (a Between)
<ERR> CREATE TABLE _55768541 (a In)
<ERR> CREATE TABLE _c6a3d114 (a IsNull)
<ERR> CREATE TABLE _48515b1a (a NotNull)
<ERR> CREATE TABLE _a20fc33f (a Escape)
<ERR> CREATE TABLE _bcce1fb3 (a Collate)
<ERR> CREATE TABLE _11ff52a0 (a Join)
<ERR> CREATE TABLE _d4249d04 (a Constraint)
<ERR> CREATE TABLE _360a68f2 (a Default)
<ERR> CREATE TABLE _bf9aeb8d (a Primary)
<ERR> CREATE TABLE _88d459b5 (a Check)
<ERR> CREATE TABLE _58829ddc (a References)
<ERR> CREATE TABLE _44ff21a7 (a On)
<ERR> CREATE TABLE _ca00bc89 (a Insert)
<ERR> CREATE TABLE _8a541cb1 (a Delete)
<ERR> CREATE TABLE _bd830ffb (a Update)
<ERR> CREATE TABLE _cc0bd027 (a Set)
<ERR> CREATE TABLE _d1251241 (a Foreign)
<ERR> CREATE TABLE _6611a30a (a Drop)
<ERR> CREATE TABLE _a86baff1 (a Union)
<ERR> CREATE TABLE _08072dbf (a All)
<ERR> CREATE TABLE _cc6c5e62 (a Except)
<ERR> CREATE TABLE _f652278c (a Intersect)
<ERR> CREATE TABLE _84e121ed (a Select)
<ERR> CREATE TABLE _85d18bd9 (a Values)
<ERR> CREATE TABLE _ce662710 (a Distinct)
<ERR> CREATE TABLE _fc725c55 (a From)
<ERR> CREATE TABLE _f9658ffb (a Join)
<ERR> CREATE TABLE _9cb179ed (a Using)
<ERR> CREATE TABLE _4a2c727c (a Order)
<ERR> CREATE TABLE _3cfcfb9d (a Group)
<ERR> CREATE TABLE _054df1c3 (a Having)
<ERR> CREATE TABLE _aa6a9ec9 (a Limit)
<ERR> CREATE TABLE _388c4afa (a Where)
<ERR> CREATE TABLE _07907027 (a Into)
<ERR> CREATE TABLE _321261cc (a Case)
<ERR> CREATE TABLE _860c6699 (a When)
<ERR> CREATE TABLE _7671a0d9 (a Then)
<ERR> CREATE TABLE _26e48bd5 (a Else)
<ERR> CREATE TABLE _7f57e303 (a Index)
<ERR> CREATE TABLE _43ec5fd8 (a Alter)
<ERR> CREATE TABLE _740e92e6 (a Add)

-- column-constraint
CREATE TABLE _634ee2e6 (a CONSTRAINT _d2c5954a PRIMARY KEY)
CREATE TABLE _826d0879 (a PRIMARY KEY)
CREATE TABLE _19a3c5bb (a PRIMARY KEY ASC)
CREATE TABLE _27c6b9cc (a PRIMARY KEY DESC)
CREATE TABLE _50903fb3 (a PRIMARY KEY ON CONFLICT ROLLBACK)
CREATE TABLE _c04fb5f2 (a INTEGER PRIMARY KEY AUTOINCREMENT)
CREATE TABLE _ead52817 (a INTEGER PRIMARY KEY ON CONFLICT ROLLBACK AUTOINCREMENT)
CREATE TABLE _9bb70ccb (a NOT NULL)
CREATE TABLE _44fcd747 (a NOT NULL ON CONFLICT ROLLBACK)
CREATE TABLE _3bfa996d (a UNIQUE)
CREATE TABLE _a361f7b1 (a UNIQUE ON CONFLICT ROLLBACK)
CREATE TABLE _728e41fd (a CHECK (a > 5))
CREATE TABLE _eab38f88 (a DEFAULT -5)
CREATE TABLE _0b24ff4c (a DEFAULT - 5)
CREATE TABLE _1214f27c (a DEFAULT 'hello')
CREATE TABLE _48fb7c04 (a DEFAULT 5.55)
CREATE TABLE _88481f6e (a DEFAULT (5 + 6))
CREATE TABLE _8ced2b0b (a COLLATE BINARY)
CREATE TABLE colconstr_tbl (blah)
CREATE TABLE _bc6d1f5e (a REFERENCES colconstr_tbl (blah))

-- signed-number
CREATE TABLE _687a0c7f (a DEFAULT + 5)
CREATE TABLE _c5220662 (a DEFAULT - 5)
CREATE TABLE _2a1ce3cf (a DEFAULT +5)
CREATE TABLE _4b7cc691 (a DEFAULT -5)
CREATE TABLE _04cfe310 (a DEFAULT + 5.123)
CREATE TABLE _9ad7c68f (a DEFAULT - 5.123)
CREATE TABLE _42fe9df5 (a DEFAULT +5.123)
CREATE TABLE _c38ee725 (a DEFAULT -5.123)
CREATE TABLE _91076960 (a DECIMAL( + 5))
CREATE TABLE _c88a0068 (a DECIMAL( - 5))
CREATE TABLE _b3c65440 (a DECIMAL( +5))
CREATE TABLE _d80dd992 (a DECIMAL( -5))
CREATE TABLE _f0aa1d3f (a DECIMAL( + 5.123))
CREATE TABLE _f98bbe60 (a DECIMAL( - 5.123))
CREATE TABLE _45c9ebeb (a DECIMAL( +5.123))
CREATE TABLE _5207b870 (a DECIMAL( -5.123))

-- table-constraint
<ERR> CREATE TABLE _7b480424 (a, b, PRIMARY KEY ())
CREATE TABLE _1f753021 (a, b, PRIMARY KEY (a))
CREATE TABLE _e4381250 (a, b, PRIMARY KEY (a, b))
CREATE TABLE _2919b89a (a, b, CONSTRAINT _93092f73 PRIMARY KEY (a))
CREATE TABLE _d385ee34 (a, b, PRIMARY KEY (a, b) ON CONFLICT ROLLBACK)
CREATE TABLE _3eab040e (a, b, UNIQUE (a))
CREATE TABLE _3769d5d4 (a, b, CHECK (a > 5))
CREATE TABLE tblconstr_tbl (fa, fb)
CREATE TABLE _69547c2c (a, b, FOREIGN KEY (a, b) REFERENCES tblconstr_tbl (fa, fb))
CREATE TABLE _1e29b0db (a, b, FOREIGN KEY (a) REFERENCES tblconstr_tbl (fa))
<ERR> CREATE TABLE _3d04f111 (a, b, FOREIGN KEY () REFERENCES tblconstr_tbl (fa))

-- foreign-key-clause
CREATE TABLE fkc_tbl (a, b)
CREATE TABLE _1151b396 (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b))
CREATE TABLE _7d568b41 (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) ON DELETE SET NULL)
CREATE TABLE _75b54c57 (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) ON DELETE SET DEFAULT)
CREATE TABLE _803326cb (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) ON DELETE CASCADE)
CREATE TABLE _b804cc65 (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) ON DELETE RESTRICT)
CREATE TABLE _fdbbfcba (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) ON DELETE NO ACTION)
CREATE TABLE _9062a9e6 (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) ON UPDATE SET NULL)
CREATE TABLE _c6a2dd9c (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) ON UPDATE SET DEFAULT)
CREATE TABLE _62397ddd (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) ON UPDATE CASCADE)
CREATE TABLE _94272e3b (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) ON UPDATE RESTRICT)
CREATE TABLE _519b0036 (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) ON UPDATE NO ACTION)
CREATE TABLE _995c0bc9 (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) MATCH blah)
CREATE TABLE _db8ab709 (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) ON DELETE CASCADE ON UPDATE CASCADE)
CREATE TABLE _963b65e9 (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) ON UPDATE CASCADE ON DELETE CASCADE)
CREATE TABLE _035ca273 (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) ON UPDATE CASCADE ON DELETE CASCADE MATCH blah)
CREATE TABLE _f12f1170 (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) DEFERRABLE)
CREATE TABLE _8f824adc (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) NOT DEFERRABLE)
CREATE TABLE _5e74d316 (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) DEFERRABLE INITIALLY DEFERRED)
CREATE TABLE _534ca27e (a, b, FOREIGN KEY (a, b) REFERENCES fkc_tbl (a, b) DEFERRABLE INITIALLY IMMEDIATE)

-- SQLite test suite - randexpr1.test
CREATE TABLE t1(a,b,c,d,e,f)
INSERT INTO t1 VALUES(100,200,300,400,500,600)
SELECT coalesce((select 11 from t1 where 19 in (t1.b,+11,coalesce((select max((abs(17)/abs(t1.f))) from t1 where ((abs(t1.f)/abs(t1.b)) in (select case (min(t1.a | d*d)+(abs(count(*)-count(*)+ -count(*)*max( -t1.c))-max(f))) when  -count(distinct 19) then ((count(*))) else max(13) end from t1 union select count(distinct b) from t1)) or 19 in (t1.a,t1.c,17)),17) | 17)),13) FROM t1 WHERE not not c=a-+(select case ~case  -~+count(distinct (select count(distinct t1.a)*max(13) from t1))+max( -19*f)*max(f)*max(f)* -count(distinct d)-(count(distinct 11)) | max(t1.f)*count(*) when count(distinct b) then count(distinct t1.b) else  -min(t1.f) end*cast(avg(11) AS integer) when max(t1.f) then max(c) else count(*) end from t1)+d
SELECT f*f*(abs(case when (17-coalesce((select max(11) from t1 where coalesce((select max(t1.c-11*t1.a) from t1 where +e=(abs(d)/abs(11))+ -13),t1.b)<b and exists(select 1 from t1 where t1.f between d and  -t1.c)),19)-19 in (select max(t1.e) | min(t1.e) from t1 union select count(*) from t1) and 17>t1.e) then e else 17 end)/abs(c)) FROM t1 WHERE (t1.a in ((t1.f-t1.c)+b,11,t1.a+f))
SELECT ~case when 19 not between ~case when coalesce((select max(t1.c*(select cast(avg(13) AS integer) from t1)) from t1 where exists(select 1 from t1 where case when (t1.b in (d,b,t1.c)) and 13>t1.d then e+t1.c when 19 between t1.f and t1.f then 11 else e end between t1.e and 17) and c=t1.f),t1.c) not between t1.a and (19) then c else t1.b end*a-19 and b then b when a not in (a,t1.f,11) then a else a end FROM t1 WHERE NOT ((select abs((+min(t1.c)*min(t1.d)*count(*)))*count(*)++count(distinct ~13*11+t1.f) | case abs(+count(*)) when min(t1.e) then count(*) else max(13) end from t1)*t1.b between d and a++19 and t1.c in (select ~((abs(17)/abs(c)))-d from t1 union select t1.f from t1))
SELECT coalesce((select coalesce((select t1.e from t1 where case when c+a in (select t1.f from t1 union select 13 from t1) then c when not not exists(select 1 from t1 where d>= -11) then t1.d else  -t1.f end between b and 11 or d not between e and 11), -f)+e+ -11*17 from t1 where not exists(select 1 from t1 where exists(select 1 from t1 where t1.e in (select ~min(e) from t1 union select case case count(distinct f) & min(19)+min(a) when count(*) then max( -t1.c) else max(17) end when (max(t1.c)) then min(11) else min(t1.c) end from t1)) or d=a)),t1.e) FROM t1 WHERE NOT (+t1.e<~case when t1.e in (select (abs(c)/abs(~t1.d)) from t1 union select a from t1) then coalesce((select e from t1 where t1.d+ -coalesce((select max(11-t1.e*b) from t1 where 19 in (11,t1.b,e)),11)*t1.e+19 in (select t1.d from t1 union select  -t1.d from t1)),17) | t1.e+e when t1.f in (17,f,f) then t1.b else 17 end*13)
DROP TABLE t1

-- SQLite test suite - where.test
CREATE TABLE t1(w int, x int, y int)
CREATE TABLE t2(p int, q int, r int, s int)
INSERT INTO t2 SELECT 101-w, x, (SELECT max(y) FROM t1)+1-y, y FROM t1
CREATE INDEX i1w ON t1("w")
CREATE INDEX i1xy ON t1(`x`,'y' ASC)
CREATE INDEX i2p ON t2(p)
CREATE INDEX i2r ON t2(r)
CREATE INDEX i2qs ON t2(q, s)
SELECT x, y, w FROM t1 WHERE w=10
SELECT x, y, w FROM t1 WHERE w IS 10
SELECT x, y, w AS abc FROM t1 WHERE 11 IS abc
SELECT w, x, y FROM t1 WHERE 11 IS w AND x>2
SELECT x, y FROM t1 WHERE x IS 3 AND y IS 100 AND w<10
SELECT A.w, B.p, C.w FROM t1 as A, t2 as B, t1 as C WHERE A.w=15 AND B.p=C.w AND B.r=10202-A.y
SELECT * FROM t1 WHERE rowid IN (1,2,3,1234) order by 1
SELECT * FROM t1 WHERE rowid+0 IN (1,2,3,1234) order by 1
SELECT * FROM t1 WHERE rowid+0 IN (select rowid from t1 where rowid IN (-1,2,4)) ORDER BY 1
SELECT * FROM t1 WHERE x IN (1,7) AND y NOT IN (6400,8100) ORDER BY 1
SELECT w, x, y FROM t1 WHERE x IN (1,5) AND y IN (9,8,3025,1000,3969) ORDER BY x, y
CREATE TABLE t3(a,b,c)
CREATE INDEX t3a ON t3(a)
CREATE INDEX t3bc ON t3(b,c)
CREATE INDEX t3acb ON t3(a,c,b)
INSERT INTO t3 SELECT w, 101-w, y FROM t1
SELECT * FROM t3 ORDER BY a LIMIT 3
SELECT * FROM t3 ORDER BY a+1 LIMIT 3
SELECT * FROM t3 WHERE a>0 AND a<10 ORDER BY a LIMIT 3
SELECT * FROM t3 WHERE a IN (3,5,7,1,9,4,2) ORDER BY a DESC LIMIT 3
DROP TABLE t1
DROP TABLE t2

-- SQLite test suite - subquery.test
CREATE TABLE t1(a,b)
CREATE TABLE t2(x,y)
SELECT a, (SELECT y FROM t2 WHERE x=a) FROM t1 WHERE b<8
UPDATE t1 SET b=b+(SELECT y FROM t2 WHERE x=a)
SELECT b FROM t1 WHERE EXISTS(SELECT * FROM t2 WHERE y=a)
SELECT b FROM t1 WHERE NOT EXISTS(SELECT * FROM t2 WHERE y=a)
SELECT a, x FROM t1, t2 WHERE t1.a = (SELECT x)
SELECT count(*) FROM t1 WHERE a > (SELECT count(*) FROM t2)
SELECT a FROM t1 WHERE (SELECT (y*2)>b FROM t2 WHERE a=x)
SELECT * FROM (SELECT (SELECT sum(a) FROM t1))
CREATE TABLE t5 (val int, period text PRIMARY KEY)
SELECT period, vsum FROM (SELECT a.period, (select sum(val) from t5 where period between a.period and '2002-4') vsum FROM t5 a where a.period between '2002-1' and '2002-4') WHERE vsum < 45
DROP TABLE t1
DROP TABLE t2
DROP TABLE t5
