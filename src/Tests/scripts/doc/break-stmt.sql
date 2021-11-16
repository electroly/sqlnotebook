-- Prints the numbers 1 to 10.
DECLARE @counter = 1;
WHILE 1 BEGIN
    PRINT @counter;
    SET @counter = @counter + 1;
    IF @counter > 10
        BREAK;
END
--output--
1
2
3
4
5
6
7
8
9
10
