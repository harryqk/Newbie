using GameEngine;
using UnityEngine;

public class LeftNormalShooter : IShoot
{
    public GameObject shooter;
    public Rectangle shooterPos;
    public void shoot()
    {
        GameObject obj = TrashMan.spawn("Bullet");
        obj.tag = ObjectType.EnemyBullet;
        BulletView bulletView = obj.GetComponent<BulletView>();
        obj.transform.parent = shooter.transform.parent;
        obj.transform.position = shooter.transform.position;
        BulletLeftRunner bulletLeftRunner = new BulletLeftRunner();
        Rectangle body = new Rectangle(shooterPos.origin.x, shooterPos.origin.y, 20, 20);
        NetScene.getInstance().collisionTree.Insert(body);
        bulletView.SetRunner(body);
        bulletView.valid = true;
    }
}
