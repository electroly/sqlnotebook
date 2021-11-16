PRINT SPLIT('AAA|BBB|CCC', '|', 1);  -- "BBB"
PRINT SPLIT('AAA|BBB|CCC', '|', 5);  -- NULL
PRINT SPLIT('AAA|BBB|CCC', '|');  -- "[AAA, BBB, CCC]"

--output--
BBB

[AAA, BBB, CCC]
