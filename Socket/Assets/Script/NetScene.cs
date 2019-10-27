using System.Collections;
using System.Collections.Generic;
using System.Threading;
using GameEngine;
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


    LinkedList<NetObject> listBullet = new LinkedList<NetObject>();

    LinkedList<NetObject> listEnemy = new LinkedList<NetObject>();

    public SocketClient client;
    public Queue<MessageVO> queMes = new Queue<MessageVO>();
    Rectangle bound = new Rectangle(0, 0, 800, 480);
    public QuardTree collisionTree;
    public long frame = 0;

    public int MyId = 0;
    Thread threadU = null;
    public void SetMyId(int id)
    {
        MyId = id;
    }


    public NetObject GetBall(int id)
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


    public void CreateBall(int id)
    {
        NetObject ball = new NetObject();
        ball.uid = id;
        if (ball.uid == MyId)
        {
            ball.isMe = true;
        }
        ball.isDead = false;
        listBall.Add(ball);

    }

    public void DelBall(int id)
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

    public void DelAllBall()
    {
        listBall.Clear();
    }


    public void BallMove(int id, int dir)
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

    public void UpdatePos()
    {
        for (int i = 0; i < listBall.Count; i++)
        {
            NetObject ball = listBall[i];
            ball.PerformMove();
        }
    }


    public void UpdateBulletPos()
    {
        List<NetObject> remove = new List<NetObject>();
        foreach(NetObject obj in listBullet)
        {
            obj.PerformMove();
            if(!GraphicUtil.isInner(obj.body, bound))
            {
                remove.Add(obj);
            }
        }

        for(int i = 0; i < remove.Count; i++) 
        {
            collisionTree.remove(remove[i].body);
            listBullet.Remove(remove[i]);
        }
        remove.Clear();


    }

    public void UpdateEnemyPos()
    {
        List<NetObject> remove = new List<NetObject>();
        foreach (NetObject obj in listEnemy)
        {
            obj.PerformMove();
            if (!GraphicUtil.isInner(obj.body, bound))
            {
                remove.Add(obj);
            }
        }

        for (int i = 0; i < remove.Count; i++)
        {
            collisionTree.remove(remove[i].body);
            listEnemy.Remove(remove[i]);
        }
        remove.Clear();
    }

    public void AddBullet(NetObject obj)
    {
        listBullet.AddLast(obj);
    }

    public void AddEnemy(NetObject obj)
    {
        listEnemy.AddLast(obj);
    }

    public void StartGame()
    {
        for (int i = 0; i < listBall.Count; i++)
        {
            NetObject ball = listBall[i];
            ball.SetMove(true);
        }

        if(collisionTree == null) 
        {
            collisionTree = new QuardTree(bound, 0); 
        }
        collisionTree.clear();

    }

    public void StartTreadUpdateByNetWork()
    {
        threadU = new Thread(UpdateByNetWork);
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

    public void UpdateByNetWork()
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


    public void StopGame()
    {
        frame = 0;
        client.Close();
        CloseTreadU();
        DelAllBall();
        queMes.Clear();
    }

    public void Shoot()
    {
        NetObject netObject = GetBall(MyId);
        if (!netObject.isDead) 
        {
            NetMgr.getInstance().send(Protocol.Update, ByteUtil.intToBytes2(ActionType.shoot));
        }

    }

    public bool IsDeath(int id)
    {
        NetObject netObject = GetBall(id);
        if (netObject.isDead)
        {
            return true;
        }
        return false;
    }
}
