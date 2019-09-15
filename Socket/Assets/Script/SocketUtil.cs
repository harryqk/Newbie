using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

public class SocketUtil
{

    public static byte[] convertStrToSend(int protocol, string s)
    {
        try
        {
            byte[] content = Encoding.Default.GetBytes(s);
            byte[] data = convertByteArrayToSend(protocol, content);
            return data;
        }
        catch (Exception e)
        {
            throw (e);
        }
    }

    /// <summary>
    /// 读取内容长度
    /// </summary>
    /// <param name="soket"></param>
    /// <returns></returns>
    public static int readInt(Socket soket, byte[] buffer)
    {
        int ret = 0;
        try
        {
            //byte[] data = new byte[4];
            ret = soket.Receive(buffer, 0, 4, SocketFlags.None);
            if (ret > 0)
            {
                return ByteUtil.bytesToInt2(buffer, 0);
            }
        }
        catch (SocketException exception)
        {
            throw (exception);
        }
        return ret;
    }

    /// <summary>
    /// 读取内容
    /// </summary>
    /// <param name="soket"></param>
    /// <returns></returns>
    public static bool readContent(Socket soket, byte[] buffer, byte[] data)
    {
        try
        {
            int read = 0;
            int length = data.Length;
            int left = data.Length;
            while (length > 0
                && left > 0)
            {
                read = soket.Receive(buffer, read, left, SocketFlags.None);

                if (read <= 0)
                {
                    return false;
                }

                if (read > 0)
                {
                    read += read;
                    left -= read;
                }
                if (left <= 0)
                {
                    Buffer.BlockCopy(buffer, 0, data, 0, length);
                    return true;
                }
            }
        }
        catch (SocketException exception)
        {
            throw (exception);
        }
        return false;
    }




    /// <summary>
    /// 读取内容长度
    /// </summary>
    /// <param name="soket"></param>
    /// <returns></returns>
    public static bool write(Socket soket, byte[] data)
    {
        try
        {
            int send = 0;
            int length = data.Length;
            int left = data.Length;
            while (length > 0
                && left > 0)
            {
                send = soket.Send(data, send, left, SocketFlags.None);

                if (send <= 0)
                {
                    return false;
                }

                if (send > 0)
                {
                    send += send;
                    left -= send;
                }

                if (left <= 0)
                {
                    return true;
                }
            }
        }
        catch (SocketException e)
        {
            throw (e);
        }
        return false;
    }

    /// <summary>
    /// 创建发送内容
    /// </summary>
    /// <param name="protocol"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static byte[] convertByteArrayToSend(int protocol, byte[] content)
    {
        try
        {

            byte[] length = ByteUtil.intToBytes2(content.Length);
            byte[] cmd = ByteUtil.intToBytes2(protocol);

            //byte[] data = new byte[content.Length + 8];
            //Buffer.BlockCopy(length, 0, data, 0, length.Length);//前4位代表内容长度
           //Buffer.BlockCopy(cmd, 0, data, 4, cmd.Length);//中间4位代表协议
          //Buffer.BlockCopy(content, 0, data, 8, content.Length);//内容
            //return data;
            return ByteUtil.bytesCombine(length, cmd, content);

        }
        catch (Exception e)
        {
            throw (e);
        }

    }
}
