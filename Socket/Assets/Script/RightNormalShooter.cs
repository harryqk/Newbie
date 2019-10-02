using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightNormalShooter : IShoot
{
    public GameObject shooter;
    public void shoot()
    {
        GameObject obj = TrashMan.spawn("Bullet");
        obj.tag = ObjectType.PlayerBullet;
        BulletView bulletView = obj.GetComponent<BulletView>();
        obj.transform.parent = shooter.transform.parent;
        obj.transform.position = shooter.transform.position;
        BulletRightRunner bulletRightRunner = new BulletRightRunner();
        bulletRightRunner.bullet = bulletView.GetComponent<RectTransform>();
        bulletView.SetRunner(bulletRightRunner);
        bulletView.valid = true;
    }
}
