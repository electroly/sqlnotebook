-- https://sqlite.org/rowvalue.html
CREATE TABLE item (ordid, prodid, qty);

SELECT t1.ordid, t1.prodid, t1.qty
  FROM item AS t1, item AS t2
 WHERE t1.prodid=t2.prodid
   AND t1.qty=t2.qty
   AND t2.ordid=365;

--output--
ordid,prodid,qty
-
