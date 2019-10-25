using GameEngine;
using UnityEngine;
using UnityEngine.UI;

public class BallView : MonoBehaviour, ICollision
{
    RectTransform ball;
    float pass = 0;
    const float sec = 3;
    public int uid = 0;
    RightNormalShooter shooter;
    public Rectangle rect;
    NetObject data = null;
    public Text txt = null;
    private void Start()
    {
        ball = this.transform.GetComponent<RectTransform>();
        shooter = new RightNormalShooter();
        shooter.shooter = this.gameObject;
        rect.collision = this;
    }

    public void setData(NetObject obj)
    {
        data = obj;
    }


    public void performShoot()
    {
        shooter.shoot();
    }

    private void Update()
    {
        //SyncMove();
        //updateMove();
        testMove();
    }


    public void SetUid(int id)
    {
        uid = id;
    }


    void SyncMove()
    {
        if (data != null
            && data.GetMove()
            && data.isMe)
        {
            pass += Time.deltaTime;
            if (pass > sec)
            {
                pass = 0;
                int dir = Random.Range(1, 5);
                dir = validateDir(dir);
                byte[] action = ByteUtil.intToBytes2(ActionType.keyboardMove);
                byte[] content = ByteUtil.intToBytes2(dir);
                byte[] send = ByteUtil.bytesCombine(action, content);
                //NetMgr.getInstance().send(Protocol.Update, send);
            }
        }
    }


    void updateMove()
    {
        if (data != null
            && data.GetMove())
        {
            Vector2 moveTo = new Vector2(data.posX, data.posY);
            ball.anchoredPosition = Vector2.Lerp(ball.anchoredPosition, moveTo, data.speed * Time.deltaTime);
            //Debug.Log("pos:" + data.posX + ":" + data.posY + "curPos:" + (int)ball.anchoredPosition.x + ":" + (int)ball.anchoredPosition.y);

        }
    }

    int testSp = 2;

    public void onEnter()
    {
        Invoke("blink", 1);
        Invoke("blink", 2);
    }

    void blink()
    {
        if (txt.text.CompareTo("0") == 0)
        {
            txt.text = "1";
        }
        else
        {
            txt.text = "0";
        }
    }
    void testMove()
    {
        pass += Time.deltaTime;
        if (pass > sec)
        {
            pass = 0;
            int dir = Random.Range(1, 5);
            dir = validateDir(dir);
            if (dir == 1)
            {
                rect.updatePos(rect.origin.x + testSp, rect.origin.y);
            }
            else if (dir == 2)
            {
                rect.updatePos(rect.origin.x, rect.origin.y - testSp);
            }
            else if (dir == 3)
            {
                rect.updatePos(rect.origin.x - testSp, rect.origin.y);
            }
            else if (dir == 4)
            {
                rect.updatePos(rect.origin.x, rect.origin.y + testSp);
            }
            else
            {

            }
        }
        Vector2 vector2 = new Vector2(rect.center.x, rect.center.y);
        ball.anchoredPosition = vector2;
    }

    int validateDir(int dir)
    {
        if (data != null)
        {
            if (data.posX + 100 >= Screen.width / 2)
            {
                return 3;
            }
            if (data.posX - 100 <= -Screen.width / 2)
            {
                return 1;
            }
            if (data.posY + 100 >= Screen.height / 2)
            {
                return 2;
            }
            if (data.posY - 100 <= -Screen.height / 2)
            {
                return 4;
            }
            return dir;
        }
        return dir;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.CompareTo(ObjectType.EnemyBullet) == 0)
        {
            NetObject netObject = NetScene.getInstance().getBall(uid);
            netObject.isDead = true;
            TrashMan.despawn(this.gameObject);
            return;
        }
    }
}
