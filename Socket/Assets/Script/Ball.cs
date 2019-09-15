using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    RectTransform ball;
    float pass = 0;
    int lastDir = 0;
    const float sec = 5;
    const float speed = 5;
    bool startMove = false;
    int uid = 0;
    bool isMe = false;

    private void Awake()
    {
        ball = this.transform.Find("Ball").GetComponent<RectTransform>();
    }

    private void Update()
    {
        SyncMove();
        Move();
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
        lastDir = dir;
    }

    void SyncMove()
    {
        if (isMe)
        {
            pass += Time.deltaTime;
            if (pass > sec)
            {
                pass = 0;
                int dir = Random.Range(1, 5);
                lastDir = dir;
            }
            byte[] data = SocketUtil.convertByteArrayToSend(Protocol.Move, ByteUtil.intToBytes2(lastDir));
            SceneController.getInstance().client.writeByte(data);
        }
    }

    void Move()
    {
        if (!startMove)
        {
            return;
        }


        if (lastDir == 1)
        {
            ball.anchoredPosition += new Vector2(speed * Time.deltaTime, 0);
        }
        else if (lastDir == 2)
        {
            ball.anchoredPosition += new Vector2(0, -speed * Time.deltaTime);
        }
        else if (lastDir == 3)
        {
            ball.anchoredPosition += new Vector2(-speed * Time.deltaTime, 0);
        }
        else if (lastDir == 4)
        {
            ball.anchoredPosition += new Vector2(0, speed * Time.deltaTime);
        }
        else
        {

        }

    }
}
