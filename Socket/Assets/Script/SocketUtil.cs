using System;
using System.Net.Sockets;
using System.Text;

public class SocketUtil
{

    public static byte[] ConvertStrToSend(int protocol, string s)
    {
        try
        {
            byte[] content = Encoding.Default.GetBytes(s);
            byte[] data = ConvertByteArrayToSend(protocol, content);
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
    public static int ReadLength(Socket soket)
    {
        int ret = 0;
        try
        {
            byte[] data = new byte[4];
            ret = soket.Receive(data, 0, 4, SocketFlags.None);
            if (ret > 0)
            {
                if (BitConverter.IsLittleEndian) // 若为 小端模式
                {
                    Array.Reverse(data); // 转换为 大端模式               
                }
                return BitConverter.ToInt32(data, 0);
            }
        }
        catch (SocketException exception)
        {
            throw (exception);
        }
        return ret;
    }

    /// <summary>
    /// 读取协议
    /// </summary>
    /// <param name="soket"></param>
    /// <returns></returns>
    public static int ReadProtocol(Socket soket)
    {
        int ret = 0;
        try
        {
            byte[] data = new byte[4];
            ret = soket.Receive(data, 0, 4, SocketFlags.None);
            if (ret > 0)
            {
                if (BitConverter.IsLittleEndian) // 若为 小端模式
                {
                    Array.Reverse(data); // 转换为 大端模式               
                }
                return BitConverter.ToInt32(data, 0);
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
    public static bool ReadContent(Socket soket, byte[] buffer, byte[] data)
    {
        try
        {
            int read = 0;
            int length = data.Length;
            int left = data.Length;
            while (length > 0
                && left > 0)
            {
                read = soket.Receive(buffer, 8 + read, left, SocketFlags.None);

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
                    if (BitConverter.IsLittleEndian) // 若为 小端模式
                    {
                        Array.Reverse(data); // 转换为 大端模式               
                    }
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
    public static bool Write(Socket soket, byte[] data)
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
    public static byte[] ConvertByteArrayToSend(int protocol, byte[] content)
    {
        try
        {
            byte[] length = BitConverter.GetBytes(content.Length);
            byte[] cmd = BitConverter.GetBytes(protocol);
            byte[] data = new byte[content.Length + 8];
            Buffer.BlockCopy(length, 0, data, 0, length.Length);//前4位代表内容长度
            Buffer.BlockCopy(cmd, 0, data, 4, cmd.Length);//中间4位代表协议
            Buffer.BlockCopy(content, 0, data, 8, content.Length);//内容
            return data;
        }
        catch (Exception e)
        {
            throw (e);
        }

    }

    public static byte[] convertPositionData(int player, int posX, int posY)
    {
        try
        {
            byte[] p = BitConverter.GetBytes(player);
            byte[] x = BitConverter.GetBytes(posX);
            byte[] y = BitConverter.GetBytes(posY);
            byte[] data = new byte[12];
            Buffer.BlockCopy(p, 0, data, 0, p.Length);
            Buffer.BlockCopy(x, 0, data, 4, x.Length);
            Buffer.BlockCopy(y, 0, data, 8, y.Length);
            return data;
        }
        catch (Exception e)
        {
            throw (e);
        }
    }

    public static int readPlayer(byte[] data)
    {
        try
        {
            int ret = 0;
            byte[] p = new byte[4];
            Buffer.BlockCopy(data, 0, p, 0, p.Length);
            ret = BitConverter.ToInt32(p, 0);
            return ret;
        }
        catch (Exception e)
        {
            throw (e);
        }
    }

    public static int readPosX(byte[] data)
    {
        try
        {
            int ret = 0;
            byte[] x = new byte[4];
            Buffer.BlockCopy(data, 4, x, 0, x.Length);
            ret = BitConverter.ToInt32(x, 0);
            return ret;
        }
        catch (Exception e)
        {
            throw (e);
        }
    }

    public static int readPosY(byte[] data)
    {
        try
        {
            int ret = 0;
            byte[] y = new byte[4];
            Buffer.BlockCopy(data, 8, y, 0, y.Length);
            ret = BitConverter.ToInt32(y, 0);
            return ret;
        }
        catch (Exception e)
        {
            throw (e);
        }
    }

    public static int[] readPositonData(byte[] data)
    {
        try
        {
            int[] ret = new int[3];
            byte[] p = new byte[4];
            byte[] x = new byte[4];
            byte[] y = new byte[4];
            Buffer.BlockCopy(data, 0, data, 0, p.Length);
            Buffer.BlockCopy(data, 4, data, 0, x.Length);
            Buffer.BlockCopy(data, 8, data, 0, y.Length);
            ret[0] = BitConverter.ToInt32(p, 0);
            ret[1] = BitConverter.ToInt32(x, 0);
            ret[2] = BitConverter.ToInt32(y, 0);
            return ret;
        }
        catch (Exception e)
        {
            throw (e);
        }
    }

    public static byte[] readContent(byte[] buffer)
    {
        try
        {
            byte[] length = new byte[4];
            Buffer.BlockCopy(buffer, 0, length, 0, 4);
            int num = BitConverter.ToInt32(length, 0);
            byte[] data = new byte[num];
            Buffer.BlockCopy(buffer, 8, data, 0, num);
            return data;
        }
        catch (Exception e)
        {
            throw (e);
        }
    }

    public static byte[] readOriginalByteArray(byte[] buffer)
    {
        try
        {
            byte[] length = new byte[4];
            Buffer.BlockCopy(buffer, 0, length, 0, 4);
            int num = BitConverter.ToInt32(length, 0);
            byte[] data = new byte[num + 8];
            Buffer.BlockCopy(buffer, 0, data, 0, data.Length);
            return data;
        }
        catch (Exception e)
        {
            throw (e);
        }
    }

    public static int readProtocol(byte[] buffer)
    {
        try
        {
            byte[] cmd = new byte[4];
            Buffer.BlockCopy(buffer, 4, cmd, 0, 4);
            int num = BitConverter.ToInt32(cmd, 0);
            return num;
        }
        catch (Exception e)
        {
            throw (e);
        }
    }
}
