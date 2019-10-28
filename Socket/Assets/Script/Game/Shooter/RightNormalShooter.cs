using GameEngine;
using UnityEngine;

public class RightNormalShooter : IShoot
{
    public GameObject shooter;
    public Rectangle shooterPos;
    public void shoot()
    {
        GameObject obj = TrashMan.spawn("Bullet");
        obj.tag = ObjectType.PlayerBullet;
        BulletView bulletView = obj.GetComponent<BulletView>();
        obj.transform.parent = shooter.transform.parent;
        obj.transform.position = shooter.transform.position;
        BulletRightRunner bulletRightRunner = new BulletRightRunner();
        Rectangle body = new Rectangle(shooterPos.origin.x, shooterPos.origin.y, 20, 20);
        NetScene.getInstance().collisionTree.Insert(body);
        bulletView.SetRunner(body);
        bulletView.valid = true;
    }
}
