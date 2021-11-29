-- https://sqlite.org/rowvalue.html
CREATE TABLE tab1 (a, b);

DECLARE @a = 1;
DECLARE @b = 2;

SELECT * FROM tab1 WHERE a=@a AND b=@b;
SELECT * FROM tab1 WHERE (a,b)=(@a,@b);

--output--
a,b
-
a,b
-
