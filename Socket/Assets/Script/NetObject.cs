using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetObject
{
    int lastDir = 0;
    const float sec = 5;
    public float speed = sp * updateFrequency;
    const float sp = 6f;
    bool startMove = false;
    public int uid = 0;
    public bool isMe = false;
    public float posX = 0;
    public float posY = 0;
    const int updateFrequency = 20;
    public bool isDead = false;

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

    public void updatePos()
    {
        if (lastDir == 1)
        {
            posX += sp;
        }
        else if (lastDir == 2)
        {
            posY -= sp;
        }
        else if (lastDir == 3)
        {
            posX -= sp;
        }
        else if (lastDir == 4)
        {
            posY += sp;
        }
        else
        {

        }
    }

    public void SetMove(bool value)
    {
        startMove = value;
    }

    public bool GetMove()
    {
        return startMove;
    }

    public void SetDir(int dir)
    {
        lastDir = dir;
    }

    public int GetDir()
    {
        return lastDir;
    }
}
