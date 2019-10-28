public class NetMgr
{
    private static NetMgr _instance;

    public static NetMgr GetInstance()
    {
        if (_instance == null)
        {
            _instance = new NetMgr();
        }
        return _instance;
    }

    public SocketClient client;

    public void Send(int prot, byte[] content)
    {
        byte[] data = NetUtil.ConvertByteArrayToSend(prot, content);
        if (client != null
            && client.IsConnected())
        {
            client.WriteByte(data);
        }
    }
}
