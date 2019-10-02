using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBallView : MonoBehaviour
{
    RectTransform rTran;
    float speed = 0f;
    float angle = 0;
    float max = 0;
    public float startY = 0;
    LeftNormalShooter shooter;
    // Start is called before the first frame update
    void Awake()
    {
        rTran = this.transform.GetComponent<RectTransform>();
        speed = Random.Range(3f, 5f);
        max = Random.Range(8, 13);
        LeftNormalShooter leftNormalShooter = new LeftNormalShooter();
        shooter = leftNormalShooter;
        shooter.shooter = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        angle += Time.deltaTime * speed;
        rTran.anchoredPosition = new Vector2(rTran.anchoredPosition.x, startY + Mathf.Sin(angle) * max);
        if(Time.frameCount % 300 == 0)
        {
            shoot();
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
            TrashMan.despawn(this.gameObject);
            return;
        }
    }
}
