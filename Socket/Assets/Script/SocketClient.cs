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
    Queue<MessageVO> queueMove = new Queue<MessageVO>();
    Queue<string> queueLog = new Queue<string>();
    byte[] bufferForInt = new byte[4];
    byte[] buffer = new byte[1024 * 8];
    Thread threadConnect = null;
    Thread threadRead = null;
    bool heartBreak = false;
    Thread threadHeartBreak = null;
    System.Timers.Timer timerSend = null;
    System.Timers.Timer timerCheck = null;
    EventHandler onConnectHandler = null;
    bool conneted = false;
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

    public bool isConnected()
    {
        return conneted;
    }

    public Queue<string> getReadMsg()
    {
        return queueRead;
    }

    public Queue<MessageVO> getMoveMsg()
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
        if (threadConnect != null)
        {
            threadConnect.Abort();
        }
        if (threadHeartBreak != null)
        {
            threadHeartBreak.Abort();
        }
        if (socketClient != null)
        {
            socketClient.Close();
            socketClient = null;
        }
        queueLog.Enqueue("关闭读线程1");
        if (threadRead != null)
        {
            threadRead.Abort();
        }
        queueLog.Enqueue("客户端关闭");
        conneted = false;
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
        try
        {
            IPAddress ipAddress = IPAddress.Parse(serverIp);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, serverPort);
            socketClient = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socketClient.Connect(ipAddress, serverPort);
            OnConnected();
        }
        catch(Exception exc)
        {
            OnServerConnectFailed();
            throw exc;
        }
    }

    /// <summary>
    /// 连接成功
    /// </summary>
    void OnConnected()
    {
        conneted = true;
        queueLog.Enqueue("连接服务器成功");
        StartReceive();
        queueLog.Enqueue("开始接受数据");
    }

    /// <summary>
    /// 开始接收数据
    /// </summary>
    void StartReceive()
    {
        threadRead = new Thread(receive);
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
        byte[] dataSend = SocketUtil.convertStrToSend(protocol, s);
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
                bool value = SocketUtil.write(socketClient, data);
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
                int len = SocketUtil.readInt(socketClient, bufferForInt);
                if (len > 0)
                {
                    int protocol = SocketUtil.readInt(socketClient, bufferForInt);
                    if (protocol > 0)
                    {
                        byte[] data = new byte[len];
                        bool read = SocketUtil.readContent(socketClient, buffer, data);
                        if (read)
                        {
                            //string strReceive = Encoding.Default.GetString(data);
                            //queueRead.Enqueue(strReceive);
                            MessageVO messageVO = new MessageVO();
                            messageVO.protocol = protocol;
                            messageVO.data = data;
                            queueMove.Enqueue(messageVO);
                            //Debug.Log("client read from server:" + socketClient.RemoteEndPoint + "|" + strReceive);
                        }
                        else
                        {
                            OnReadContenError();
                            break;
                        }
                    }
                    else
                    {
                        OnReadProtocolError();
                        break;
                    }
                }
                else
                {
                    OnReadLengthError();
                    break;
                }
            }
            else if(socketClient.Poll(-1, SelectMode.SelectError))
            {
                OnServerDisconnected();
                break;
            }

        }

        OnServerDisconnected();
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
        ClientWrite(Protocol.Move, "1");
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

    void OnServerDisconnected()
    {
        queueLog.Enqueue("服务器断开链接");
    }

    void OnServerConnectFailed()
    {
        queueLog.Enqueue("连接服务器失败");
    }

    public void receive()
    {
        while (socketClient != null
            && socketClient.Connected == true)
        {
            int len = SocketUtil.readInt(socketClient, bufferForInt);
            if (len > 0)
            {
                int protocol = SocketUtil.readInt(socketClient, bufferForInt);
                if (protocol > 0)
                {
                    byte[] data = new byte[len];
                    bool read = SocketUtil.readContent(socketClient, buffer, data);
                    if (read)
                    {
                        //string strReceive = Encoding.Default.GetString(data);
                        //queueRead.Enqueue(strReceive);
                        MessageVO messageVO = new MessageVO();
                        messageVO.protocol = protocol;
                        messageVO.data = data;
                        queueMove.Enqueue(messageVO);
                        //Debug.Log("client read from server:" + socketClient.RemoteEndPoint + "|" + strReceive);
                    }
                    else
                    {
                        OnReadContenError();
                        break;
                    }
                }
                else
                {
                    OnReadProtocolError();
                    break;
                }
            }
            else
            {
                OnReadLengthError();
                break;
            }
        }

        OnServerDisconnected();
    }
}
