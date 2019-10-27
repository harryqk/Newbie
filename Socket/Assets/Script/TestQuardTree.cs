using System.Collections.Generic;
using GameEngine;
using UnityEngine;

public class TestQuardTree : MonoBehaviour
{
    public Transform ballPrefab = null;
    QuardTree quardTree = null;
    public Transform textPrefab = null;
    public void Start()
    {
        createTree();


        TrashManRecycleBin bin = new TrashManRecycleBin();
        bin.instancesToPreallocate = 20;
        bin.prefab = textPrefab.gameObject;
        ballPrefab.tag = ObjectType.Player;
        TrashMan.manageRecycleBin(bin);

    }
    public void createTree()
    {
        TrashManRecycleBin bin = new TrashManRecycleBin();
        bin.instancesToPreallocate = 20;
        bin.prefab = ballPrefab.gameObject;
        ballPrefab.tag = ObjectType.Player;
        TrashMan.manageRecycleBin(bin);
        Rectangle bound = new Rectangle(0, 0, 800, 480);
        quardTree = new QuardTree(bound, 0);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                ObjectView objectView = TrashMan.spawn(ballPrefab.gameObject).GetComponent<ObjectView>();
                Rectangle rect = new Rectangle(200 + i * 50, 120 + j * 50, 20, 20);
                //Rectangle rect = new Rectangle(i * 50, j * 50, 20, 20);
                objectView.setCollisionHandler(rect);
                //objectView.transform.SetParent(this.transform, false);
                Vector3 localPotion = new Vector3(objectView.rect.center.x, objectView.rect.center.y, 0);
                objectView.transform.localPosition = localPotion;
                quardTree.insert(objectView.rect);
            }
        }
        drawQuardTree(quardTree);

    }

    void Update()
    {
        if (quardTree != null
            && Time.frameCount % 1 == 0)
        {
            quardTree.refresh(null);
            quardTree.narrowPhase();

            showText();
        }
        GraphicUtil.drawQuardTree(quardTree);
        if (Input.GetKeyUp(KeyCode.P))
        {
            quardTree.refresh(null);
            //quardTree.narrowPhase();
            showText();
            //Rectangle a = new Rectangle(0, 0, 20, 20);

            //Rectangle b = new Rectangle(19, 20, 20, 20);

            //if(GraphicUtil.isOverlap(a, b))
            //{
            //    Debug.Log("overlap");
            //}
        }
    }

    Dictionary<TextMesh, QuardTree> dicShow = new Dictionary<TextMesh, QuardTree>();
    public void drawQuardTree(QuardTree tree)
    {
    
        GameObject txt = TrashMan.spawn(textPrefab.gameObject);
        txt.transform.position = new Vector3(tree.bound.center.x, tree.bound.center.y, 0);
        txt.GetComponent<TextMesh>().text = tree.objs.Count.ToString();
        dicShow.Add(txt.GetComponent<TextMesh>(), tree);
        if (tree.trees.Count > 0)
        {
            for (int i = 0; i < tree.trees.Count; i++)
            {
                drawQuardTree(tree.trees[i]);
            }
        }
    }

    public void showText()
    {
        foreach(KeyValuePair<TextMesh, QuardTree> kv in dicShow) 
        {

            //kv.Key.text = kv.Value.getInnerCount().ToString();
            kv.Key.text = kv.Value.objs.Count.ToString();
        }

    }
}
