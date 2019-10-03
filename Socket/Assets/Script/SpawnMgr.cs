using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMgr : MonoBehaviour
{
    static SpawnMgr _instacne;
    public const int maxSpawn = 3;
    public int curSpawn = 0;
    public bool startSpawn = false;
    TrashManRecycleBin enemy;
    Transform enemyBallPrefab;
    public static SpawnMgr getStance()
    {
        if(_instacne == null)
        {
            _instacne = GameObject.Find("SpawnMgr").GetComponent<SpawnMgr>();
        }
        return _instacne;
    }

    public void init()
    {
        enemyBallPrefab = this.transform.Find("EnemyBall");
        enemyBallPrefab.tag = ObjectType.Enemy;
        enemy = new TrashManRecycleBin();
        enemy.prefab = enemyBallPrefab.gameObject;
        enemy.instancesToPreallocate = 200;
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
        if(curSpawn >= maxSpawn)
        {
            return;
        }
        curSpawn++;
        int x = 100 + curSpawn * 50;
        int y = 250;
        for(int i = 0; i < 10; i++)
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
