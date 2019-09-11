using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;

public class SocketClient
{
    string serverIp = "";
    int serverPort = 0;
    Socket socketClient = null;
    Queue<string> queueRead = new Queue<string>();
    Queue<byte[]> queueMove = new Queue<byte[]>();
    Queue<string> queueLog = new Queue<string>();
    byte[] buffer = new byte[1024 * 8];
    Thread threadConnect = null;
    Thread threadRead = null;
    bool heartBreak = false;
    Thread threadHeartBreak = null;
    System.Timers.Timer timerSend = null;
    System.Timers.Timer timerCheck = null;
    EventHandler onConnectHandler = null;
    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public void Connect(string ip, int port)
    {
        serverIp = ip;
        serverPort = port;
        StartConnet();
    }

    public Queue<string> getReadMsg()
    {
        return queueRead;
    }

    public Queue<byte[]> getMoveMsg()
    {
        return queueMove;
    }

    public Queue<string> GetLog()
    {
        return queueLog;
    }

    /// <summary>
    /// 关闭socket
    /// </summary>
    public void Close()
    {
        if (timerSend != null)
        {
            timerSend.Close();
        }
        if (socketClient != null)
        {
            socketClient.Close();
            socketClient = null;
        }
        if (threadRead != null)
        {
            threadRead.Abort();
        }
        if (threadConnect != null)
        {
            threadConnect.Abort();
        }
        if (threadHeartBreak != null)
        {
            threadHeartBreak.Abort();
        }
    }

    /// <summary>
    /// 开启连接线程
    /// </summary>
    void StartConnet()
    {
        threadConnect = new Thread(ThreadConnect);
        threadConnect.IsBackground = true;
        threadConnect.Start();
    }

    /// <summary>
    /// 连接线程
    /// </summary>
    void ThreadConnect()
    {
        IPAddress ipAddress = IPAddress.Parse(serverIp);
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, serverPort);
        socketClient = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socketClient.Connect(ipAddress, serverPort);
        OnConnected();
    }

    /// <summary>
    /// 连接成功
    /// </summary>
    void OnConnected()
    {
        queueLog.Enqueue("连接服务器成功");
        Receive();
        queueLog.Enqueue("开始接受数据");
    }

    /// <summary>
    /// 开始接收数据
    /// </summary>
    void StartReceive()
    {
        threadRead = new Thread(Receive);
        threadRead.IsBackground = true;
        threadRead.Start();
    }

    /// <summary>
    /// 传输数据
    /// </summary>
    /// <param name="protocol"></param>
    /// <param name="s"></param>
    public void ClientWrite(int protocol, string s)
    {
        byte[] dataSend = SocketUtil.ConvertStrToSend(protocol, s);
        writeByte(dataSend);
    }

    public void writeByte(byte[] data)
    {
        if (data == null
            && data.Length == 0)
        {
            return;
        }
        if (socketClient != null
                && socketClient.Connected == true)
        {
            if (socketClient.Poll(-1, SelectMode.SelectWrite))
            {
                bool value = SocketUtil.Write(socketClient, data);
                if (!value)
                {
                    OnWriteError();
                }
                //try
                //{
                //    if (socketClient.Connected == true)
                //    {
                //        int send = socketClient.Send(data);
                //    }
                //}
                //catch (SocketException e)
                //{
                //    throw (e);
                //}

            }
        }
    }

    public int getLocalPort()
    {
        int ret = 0;
        if (socketClient != null
            && socketClient.LocalEndPoint != null)
        {
            int.TryParse(socketClient.LocalEndPoint.ToString().Split(':')[1], out ret);
        }
        return ret;
    }

    /// <summary>
    /// 接收数据线程
    /// </summary>
    public void Receive()
    {
        while (socketClient != null
            && socketClient.Connected == true)
        {
            if (socketClient.Poll(-1, SelectMode.SelectRead))
            {
                int len = SocketUtil.ReadLength(socketClient);
                if (len > 0)
                {
                    int protocol = SocketUtil.ReadProtocol(socketClient);
                    if (protocol > 0)
                    {
                        byte[] data = new byte[len - 8];
                        bool read = SocketUtil.ReadContent(socketClient, buffer, data);
                        if (read)
                        {
                            string strReceive = Encoding.Default.GetString(data);
                            queueRead.Enqueue(strReceive);
                            //Debug.Log("client read from server:" + socketClient.RemoteEndPoint + "|" + strReceive);
                        }
                        else
                        {
                            OnReadContenError();
                        }
                    }
                    else
                    {
                        OnReadProtocolError();
                    }
                }
                else
                {
                    OnReadLengthError();
                }
            }

        }
    }

    void startHeartBreakCheck()
    {
        Thread thread = new Thread(heartBreakCheck);
        thread.IsBackground = true;
        thread.Start();
    }

    void startHeartBreakSend()
    {
        timerSend = new System.Timers.Timer();
        timerSend.Elapsed += new ElapsedEventHandler(onHeartBreakSend);
        timerSend.Interval = 10000;
        timerSend.AutoReset = true;
        timerSend.Enabled = true;
        timerSend.Start();
    }

    void heartBreakCheck()
    {
        Thread.Sleep(10000);
        timerCheck = new System.Timers.Timer();
        timerCheck.Elapsed += new ElapsedEventHandler(onHeartBreakCheck);
        timerCheck.Interval = 20000;
        timerCheck.AutoReset = true;
        timerCheck.Enabled = true;
        timerCheck.Start();
    }

    void onHeartBreakCheck(object source, ElapsedEventArgs e)
    {
        if (!heartBreak)
        {
            if (socketClient != null)
            {
                socketClient.Close();
                socketClient = null;
            }
        }
        else
        {
            heartBreak = false;
        }
    }

    void onHeartBreakSend(object source, ElapsedEventArgs e)
    {
        ClientWrite(Protocol.HEART_BREAK, "1");
    }

    void OnReadLengthError()
    {
        queueLog.Enqueue("读取长度错误");
    }

    void OnReadProtocolError()
    {
        queueLog.Enqueue("读取协议错误");
    }


    void OnReadContenError()
    {
        queueLog.Enqueue("读取内容错误");
    }

    void OnWriteError()
    {
        queueLog.Enqueue("写错错误");
    }
}
