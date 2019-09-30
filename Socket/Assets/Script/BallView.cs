﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallView : MonoBehaviour
{
    RectTransform ball;
    float pass = 0;
    const float sec = 5;
    public int uid = 0;


    NetObject data = null;
    private void Awake()
    {
        ball = this.transform.GetComponent<RectTransform>();
        StartCoroutine(getData());
    }

    IEnumerator getData()
    {
        while (data == null)
        {
            data = NetScene.getInstance().getBall(uid);
            yield return 1;
        }
    }

    private void Update()
    {
        SyncMove();
        updateMove();
    }


    public void SetUid(int id)
    {
        uid = id;
    }


    void SyncMove()
    {
        if (data != null
            && data.GetMove()
            && data.isMe)
        {
            pass += Time.deltaTime;
            if (pass > sec)
            {
                pass = 0;
                int dir = Random.Range(1, 5);
                dir = validateDir(dir);
                byte[] content = ByteUtil.intToBytes2(dir);
                byte[] data = SocketUtil.convertByteArrayToSend(Protocol.Update, content);
                NetScene.getInstance().client.writeByte(data);
            }
        }
    }


    void updateMove()
    {
        if (data != null
            && data.GetMove())
        {
            Vector2 moveTo = new Vector2(data.posX, data.posY);
            ball.anchoredPosition = Vector2.Lerp(ball.anchoredPosition, moveTo, data.speed);
        }
    }


    int validateDir(int dir)
    {
        if (data != null)
        {
            if (data.posX >= Screen.width / 2)
            {
                return 3;
            }
            if (data.posX <= -Screen.width / 2)
            {
                return 1;
            }
            if (data.posY >= Screen.height / 2)
            {
                return 2;
            }
            if (data.posY <= -Screen.height / 2)
            {
                return 4;
            }
            return dir;
        }
        return dir;

    }
}