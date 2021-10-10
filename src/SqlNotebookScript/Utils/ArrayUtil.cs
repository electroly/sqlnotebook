using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SqlNotebookScript.Utils {
    public static class ArrayUtil {
        // the blob format for arrays is:
        // - 8 bytes: chars "SNB~arr~"
        // - 32-bit integer: number of elements
        // - for each element:
        //    - 32-bit integer: length of the element record to follow (excluding this size integer itself)
        //    - 1 byte: SQLITE_INTEGER/FLOAT/BLOB/NULL/TEXT
        //    - for null: nothing
        //    - for integer: the 64-bit integer value
        //    - for float: the 64-bit floating point value
        //    - for text: a 32-bit integer byte count of the UTF-8 text to follow, then the UTF-8 text
        //    - for blob: a 32-bit integer byte count of the blob to follow, then the blob data

        public const byte SQLITE_INTEGER = 1;
        public const byte SQLITE_FLOAT = 2;
        public const byte SQLITE_TEXT = 3;
        public const byte SQLITE_BLOB = 4;
        public const byte SQLITE_NULL = 5;

        private const string SENTINEL_TEXT = "SNB~arr~";
        private static readonly byte[] _sentinel = Encoding.ASCII.GetBytes(SENTINEL_TEXT);
        private const int SENTINEL_OFFSET = 0;
        private static readonly int COUNT_OFFSET = SENTINEL_TEXT.Length;
        private static readonly int DATA_OFFSET = COUNT_OFFSET + sizeof(int);

        public static bool IsSqlArray(byte[] blob) {
            return HasSentinel(blob);
        }

        // write the element records (but not the initial sentinel nor count)
        public static void ConvertToSqlArray(IReadOnlyList<object> objects, BinaryWriter writer) {
            var cbByte = sizeof(byte);
            var cbInt32 = sizeof(int);
            var cbInt64 = sizeof(long);
            var cbDouble = sizeof(double);

            for (int i = 0; i < objects.Count; i++) {
                var o = objects[i] ?? DBNull.Value;
                var t = o.GetType();
                if (t == typeof(long)) {
                    writer.Write((int)(cbByte + cbInt64));
                    writer.Write((byte)SQLITE_INTEGER);
                    writer.Write((long)o);
                } else if (t == typeof(double)) {
                    writer.Write((int)(cbByte + cbDouble));
                    writer.Write((byte)SQLITE_FLOAT);
                    writer.Write((double)o);
                } else if (t == typeof(string)) {
                    var utf8 = Encoding.UTF8.GetBytes((string)o);
                    writer.Write((int)(cbByte + cbInt32 + utf8.Length));
                    writer.Write((byte)SQLITE_TEXT);
                    writer.Write((int)(utf8.Length));
                    writer.Write(utf8);
                } else if (t == typeof(byte[])) {
                    var bytes = (byte[])o;
                    writer.Write((int)(cbByte + cbInt32 + bytes.Length));
                    writer.Write((byte)SQLITE_BLOB);
                    writer.Write((int)(bytes.Length));
                    writer.Write(bytes);
                } else if (t == typeof(DBNull)) {
                    writer.Write((int)(cbByte));
                    writer.Write((byte)SQLITE_NULL);
                } else {
                    throw new ArgumentException(
                        $"Unrecognized object type \"{t.Name}\" found at index {i} when building SQL array.");
                }
            }
        }

        public static byte[] ConvertToSqlArray(IReadOnlyList<object> objects) {
            using (var memoryStream = new MemoryStream())
            using (var writer = new BinaryWriter(memoryStream)) {
                writer.Write(_sentinel);
                writer.Write((int)objects.Count);
                ConvertToSqlArray(objects, writer);
                return memoryStream.ToArray();
            }
        }

        private static void VerifySentinel(byte[] blob) {
            if (!HasSentinel(blob)) {
                throw new Exception("This blob is not an array.");
            }
        }

        private static bool HasSentinel(byte[] blob) {
            if (blob.Length < _sentinel.Length) {
                return false;
            }
            for (int i = 0; i < _sentinel.Length; i++) {
                if (_sentinel[i] != blob[i]) {
                    return false;
                }
            }
            return true;
        }

        private static int ReadInt32(byte[] blob, int offset) {
            if (offset + sizeof(int) > blob.Length) {
                throw new Exception("Unexpected end of blob data.");
            } else {
                return BitConverter.ToInt32(blob, offset);
            }
        }

        private static long ReadInt64(byte[] blob, int offset) {
            if (offset + sizeof(long) > blob.Length) {
                throw new Exception("Unexpected end of blob data.");
            } else {
                return BitConverter.ToInt64(blob, offset);
            }
        }

        private static double ReadDouble(byte[] blob, int offset) {
            if (offset + sizeof(double) > blob.Length) {
                throw new Exception("Unexpected end of blob data.");
            } else {
                return BitConverter.ToDouble(blob, offset);
            }
        }

        private static byte ReadByte(byte[] blob, int offset) {
            if (offset + sizeof(byte) > blob.Length) {
                throw new Exception("Unexpected end of blob data.");
            } else {
                return blob[offset];
            }
        }

        public static int GetArrayCount(byte[] arrayBlob) {
            VerifySentinel(arrayBlob);
            if (arrayBlob.Length < sizeof(int)) {
                throw new Exception("This blob is not an array.");
            } else {
                return ReadInt32(arrayBlob, COUNT_OFFSET);
            }
        }

        private static object ReadArrayElement(byte[] arrayBlob, ref int position) {
            var elementLength = ReadInt32(arrayBlob, position);
            position += sizeof(int);
            var dataType = ReadByte(arrayBlob, position);
            position += sizeof(byte);
            switch (dataType) {
                case SQLITE_INTEGER: {
                    var value = ReadInt64(arrayBlob, position);
                    position += sizeof(long);
                    return value;
                }

                case SQLITE_FLOAT: {
                    var value = ReadDouble(arrayBlob, position);
                    position += sizeof(double);
                    return value;
                }

                case SQLITE_TEXT: {
                    var cbText = ReadInt32(arrayBlob, position);
                    position += sizeof(int);
                    var str = Encoding.UTF8.GetString(arrayBlob, position, cbText);
                    position += cbText;
                    return str;
                }

                case SQLITE_BLOB: {
                    var cbBlob = ReadInt32(arrayBlob, position);
                    position += sizeof(int);
                    var blob = new byte[cbBlob];
                    Array.Copy(arrayBlob, position, blob, 0, cbBlob);
                    position += cbBlob;
                    return blob;
                }

                case SQLITE_NULL:
                    return DBNull.Value;

                default:
                    throw new Exception($"Unrecognized data type in array blob: {dataType}");
            }
        }

        public static object GetArrayElement(byte[] arrayBlob, int elementIndex) {
            VerifySentinel(arrayBlob);
            var count = GetArrayCount(arrayBlob);
            if (elementIndex < 0 || elementIndex >= count) {
                throw new Exception(
                    $"The index {elementIndex} is out of bounds for the array, which has {count} elements.");
            }

            int position = DATA_OFFSET;
            for (int i = 0; i < elementIndex; i++) {
                // skip this element
                var skipElementLength = ReadInt32(arrayBlob, position);
                position += sizeof(int) + skipElementLength;
            }

            return ReadArrayElement(arrayBlob, ref position);
        }

        public static object[] GetArrayElements(byte[] arrayBlob) {
            VerifySentinel(arrayBlob);
            var count = GetArrayCount(arrayBlob);
            var array = new object[count];
            int position = DATA_OFFSET;
            for (int i = 0; i < count; i++) {
                array[i] = ReadArrayElement(arrayBlob, ref position);
            }
            return array;
        }

        public static byte[] SliceArrayElements(byte[] originalArrayBlob, int index, int removeElements,
        IReadOnlyList<object> insertElements) {
            VerifySentinel(originalArrayBlob);
            var oldCount = GetArrayCount(originalArrayBlob);
            if (index < 0 || index > oldCount) {
                throw new Exception(
                    $"Array index {index} is out of range. The array has {oldCount} elements.");
            }
            if (index + removeElements > oldCount) {
                throw new Exception(
                    $"Array index {index} - removed elements {removeElements} must not exceed the length of the array.");
            }

            var newCount = oldCount - removeElements + insertElements.Count;

            using (var memoryStream = new MemoryStream())
            using (var writer = new BinaryWriter(memoryStream)) {
                writer.Write(_sentinel);
                writer.Write((int)newCount);

                // copy the elements up to the edit point as-is
                int position = DATA_OFFSET;
                for (int i = 0; i < index; i++) {
                    var skipLength = ReadInt32(originalArrayBlob, position);
                    writer.Write(originalArrayBlob, position, sizeof(int) + skipLength);
                    position += sizeof(int) + skipLength;
                }

                // skip the removed elements
                for (int i = index; i < index + removeElements; i++) {
                    var skipLength = ReadInt32(originalArrayBlob, position);
                    position += sizeof(int) + skipLength;
                }

                // insert the new elements
                ConvertToSqlArray(insertElements, writer);

                // copy the rest of the elements as-is
                for (int i = index + removeElements; i < oldCount; i++) {
                    var skipLength = ReadInt32(originalArrayBlob, position);
                    writer.Write(originalArrayBlob, position, sizeof(int) + skipLength);
                    position += sizeof(int) + skipLength;
                }

                return memoryStream.ToArray();
            }
        }
    }
}
