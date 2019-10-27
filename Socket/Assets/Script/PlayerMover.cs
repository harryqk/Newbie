using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : IMove
{

    public void Move(NetObject obj)
    {
        if (obj.lastDir == 1)
        {
            obj.body.origin.x += NetObject.sp;
        }
        else if (obj.lastDir == 2)
        {
            obj.body.origin.y -= NetObject.sp;
        }
        else if (obj.lastDir == 3)
        {
            obj.body.origin.x -= NetObject.sp;
        }
        else if (obj.lastDir == 4)
        {
            obj.body.origin.y += NetObject.sp;
        }
        else
        {

        }
    }

}
