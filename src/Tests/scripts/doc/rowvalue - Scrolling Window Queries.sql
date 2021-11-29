-- https://sqlite.org/rowvalue.html
CREATE TABLE contacts (lastname, firstname);
INSERT INTO contacts VALUES
   ('A', 'A'),
   ('A', 'Z'),
   ('Z', 'A'),
   ('Z', 'Z');

SELECT * FROM contacts
 WHERE (lastname,firstname) > ('L', 'Q')
 ORDER BY lastname, firstname
 LIMIT 7;

--output--
lastname,firstname
Z,A
Z,Z
-
