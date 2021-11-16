-- Prints the numbers: 1, 2, 3, 4, 5.
FOR @i = 1 TO 5
    PRINT @i;

-- Prints the numbers: 5, 4, 3, 2, 1.
FOR @i = 5 TO 1
    PRINT @i;

-- Prints the numbers: 1, 3, 5, 9.
FOR @i = 1 TO 10 STEP 2
    PRINT @i;

--output--
1
2
3
4
5
5
4
3
2
1
1
3
5
7
9
