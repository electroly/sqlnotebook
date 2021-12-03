-- The following examples assume a US English computer.
-- In other locales, DATENAME() will return locale-specific names.
PRINT DATENAME('month', '2016-07-23');  -- "July"
PRINT DATENAME('weekday', '2016-07-23');  -- "Saturday"
PRINT DATENAME('dd', '2016-07-23');  -- "23"
--output--
July
Saturday
23
