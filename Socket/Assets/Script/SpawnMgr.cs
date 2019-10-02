using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMgr : MonoBehaviour
{
    static SpawnMgr _instacne;
    public const int maxSpawn = 10;
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
        Random.InitState(1);
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
        TrashMan.removeRecycleBin(enemy, false);
    }


    public void spawn()
    {
        if (!startSpawn)
        {
            return;
        }
        curSpawn++;
        if(curSpawn >= maxSpawn)
        {
            startSpawn = false;
        }
        int x = Random.Range(100, 200);
        int y = 250;
        for(int i = 0; i < 10; i++)
        {
            GameObject obj = TrashMan.spawn("EnemyBall");
            obj.transform.parent = GameObject.Find("Canvas").transform;
            RectTransform rect = obj.GetComponent<RectTransform>();
            float startY = y - i * 50;
            EnemyBallView enemyBallView = obj.GetComponent<EnemyBallView>();
            enemyBallView.startY = startY;
            rect.anchoredPosition = new Vector2(x, startY);
        }

    }

    void spawnLevel1() 
    {
        int x = Random.Range(100, 200);
        int y = 250;
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = TrashMan.spawn("EnemyBall");
            obj.transform.parent = GameObject.Find("Canvas").transform;
            RectTransform rect = obj.GetComponent<RectTransform>();
            float startY = y - i * 50;
            EnemyBallView enemyBallView = obj.GetComponent<EnemyBallView>();
            enemyBallView.startY = startY;
            rect.anchoredPosition = new Vector2(x, startY);
        }

    }

    void spawnLevel2()
    {
        int x = Random.Range(100, 200);
        int y = 250;
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = TrashMan.spawn("EnemyBall");
            obj.transform.parent = GameObject.Find("Canvas").transform;
            RectTransform rect = obj.GetComponent<RectTransform>();
            float startY = y - i * 50;
            EnemyBallView enemyBallView = obj.GetComponent<EnemyBallView>();
            enemyBallView.startY = startY;
            rect.anchoredPosition = new Vector2(x, startY);
        }

    }

    void spawnLevel3()
    {
        int x = Random.Range(100, 200);
        int y = 250;
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = TrashMan.spawn("EnemyBall");
            obj.transform.parent = GameObject.Find("Canvas").transform;
            RectTransform rect = obj.GetComponent<RectTransform>();
            float startY = y - i * 50;
            EnemyBallView enemyBallView = obj.GetComponent<EnemyBallView>();
            enemyBallView.startY = startY;
            rect.anchoredPosition = new Vector2(x, startY);
        }

    }


}
