using System;
using System.Linq;

public class ArrayUtil
{
    public static byte[] join(params byte[][] data) {
        byte[] res = new byte[data.Select(arr => arr.Length).Sum()];
        uint i = 0;
        foreach (byte[] arr in data)
            for (int j = 0; j < arr.Length; ++j)
                res[i++] = arr[j];
        return res;
    }

    public static byte[] fromHexString(string str) {
        byte[] res = new byte[str.Length / 2];
        for (int i = 0; i < str.Length / 2; ++i)
            res[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
        return res;
    }
}
