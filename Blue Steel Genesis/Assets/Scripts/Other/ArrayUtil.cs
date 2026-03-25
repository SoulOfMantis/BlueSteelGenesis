using System;
using System.Collections.Generic;
using System.Linq;

public static class ArrayUtil
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
        if (str == null || str.Length % 2 != 0)
            throw new ArgumentException();

        byte[] res = new byte[str.Length / 2];
        for (int i = 0; i < str.Length / 2; ++i)
            res[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
        return res;
    }

    public static string toHexString(byte[] data) {
        return BitConverter.ToString(data).Replace("-", "");
    }

    public static T MinBy<T>(this IEnumerable<T> seq, Func<T, IComparable> comp) {
        if (seq.Count() == 0)
            return default(T);

        (T val, IComparable est) best_fit = (seq.First(), comp(seq.First()));
        foreach (T el in seq) {
            var el_est = comp(el);
            if (el_est.CompareTo(best_fit.est) < 0)
                best_fit = (el, el_est);
        }
        return best_fit.val;
    }
    public static T MaxBy<T>(this IEnumerable<T> seq, Func<T, IComparable> comp) {
        if (seq.Count() == 0)
            return default(T);

        (T val, IComparable est) best_fit = (seq.First(), comp(seq.First()));
        foreach (T el in seq) {
            var el_est = comp(el);
            if (el_est.CompareTo(best_fit.est) > 0)
                best_fit = (el, el_est);
        }
        return best_fit.val;
    }
}
