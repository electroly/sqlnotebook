-- Prints "2016-07-23 22:27:29.940 -04:00".
PRINT SWITCHOFFSET('2016-07-23 22:27:29.940 +06:00', '-04:00');

-- Prints "2016-07-23 22:27:29.940 -04:00".
PRINT SWITCHOFFSET('2016-07-23 22:27:29.940 +06:00', -240);

--output--
2016-07-23 22:27:29.940 -04:00
2016-07-23 22:27:29.940 -04:00