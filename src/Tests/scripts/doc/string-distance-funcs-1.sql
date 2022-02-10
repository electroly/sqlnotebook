PRINT LEVENSHTEIN('pickle', 'pickle'); -- 0
PRINT LEVENSHTEIN('pickle', 'tickle'); -- 1
PRINT LEVENSHTEIN('pickle', 'stick');  -- 4
--output--
0
1
4
