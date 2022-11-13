using System.Diagnostics;
using System.Text;

namespace SqlNotebookScript.Utils;

public static class BlobUtil
{
    public static string ToString(byte[] bytes)
    {
        StringBuilder sb = new(42);
        sb.Append('[');
        if (ArrayUtil.IsSqlArray(bytes))
        {
            sb.Append(string.Join(", ", ArrayUtil.GetArrayElements(bytes)));
        }
        else
        {
            if (bytes.Length < 20)
            {
                for (var i = 0; i < bytes.Length; i++)
                {
                    var b = bytes[i];
                    sb.Append(NibbleToHex(b >> 4));
                    sb.Append(NibbleToHex(b & 0xF));
                }
            }
            else
            {
                sb.Append(bytes.Length.ToString("#,##0"));
                sb.Append(" bytes");
            }
        }
        sb.Append(']');
        return sb.ToString();
    }

    private static char NibbleToHex(int nibble)
    {
        Debug.Assert(nibble >= 0 && nibble < 16);
        if (nibble < 10)
        {
            return (char)('0' + nibble);
        }
        else
        {
            return (char)('A' + (nibble - 10));
        }
    }
}
