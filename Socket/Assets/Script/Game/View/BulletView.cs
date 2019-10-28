using System.Collections;
using System.Collections.Generic;
using GameEngine;
using UnityEngine;

public class BulletView : MonoBehaviour, ICollision
{
    RectTransform rTran;
    public bool valid = false;
    public bool collision = false;
    Rectangle bulletRunner;
    // Start is called before the first frame update
    void Start()
    {
        rTran = this.transform.GetComponent<RectTransform>();
    }

    public void SetRunner(Rectangle runner)
    {
        bulletRunner = runner;
    }

    public void onCollision()
    {
        collision = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(bulletRunner == null)
        {
            return;
        }
        if (valid)
        {
            rTran.anchoredPosition = new Vector2(bulletRunner.origin.x, bulletRunner.origin.y);
        }
        if(bulletRunner.origin.x > 800)
        {
            despawn();
        }

        if (collision) 
        {
            despawn();
        }


    }

    void despawn()
    {
        valid = false;
        collision = false;
        rTran.anchoredPosition = Vector2.zero;
        TrashMan.despawn(this.gameObject);
    }
}
