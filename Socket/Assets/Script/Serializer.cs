using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Serializer
{
    public static void Seriaize(int protocol, byte[] data)
    {
        if(protocol == Protocol.Login)
        {
            int id = ByteUtil.bytesToInt2(data, 0);
            SceneController.getInstance().SetMyId(id);
            SceneController.getInstance().createBall(id);
        }
        else if(protocol == Protocol.PlayerJoin)
        {
            int id = ByteUtil.bytesToInt2(data, 0);
            SceneController.getInstance().createBall(id);
        }
        else if (protocol == Protocol.PlayerLeave)
        {
            int id = ByteUtil.bytesToInt2(data, 0);
            SceneController.getInstance().createBall(id);
        }
        else if (protocol == Protocol.Move)
        {
            int id = ByteUtil.bytesToInt2(data, 0);
            int dir = ByteUtil.bytesToInt2(data, 4);
            SceneController.getInstance().ballMove(id, dir);
        }
        else if (protocol == Protocol.StartGame)
        {
            SceneController.getInstance().startGame();
        }
    }
}
