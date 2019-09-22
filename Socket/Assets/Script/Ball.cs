using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    RectTransform ball;
    float pass = 0;
    int lastDir = 0;
    const float sec = 5;
    const float speed = 0.5f;
    bool startMove = false;
    int uid = 0;
    bool isMe = false;
    Queue<int> action = new Queue<int>();

    private void Awake()
    {
        ball = this.transform.GetComponent<RectTransform>();
    }

    private void Update()
    {
        SyncMove();
        SyncFrame();
        updateMove();
    }

    public int GetUid()
    {
        return uid;
    }

    public void SetUid(int id)
    {
        uid = id;
    }

    public void SetIsMe(bool value)
    {
        isMe = value;
    }

    public void SetMove(bool value)
    {
        startMove = value;
    }

    public void SetDir(int dir)
    {
        if(dir != 0)
        {
            lastDir = dir;
            action.Enqueue(dir);
        }
    }

    void SyncMove()
    {
        if (startMove
         && isMe)
        {
            pass += Time.deltaTime;
            if (pass > sec)
            {
                pass = 0;
                int dir = Random.Range(1, 5);
                dir = validateDir(dir);
                byte[] content = ByteUtil.intToBytes2(dir);
                byte[] data = SocketUtil.convertByteArrayToSend(Protocol.Move, content);
                SceneController.getInstance().client.writeByte(data);
            }

        }
    }

    void updateMove()
    {
        if (!startMove)
        {
            return;
        }

        if (lastDir == 1)
        {
            ball.anchoredPosition += new Vector2(speed, 0);
        }
        else if (lastDir == 2)
        {
            ball.anchoredPosition += new Vector2(0, -speed);
        }
        else if (lastDir == 3)
        {
            ball.anchoredPosition += new Vector2(-speed, 0);
        }
        else if (lastDir == 4)
        {
            ball.anchoredPosition += new Vector2(0, speed);
        }
        else
        {

        }
    }

    void SyncFrame()
    {
        if(Time.frameCount % 15 == 0)
        {
            int synced = 0;
            while(action.Count > 0)
            {
                if(synced > 3)
                {
                    break;
                }
                lastDir = action.Dequeue();
                synced++;
            }
        }
    }

    int validateDir(int dir)
    {
        if(ball.anchoredPosition.x >= Screen.width / 2)
        {
            return 3;
        }
        else if(ball.anchoredPosition.x <= -Screen.width / 2)
        {
            return 1;
        }
        else if (ball.anchoredPosition.y >= Screen.height / 2)
        {
            return 2;
        }
        else if (ball.anchoredPosition.y <= -Screen.height / 2)
        {
            return 4;
        }
        else
        {
            return dir;
        }
    }
}
