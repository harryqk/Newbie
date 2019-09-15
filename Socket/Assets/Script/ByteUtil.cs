using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ByteUtil
{
    /// <summary>
    /// Byteses to int.little endian
    /// </summary>
    /// <returns>The to int.</returns>
    /// <param name="src">Source.</param>
    /// <param name="offset">Offset.</param>
    public static int bytesToInt(byte[] src, int offset)
    {
        int value;
        value = (int)((src[offset] & 0xFF)
                | ((src[offset + 1] & 0xFF) << 8)
                | ((src[offset + 2] & 0xFF) << 16)
                | ((src[offset + 3] & 0xFF) << 24));
        return value;
    }

    /// <summary>
    /// Byteses to int2. big endiam
    /// </summary>
    /// <returns>The to int2.</returns>
    /// <param name="src">Source.</param>
    /// <param name="offset">Offset.</param>
    public static int bytesToInt2(byte[] src, int offset)
    {
        int value;
        value = (int)(((src[offset] & 0xFF) << 24)
                | ((src[offset + 1] & 0xFF) << 16)
                | ((src[offset + 2] & 0xFF) << 8)
                | (src[offset + 3] & 0xFF));
        return value;
    }


    /// <summary>
    /// Ints to bytes. little endiam
    /// </summary>
    /// <returns>The to bytes.</returns>
    /// <param name="value">Value.</param>
    public static byte[] intToBytes(int value)
    {
        byte[] src = new byte[4];
        src[3] = (byte)((value >> 24) & 0xFF);
        src[2] = (byte)((value >> 16) & 0xFF);
        src[1] = (byte)((value >> 8) & 0xFF);
        src[0] = (byte)(value & 0xFF);
        return src;
    }

    /// <summary>
    /// Ints to bytes2. big endiam
    /// </summary>
    /// <returns>The to bytes2.</returns>
    /// <param name="value">Value.</param>
    public static byte[] intToBytes2(int value)
    {
        byte[] src = new byte[4];
        src[0] = (byte)((value >> 24) & 0xFF);
        src[1] = (byte)((value >> 16) & 0xFF);
        src[2] = (byte)((value >> 8) & 0xFF);
        src[3] = (byte)(value & 0xFF);
        return src;
    }

    public static byte[] bytesCombine(params byte[][] arr)
    {
        List<byte> ret = new List<byte>();
        int left = arr.Length;
        int start = 0;
        while(left > 0)
        {
            ret.AddRange(arr[start]);
            start++;
            left--;
        }
        return ret.ToArray();
    }
}   
