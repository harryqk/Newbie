using System.Collections;
using System.Collections.Generic;
using GameEngine;
using UnityEngine;

public class EnemyBallView : MonoBehaviour
{
    RectTransform rTran;
    public float speed = 0f;
    float angle = 0;
    public float max = 0;
    public float startY = 0;
    LeftNormalShooter shooter;
    public bool start = false;
    public Rectangle pos;
    // Start is called before the first frame update
    void Awake()
    {
        rTran = this.transform.GetComponent<RectTransform>();
        LeftNormalShooter leftNormalShooter = new LeftNormalShooter();
        shooter = leftNormalShooter;
        shooter.shooter = this.gameObject;
    }
    long lastFrame = 0;
    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            angle += Time.deltaTime * speed;
            //rTran.anchoredPosition = new Vector2(rTran.anchoredPosition.x, startY + Mathf.Sin(angle) * max);
            rTran.anchoredPosition = new Vector2(pos.center.x, pos.center.y);
            if (lastFrame != NetScene.getInstance().frame
                && NetScene.getInstance().frame % 100 == 0)
            {
                lastFrame = NetScene.getInstance().frame;
                //shoot();
            }
        }
    }

    void shoot()
    {
        shooter.shoot();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.CompareTo(ObjectType.PlayerBullet) == 0)
        {
            start = false;
            TrashMan.despawn(this.gameObject);
            return;
        }
    }
}
