-- https://sqlite.org/rowvalue.html
SELECT
    (1,2,3) = (1,2,3),          -- 1
    (1,2,3) = (1,NULL,3),       -- NULL
    (1,2,3) = (1,NULL,4),       -- 0
    (1,2,3) < (2,3,4),          -- 1
    (1,2,3) < (1,2,4),          -- 1
    (1,2,3) < (1,3,NULL),       -- 1
    (1,2,3) < (1,2,NULL),       -- NULL
    (1,3,5) < (1,2,NULL),       -- 0
    (1,2,NULL) IS (1,2,NULL);   -- 1

--output--
( 1 , 2 , 3 ) = ( 1 , 2 , 3 ),( 1 , 2 , 3 ) = ( 1 , NULL , 3 ),( 1 , 2 , 3 ) = ( 1 , NULL , 4 ),( 1 , 2 , 3 ) < ( 2 , 3 , 4 ),( 1 , 2 , 3 ) < ( 1 , 2 , 4 ),( 1 , 2 , 3 ) < ( 1 , 3 , NULL ),( 1 , 2 , 3 ) < ( 1 , 2 , NULL ),( 1 , 3 , 5 ) < ( 1 , 2 , NULL ),( 1 , 2 , NULL ) IS ( 1 , 2 , NULL )
1,null,0,1,1,1,null,0,1
-
