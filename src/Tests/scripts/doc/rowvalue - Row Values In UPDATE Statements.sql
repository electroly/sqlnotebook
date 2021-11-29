-- https://sqlite.org/rowvalue.html
CREATE TABLE tab3 (a, b, c, d, e);
INSERT INTO tab3 VALUES
   (1, 2, 3, 4, 5),
   (111, 222, 333, 444, 60);
CREATE TABLE tab4 (w, x, y, z);
INSERT INTO tab4 VALUES
   (444, 999, 999, 999);

UPDATE tab3 
   SET (a,b,c) = (SELECT x,y,z
                    FROM tab4
                   WHERE tab4.w=tab3.d)
 WHERE tab3.e BETWEEN 55 AND 66;

--output--
