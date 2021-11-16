-- The following examples assume a US English computer.
-- In other locales, DATENAME() will return locale-specific names.
PRINT DATENAME('month', '2016-07-23');  -- "July"
PRINT DATENAME('weekday', '2016-07-23');  -- "Saturday"
PRINT DATENAME('dd', '2016-07-23');  -- "23"
PRINT DATENAME('tzoffset', '2016-07-23 12:00 -04:00');  -- "-04:00"
--output--
July
Saturday
23
-04:00
