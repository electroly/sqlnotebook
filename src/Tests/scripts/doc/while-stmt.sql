-- Prints the numbers 1 to 10 in increasing order.
DECLARE @counter = 1;
WHILE @counter <= 10 BEGIN
    PRINT @counter;
    SET @counter = @counter + 1;
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
