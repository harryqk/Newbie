using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetObject
{
    int lastDir = 0;
    const float sec = 5;
    public float speed = (float)sp / 3;
    const int sp = 2;
    bool startMove = false;
    public int uid = 0;
    public bool isMe = false;
    public int posX = 0;
    public int posY = 0;


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
