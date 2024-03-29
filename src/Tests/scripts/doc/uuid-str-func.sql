-- These all print the same thing: 'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'
PRINT UUID_STR('A0EEBC99-9C0B-4EF8-BB6D-6BB9BD380A11');
PRINT UUID_STR('{a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11}');
PRINT UUID_STR('a0eebc999c0b4ef8bb6d6bb9bd380a11');
PRINT UUID_STR('a0ee-bc99-9c0b-4ef8-bb6d-6bb9-bd38-0a11');
PRINT UUID_STR('{a0eebc99-9c0b4ef8-bb6d6bb9-bd380a11}');

-- Prints 1, because 'foobar' is not a valid UUID.
PRINT UUID_STR('foobar') IS NULL;

-- Prints 1, because the blob returned by ARRAY() is not a UUID.
PRINT UUID_STR(ARRAY(1, 2, 3)) IS NULL;

--output--
a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11
a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11
a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11
a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11
a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11
1
1
