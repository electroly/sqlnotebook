CREATE TABLE foo (a, b);
INSERT INTO foo VALUES
    ('失礼だが', '上記の記事にある３つの誤りを指摘しておきたい'),
    ('いまにも絶滅しようとしている野生動物もいます', '信用して」と彼は言った');

EXPORT TXT '<TEMP>\file.txt'
FROM (SELECT * FROM foo)
OPTIONS (FILE_ENCODING: 932);

IMPORT TXT '<TEMP>\file.txt'
INTO foo2 (num, line)
OPTIONS (FILE_ENCODING: 932);

SELECT * FROM foo2;

--output--
num,line
1,失礼だが上記の記事にある３つの誤りを指摘しておきたい
2,いまにも絶滅しようとしている野生動物もいます信用して」と彼は言った
-
