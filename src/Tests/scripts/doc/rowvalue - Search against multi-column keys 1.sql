-- https://sqlite.org/rowvalue.html
CREATE TABLE item (ordid, prodid, qty);

SELECT ordid, prodid, qty
  FROM item
 WHERE (prodid, qty) IN (SELECT prodid, qty
                           FROM item
                          WHERE ordid = 365);

--output--
ordid,prodid,qty
-
