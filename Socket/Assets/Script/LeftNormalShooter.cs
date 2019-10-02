using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftNormalShooter : IShoot
{
    public GameObject shooter;
    public void shoot()
    {
        GameObject obj = TrashMan.spawn("Bullet");
        obj.tag = ObjectType.EnemyBullet;
        BulletView bulletView = obj.GetComponent<BulletView>();
        obj.transform.parent = shooter.transform.parent;
        obj.transform.position = shooter.transform.position;
        BulletLeftRunner bulletLeftRunner = new BulletLeftRunner();
        bulletLeftRunner.bullet = bulletView.GetComponent<RectTransform>();
        bulletView.SetRunner(bulletLeftRunner);
        bulletView.valid = true;
    }
}
