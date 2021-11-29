-- https://sqlite.org/lang_update.html#upfrom
CREATE TABLE inventory (itemId, quantity);
CREATE TABLE sales (itemId, quantity);

UPDATE inventory
   SET quantity = quantity - daily.amt
  FROM (SELECT sum(quantity) AS amt, itemId FROM sales GROUP BY 2) AS daily
 WHERE inventory.itemId = daily.itemId;

--output--
