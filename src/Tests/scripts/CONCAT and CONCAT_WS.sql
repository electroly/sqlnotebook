CREATE TABLE test_data (
    id INTEGER PRIMARY KEY,
    first_name TEXT,
    last_name TEXT,
    city TEXT
);

INSERT INTO test_data (first_name, last_name, city)
VALUES ('John', 'Doe', 'New York'),
       ('Jane', 'Doe', 'Los Angeles'),
       ('Jim', 'Beam', 'Chicago');

SELECT concat(first_name, ' ', last_name) AS full_name
FROM test_data;

SELECT concat_ws(', ', first_name, last_name, city) AS full_information
FROM test_data;

SELECT concat('', first_name) AS result
FROM test_data;

SELECT concat_ws('', first_name, last_name) AS result
FROM test_data;

SELECT concat(NULL, first_name) AS result
FROM test_data;

SELECT concat_ws(', ', NULL, first_name, last_name) AS result
FROM test_data;

--output--
full_name
John Doe
Jane Doe
Jim Beam
-
full_information
John, Doe, New York
Jane, Doe, Los Angeles
Jim, Beam, Chicago
-
result
John
Jane
Jim
-
result
JohnDoe
JaneDoe
JimBeam
-
result
John
Jane
Jim
-
result
John, Doe
Jane, Doe
Jim, Beam
-
