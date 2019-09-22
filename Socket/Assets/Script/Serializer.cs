using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Serializer
{
    public static void Deserialize(int protocol, byte[] data)
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
            SceneController.getInstance().delBall(id);
        }
        else if (protocol == Protocol.AddPlayer)
        {
            int num = data.Length / 4;
            int offset = 0;
            for(int i = 0; i < num; i++)
            {
                int id = ByteUtil.bytesToInt2(data, offset);
                SceneController.getInstance().createBall(id);
                offset += 4;
            }

        }
        else if (protocol == Protocol.Move)
        {
            int offset = 0;
            int num = ByteUtil.bytesToInt2(data, offset);
            for(int i = 0;i< num; i++)
            {
                offset += 4;
                int id = ByteUtil.bytesToInt2(data, offset);
                offset += 4;
                int actions = ByteUtil.bytesToInt2(data, offset);
                for (int j = 0; j < actions; j++) 
                {
                    offset += 4;
                    int dir = ByteUtil.bytesToInt2(data, offset);
                    SceneController.getInstance().ballMove(id, dir);
                }
            }

        }
        else if (protocol == Protocol.StartGame)
        {
            SceneController.getInstance().startGame();
        }
    }
}
