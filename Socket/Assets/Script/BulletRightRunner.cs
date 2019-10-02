using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRightRunner:IBulletRun
{
    float speed = 360 * 1.2f * 1.2f;
    public RectTransform bullet;

    public Vector2 run()
    {
        if(bullet == null)
        {
            return Vector2.zero;
        }
        return new Vector2(bullet.anchoredPosition.x + Time.deltaTime * speed, bullet.anchoredPosition.y);
    }


    public bool isValid()
    {
        if (bullet == null)
        {
            return false;
        }
        if (bullet.anchoredPosition.x > 800)
        {
            return false;
        }
        return true;
    }
}
