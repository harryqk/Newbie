using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletView : MonoBehaviour
{
    RectTransform rTran;
    public bool valid = false;
    IBulletRun bulletRunner;
    // Start is called before the first frame update
    void Start()
    {
        rTran = this.transform.GetComponent<RectTransform>();
    }

    public void SetRunner(IBulletRun runner)
    {
        bulletRunner = runner;
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
            rTran.anchoredPosition = bulletRunner.run();
        }
        if(!bulletRunner.isValid())
        {
            valid = false;
            rTran.anchoredPosition = Vector2.zero;
            TrashMan.despawn(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
       //Debug.Log("i am bullet trigger, i hitted");
       if(collision.tag.CompareTo(ObjectType.Player) == 0
            && this.tag.CompareTo(ObjectType.EnemyBullet) == 0)
        {
            TrashMan.despawn(this.gameObject);
            return;
        }

        if (collision.tag.CompareTo(ObjectType.Enemy) == 0
      && this.tag.CompareTo(ObjectType.PlayerBullet) == 0)
        {
            TrashMan.despawn(this.gameObject);
            return;
        }

        if (collision.tag.CompareTo(ObjectType.EnemyBullet) == 0
      && this.tag.CompareTo(ObjectType.PlayerBullet) == 0)
        {
            TrashMan.despawn(this.gameObject);
            return;
        }

        if (collision.tag.CompareTo(ObjectType.PlayerBullet) == 0
&& this.tag.CompareTo(ObjectType.EnemyBullet) == 0)
        {
            TrashMan.despawn(this.gameObject);
            return;
        }
    }
}
