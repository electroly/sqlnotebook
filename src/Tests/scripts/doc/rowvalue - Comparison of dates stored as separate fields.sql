-- https://sqlite.org/rowvalue.html
CREATE TABLE info(
  year INT,          -- 4 digit year
  month INT,         -- 1 through 12
  day INT,           -- 1 through 31
  other_stuff BLOB   -- blah blah blah
);

SELECT * FROM info
 WHERE (year,month,day) BETWEEN (2015,9,12) AND (2016,9,12);

--output--
year,month,day,other_stuff
-
