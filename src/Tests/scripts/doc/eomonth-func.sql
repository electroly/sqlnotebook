PRINT EOMONTH('2016-03-07');  -- "2016-03-31"
PRINT EOMONTH('2016-03-07', 1);  -- "2016-04-30"
PRINT EOMONTH('2016-03-07', -1);  -- "2016-02-29"
PRINT EOMONTH('2016-01-23 22:06:53.742 -04:00');  -- "2016-01-31"
--output--
2016-03-31
2016-04-30
2016-02-29
2016-01-31
