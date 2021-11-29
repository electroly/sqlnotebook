-- https://sqlite.org/rowvalue.html
CREATE TABLE tab1 (a, b);

UPDATE tab1 SET (a,b)=(b,a);
UPDATE tab1 SET a=b, b=a;

--output--
