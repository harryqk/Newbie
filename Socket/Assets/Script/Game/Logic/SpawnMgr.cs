using UnityEngine;

public class SpawnMgr
{
    static SpawnMgr _instacne;
    public const int maxSpawn = 3;
    public int curSpawn = 0;
    public bool startSpawn = false;
    public static SpawnMgr getStance()
    {
        if (_instacne == null)
        {
            _instacne = new SpawnMgr();
        }
        return _instacne;
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
}
