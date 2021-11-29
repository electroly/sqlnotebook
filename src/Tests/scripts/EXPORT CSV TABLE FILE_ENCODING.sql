CREATE TABLE foo (a, b);
INSERT INTO foo VALUES
    ('失礼だが', '上記の記事にある３つの誤りを指摘しておきたい'),
    ('いまにも絶滅しようとしている野生動物もいます', '信用して」と彼は言った');

EXPORT CSV '<TEMP>\file.csv' FROM TABLE foo OPTIONS (FILE_ENCODING: 932);
PRINT '[' || READ_FILE_TEXT('<TEMP>\file.csv', 932) || ']';

--output--
[a,b
失礼だが,上記の記事にある３つの誤りを指摘しておきたい
いまにも絶滅しようとしている野生動物もいます,信用して」と彼は言った
]
