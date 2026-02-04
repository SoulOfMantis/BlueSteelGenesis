using System;
using UnityEngine;

public class HKDF<HMAC_T> where HMAC_T : System.Security.Cryptography.HMAC, new()
{
    readonly static int hash_len = new HMAC_T().HashSize / 8;
    byte[] prk = null;
    
    public byte[] extract(byte[] salt, byte[] key_material) {
        salt ??= new byte[hash_len];
        return prk = new HMAC_T(){ Key = salt }.ComputeHash(key_material);
    }
    public byte[] expand(byte[] info, int output_length) {
        var n = Mathf.CeilToInt((float)output_length / hash_len);
        var result = new byte[n * hash_len];
        var hmac = new HMAC_T(){ Key = prk };

        byte[] hashing_data = new byte[hash_len + info.Length + 1];
        Array.Copy(info, 0, hashing_data, hash_len, info.Length);
        hashing_data[^1] = 1;
        Array.Copy(
            hmac.ComputeHash(hashing_data, hash_len, info.Length + 1), 0,
            result, 0, hash_len
        );

        for (int i = 1; i < n; ++i) {
            Array.Copy(result, hash_len * (i - 1), hashing_data, 0, hash_len);
            hashing_data[^1] = (byte)(i + 1);

            Array.Copy(
                hmac.ComputeHash(hashing_data), 0,
                result, hash_len * i, hash_len
            );
        }
        Array.Resize(ref result, output_length);
        return result;
    }
}
