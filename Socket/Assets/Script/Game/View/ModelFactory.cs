using GameEngine;
using UnityEngine;

public class ModelFactory : MonoBehaviour
{
    static ModelFactory _instacne;
    public const int maxSpawn = 3;
    public int curSpawn = 0;
    public bool startSpawn = false;
    public static ModelFactory getStance()
    {
        if (_instacne == null)
        {
            _instacne = GameObject.Find("ModelFactory").GetComponent<ModelFactory>();
        }
        return _instacne;
    }

    public void init()
    {
        TrashManRecycleBin bin = new TrashManRecycleBin();
        bin.instancesToPreallocate = 20;
        bin.prefab = this.transform.Find("Ball").gameObject;
        bin.prefab.tag = ObjectType.Player;
        TrashMan.manageRecycleBin(bin);

        TrashManRecycleBin binBullet = new TrashManRecycleBin();
        binBullet.instancesToPreallocate = 200;
        binBullet.prefab = this.transform.Find("Bullet").gameObject;
        TrashMan.manageRecycleBin(binBullet);

        TrashManRecycleBin enemy = new TrashManRecycleBin();
        enemy.instancesToPreallocate = 20;
        enemy.prefab = this.transform.Find("EnemyBall").gameObject;
        enemy.prefab.tag = ObjectType.Enemy;
        TrashMan.manageRecycleBin(enemy);
    }

    public void stop()
    {
        startSpawn = false;
        //TrashMan.removeRecycleBin(enemy, false);
    }


    public void spawn()
    {

        //if (!startSpawn)
        //{
        //    return;
        //}
        if (curSpawn >= maxSpawn)
        {
            return;
        }
        curSpawn++;
        int x = 100 + curSpawn * 50;
        int y = 250;
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = TrashMan.spawn("EnemyBall");
            obj.transform.parent = GameObject.Find("Canvas").transform;
            float speed = Random.Range(3, 5);
            float max = Random.Range(8, 13);
            RectTransform rect = obj.GetComponent<RectTransform>();
            float startY = y - i * 50;
            EnemyBallView enemyBallView = obj.GetComponent<EnemyBallView>();
            enemyBallView.speed = speed;
            enemyBallView.max = max;
            enemyBallView.startY = startY;
            enemyBallView.start = true;
            rect.anchoredPosition = new Vector2(x, startY);
        }

    }

    public BallView CreateBall(NetObject netObj)
    {
        GameObject obj = TrashMan.spawn("Ball");
        BallView ball = obj.GetComponent<BallView>();
        RectTransform rect = obj.GetComponent<RectTransform>();
        ball.uid = netObj.uid;
        obj.transform.parent = GameObject.Find("Canvas").transform;
        rect.anchoredPosition = GraphicUtil.ToVector2(netObj.body.center);
        ball.setData(netObj);
        return ball;
    }

    public EnemyBallView CreateEnemyBall(NetObject netObj)
    {
        GameObject obj = TrashMan.spawn("EnemyBall");
        obj.transform.parent = GameObject.Find("Canvas").transform;
        RectTransform rect = obj.GetComponent<RectTransform>();
        EnemyBallView enemyBallView = obj.GetComponent<EnemyBallView>();
        rect.anchoredPosition = GraphicUtil.ToVector2(netObj.body.center);
        return enemyBallView;
    }
}
