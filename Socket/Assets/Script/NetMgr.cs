using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMgr
{
    private static NetMgr _instance;

    public static NetMgr getInstance()
    {
        if(_instance == null) 
        {
            _instance = new NetMgr(); 
        }
        return _instance;
    }

    public void send(int prot, byte[] content)
    {
        byte[] data = SocketUtil.convertByteArrayToSend(prot, content);
        NetScene.getInstance().client.writeByte(data);
    }
}
