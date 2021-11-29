WITH foo AS NOT MATERIALIZED (
    SELECT 1, 2, 3
)
SELECT * FROM foo;
--output--
1,2,3
1,2,3
-
