using UnityEngine;

public class BallView : MonoBehaviour
{
    RectTransform ball;
    float pass = 0;
    const float sec = 3;
    public int uid = 0;
    RightNormalShooter shooter;
    NetObject data = null;
    private void Start()
    {
        ball = this.transform.GetComponent<RectTransform>();
        shooter = new RightNormalShooter();
        shooter.shooter = this.gameObject;
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
                byte[] action = ByteUtil.IntToBytes2(ActionType.keyboardMove);
                byte[] content = ByteUtil.IntToBytes2(dir);
                byte[] send = ByteUtil.BytesCombine(action, content);
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
            NetObject netObject = NetScene.getInstance().GetBall(uid);
            netObject.isDead = true;
            TrashMan.despawn(this.gameObject);
            return;
        }
    }
}
