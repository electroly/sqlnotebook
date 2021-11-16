-- Prints the odd numbers from 1 to 9.
DECLARE @counter = 0;
WHILE @counter < 10 BEGIN
    SET @counter = @counter + 1;
    IF @counter % 2 = 0  -- True for even numbers.
        CONTINUE;
    PRINT @counter;
END
--output--
1
3
5
7
9
