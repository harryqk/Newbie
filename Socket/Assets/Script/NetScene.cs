using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NetScene
{
    public static NetScene _instance;

    public static NetScene getInstance()
    {
        if (_instance == null)
        {
            _instance = new NetScene();
        }
        return _instance;
    }

    public List<NetObject> listBall = new List<NetObject>();


    public SocketClient client;
    public Queue<MessageVO> queMes = new Queue<MessageVO>();

    public long frame = 0;

    public int MyId = 0;
    Thread threadU = null;
    public void SetMyId(int id)
    {
        MyId = id;
    }


    public NetObject getBall(int id)
    {
        for (int i = 0; i < listBall.Count; i++)
        {
            NetObject ball = listBall[i];
            if (ball.uid == id)
            {

                return ball;
            }

        }
        return null;
    }


    public void createBall(int id)
    {
        NetObject ball = new NetObject();
        ball.uid = id;
        if (ball.uid == MyId)
        {
            ball.isMe = true;
        }
        listBall.Add(ball);

    }

    public void delBall(int id)
    {
        for (int i = 0; i < listBall.Count; i++)
        {
            NetObject ball = listBall[i];
            if (ball.uid == id)
            {
                listBall.Remove(ball);
                return;
            }

        }
    }

    public void delAllBall()
    {
        listBall.Clear();
    }


    public void ballMove(int id, int dir)
    {
        for (int i = 0; i < listBall.Count; i++)
        {
            NetObject ball = listBall[i];
            if (ball.GetUid() == id)
            {
                ball.SetDir(dir);
            }

        }
    }

    public void updatePos()
    {
        for (int i = 0; i < listBall.Count; i++)
        {
            NetObject ball = listBall[i];
            ball.updatePos();
        }
    }

    public void startGame()
    {
        for (int i = 0; i < listBall.Count; i++)
        {
            NetObject ball = listBall[i];
            ball.SetMove(true);
        }
    }

 
    public void StartTreadUpdateByNetWork()
    {
        threadU = new Thread(updateByNetWork);
        threadU.IsBackground = true;
        threadU.Start();
    }

    public void CloseTreadU()
    {
        if (threadU != null)
        {
            threadU.Abort();
        }
    }

    public void updateByNetWork()
    {
        while (true)
        {
            if (client.getMoveMsg() != null
                && client.getMoveMsg().Count > 0)
            {
                MessageVO data = new MessageVO();
                MessageVO data1 = client.getMoveMsg().Dequeue();
                data.protocol = data1.protocol;
                data.data = data1.data;
                Serializer.SynDeserialize(data1.protocol, data1.data);
                queMes.Enqueue(data);
                frame++;
            }
        }
    }


    public void stopGame()
    {
        frame = 0;
        client.Close();
        CloseTreadU();
        delAllBall();
        queMes.Clear();
    }
}
