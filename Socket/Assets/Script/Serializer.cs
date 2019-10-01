using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Serializer
{
    public static void SynDeserialize(int protocol, byte[] data)
    {
        if(protocol == Protocol.Login)
        {
            int id = ByteUtil.bytesToInt2(data, 0);
            NetScene.getInstance().SetMyId(id);
            NetScene.getInstance().createBall(id);
        }
        else if(protocol == Protocol.PlayerJoin)
        {
            int id = ByteUtil.bytesToInt2(data, 0);
            NetScene.getInstance().createBall(id);
        }
        else if (protocol == Protocol.PlayerLeave)
        {
            int id = ByteUtil.bytesToInt2(data, 0);
            NetScene.getInstance().delBall(id);
        }
        else if (protocol == Protocol.AddPlayer)
        {
            int num = data.Length / 4;
            int offset = 0;
            for(int i = 0; i < num; i++)
            {
                int id = ByteUtil.bytesToInt2(data, offset);
                NetScene.getInstance().createBall(id);
                offset += 4;
            }

        }
        else if (protocol == Protocol.Move)
        {
            //Debug.Log("update move");
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
                    NetScene.getInstance().ballMove(id, dir);
                }
            }

            NetScene.getInstance().updatePos();

        }
        else if (protocol == Protocol.StartGame)
        {
            NetScene.getInstance().startGame();
        }
        else if (protocol == Protocol.Update)
        {
            actionSynSerialize(data);
            NetScene.getInstance().updatePos();
            return;
            int offset = 0;
            int haveData = ByteUtil.bytesToInt2(data, offset);
            //Debug.Log("update move");
            if (haveData == 1)
            {
                offset = 4;
                int num = data.Length / 8;
                for (int i = 0; i < num; i++)
                {
                    int id = ByteUtil.bytesToInt2(data, offset);
                    offset += 4;
                    int dir = ByteUtil.bytesToInt2(data, offset);
                    NetScene.getInstance().ballMove(id, dir);
                    offset += 4;
                }
            }

            NetScene.getInstance().updatePos();

        }
    }

    public static void Deserialize(int protocol, byte[] data)
    {
        if (protocol == Protocol.Login)
        {
            int id = ByteUtil.bytesToInt2(data, 0);
            SceneController.getInstance().createBallView(id);
        }
        else if (protocol == Protocol.PlayerJoin)
        {
            int id = ByteUtil.bytesToInt2(data, 0);
            SceneController.getInstance().createBallView(id);
        }
        else if (protocol == Protocol.PlayerLeave)
        {
            int id = ByteUtil.bytesToInt2(data, 0);
            SceneController.getInstance().delBallView(id);
        }
        else if (protocol == Protocol.AddPlayer)
        {
            int num = data.Length / 4;
            int offset = 0;
            for (int i = 0; i < num; i++)
            {
                int id = ByteUtil.bytesToInt2(data, offset);
                SceneController.getInstance().createBallView(id);
                offset += 4;
            }

        }
        else if (protocol == Protocol.Move)
        {
            int offset = 0;
            int num = ByteUtil.bytesToInt2(data, offset);
            for (int i = 0; i < num; i++)
            {
                offset += 4;
                int id = ByteUtil.bytesToInt2(data, offset);
                offset += 4;
                int actions = ByteUtil.bytesToInt2(data, offset);
                for (int j = 0; j < actions; j++)
                {
                    offset += 4;
                    int dir = ByteUtil.bytesToInt2(data, offset);
                    Debug.Log("player:" + id + "action:" + dir);
                }
            }
        }
        else if (protocol == Protocol.StartGame)
        {

        }
        else if (protocol == Protocol.Update)
        {
            actionSerialize(data);
        }
    }

    static void actionSynSerialize(byte[] data)
    {
        int offset = 0;
        int actionNums = ByteUtil.bytesToInt2(data, offset);
        offset += 4;
        //Debug.Log("update move");
        if (actionNums >= 0)
        {
            for (int i = 0; i < actionNums; i++)
            {
                int id = ByteUtil.bytesToInt2(data, offset);
                offset += 4;
                int action = ByteUtil.bytesToInt2(data, offset);
                offset += 4;
                switch (action)
                {
                    case ActionType.keyboardMove:
                        int dir = ByteUtil.bytesToInt2(data, offset);
                        NetScene.getInstance().ballMove(id, dir);
                        offset += 4;
                        break;
                    case ActionType.shoot:

                        break;
                    case ActionType.dragMove:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    static void actionSerialize(byte[] data)
    {
        int offset = 0;
        int actionNums = ByteUtil.bytesToInt2(data, offset);
        offset += 4;
        //Debug.Log("update move");
        if (actionNums >= 0)
        {
            for (int i = 0; i < actionNums; i++)
            {
                int id = ByteUtil.bytesToInt2(data, offset);
                offset += 4;
                int action = ByteUtil.bytesToInt2(data, offset);
                offset += 4;
                switch (action)
                {
                    case ActionType.keyboardMove:
                        offset += 4;
                        Debug.Log("move move move");
                        break;
                    case ActionType.shoot:
                        Debug.Log("biu biu biu");
                        break;
                    case ActionType.dragMove:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
