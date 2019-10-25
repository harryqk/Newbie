using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public class TestQuardTree : MonoBehaviour
{
    List<BallView> balls = new List<BallView>();
    public Transform ballPrefab = null;
    QuardTree quardTree = null;
    public void Start()
    {
        createTree();
    }
    public void createTree()
    {
        TrashManRecycleBin bin = new TrashManRecycleBin();
        bin.instancesToPreallocate = 20;
        bin.prefab = ballPrefab.gameObject;
        ballPrefab.tag = ObjectType.Player;
        TrashMan.manageRecycleBin(bin);
        Rectangle bound = new Rectangle(0, 0, 1136, 640);
        quardTree = new QuardTree(bound, 0);
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                BallView ballView = TrashMan.spawn(ballPrefab.gameObject).GetComponent<BallView>();
                ballView.rect = new Rectangle(i, j, 20, 20);
                balls.Add(ballView);
                quardTree.insert(ballView.rect);
            }
        }

    }

    void Update()
    {
        if (quardTree != null
            && Time.frameCount % 3 == 0)
        {
            quardTree.refresh(null);
        }
    }
}
