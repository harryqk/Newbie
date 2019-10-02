using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMgr : MonoBehaviour
{
    static SpawnMgr _instacne;

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
        TrashManRecycleBin bin = new TrashManRecycleBin();
        bin.prefab = enemyBallPrefab.gameObject;
        bin.instancesToPreallocate = 200;
        TrashMan.manageRecycleBin(bin);
    }


    public void spawn()
    {
        int x = Random.Range(100, 200);
        int y = 300;
        for(int i = 0; i < 10; i++)
        {
            GameObject obj = TrashMan.spawn("EnemyBall");
            obj.transform.parent = GameObject.Find("Canvas").transform;
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(x, y - i * 50);
        }

    }
}
