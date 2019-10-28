using GameEngine;
using System.Collections;
using UnityEngine;

public class ObjectView : MonoBehaviour, ICollision
{
    public Material img;
    float pass = 0;
    int testSp = 1;
    float sec = 3;
    int lastDir = 0;
    public Rectangle rect;
    Transform view;
    public void Awake()
    {
        view = this.transform;
        img = this.transform.GetComponent<MeshRenderer>().material;
        Random.InitState(System.DateTime.Now.Second);
    }

    public void setCollisionHandler(Rectangle r)
    {
        rect = r;
        rect.collision = this;
    }

    void Update()
    {
        testMove();
    }
    public void onCollision()
    {

        //StopCoroutine(blink());
        StartCoroutine(blink());
    }

    IEnumerator blink()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i % 2 == 0)
            {
                img.color = Color.white;
            }
            else
            {
                img.color = Color.red;
            }
            yield return new WaitForSeconds(0.2f);
        }
        img.color = Color.white;
    }

    void testMove()
    {
        if (rect == null) return;
        pass += Time.deltaTime;
        if (pass > sec)
        {
            pass = 0;
            lastDir = Random.Range(1, 5);
            lastDir = validateDir(lastDir);
        }


        if (lastDir == 1)
        {
            rect.UpdatePos(rect.origin.x + testSp, rect.origin.y);
        }
        else if (lastDir == 2)
        {
            rect.UpdatePos(rect.origin.x, rect.origin.y - testSp);
        }
        else if (lastDir == 3)
        {
            rect.UpdatePos(rect.origin.x - testSp, rect.origin.y);
        }
        else if (lastDir == 4)
        {
            rect.UpdatePos(rect.origin.x, rect.origin.y + testSp);
        }
        else
        {

        }
        Vector2 vector2 = new Vector2(rect.center.x, rect.center.y);
        view.position = vector2;

        //Rectangle bound = new Rectangle(0, 0, 800, 480);
        //Rectangle[] arr = rect.slice(bound);
        //for(int i = 0; i < arr.Length; i++) 
        //{
        //    GraphicUtil.drawRect(arr[i]); 
        //}

    }

    int validateDir(int dir)
    {
        if (rect == null)
        {
            return dir;
        }
        if (rect.center.x + 100 >= Screen.width / 2)
        {
            return 3;
        }
        if (rect.center.x - 100 <= -Screen.width / 2)
        {
            return 1;
        }
        if (rect.center.y + 100 >= Screen.height / 2)
        {
            return 2;
        }
        if (rect.center.y - 100 <= -Screen.height / 2)
        {
            return 4;
        }
        return dir;

    }
}
