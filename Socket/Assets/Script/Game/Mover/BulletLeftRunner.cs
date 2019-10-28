using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLeftRunner:IMove
{
    float speed = 360 * 1.2f * 1.2f;

    public void Move(NetObject obj)
    {
        obj.body.origin.x -= speed;
    }
   
}
