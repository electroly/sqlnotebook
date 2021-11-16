-- Prints "1" because the clock crosses midnight once.
PRINT DATEDIFF('day', '2016-03-04 03:53', '2016-03-05 11:53');

-- Prints "1" because the clock crosses the top of the hour once.
PRINT DATEDIFF('hh', '2016-01-01 03:59', '2016-01-01 04:01');  -- "1"

-- Prints "0" because the clock does not cross the top of the hour.
PRINT DATEDIFF('hh', '2016-01-01 03:50', '2016-01-01 03:52');

-- Prints "2". Take note of the time zone offsets.
PRINT DATEDIFF('hh', '2016-07-23 21:25 -04:00', '2016-07-23 21:25 -06:00');

--output--
1
1
0
2
