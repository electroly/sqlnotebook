CREATE TABLE test (id INTEGER PRIMARY KEY, value TEXT);

INSERT INTO test (value) VALUES ('apple'), ('banana'), ('cherry');

SELECT string_agg(value, ', ') AS aggregated_string FROM test;
--output--
aggregated_string
apple, banana, cherry
-
