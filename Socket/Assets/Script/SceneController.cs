using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController _instance;

    public static SceneController getInstance()
    {
        if(_instance == null)
        {
            _instance = GameObject.Find("Canvas").GetComponent<SceneController>();
        }
        return _instance;
    }

    List<BallView> listBallView = new List<BallView>();
    Button btnConnect;
    Button btnClose;
    Button btnLogin;
    Button btnStart;
    Button btnShoot;
    Text txtLog;
    long count = 10000000;
    Transform ballPrefab;
    Transform bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        btnConnect = this.transform.Find("BtnConnect").GetComponent<Button>();
        btnClose = this.transform.Find("BtnClose").GetComponent<Button>();
        btnLogin = this.transform.Find("BtnLogin").GetComponent<Button>();
        btnStart = this.transform.Find("BtnStart").GetComponent<Button>();
        btnShoot = this.transform.Find("BtnShoot").GetComponent<Button>();
        txtLog = this.transform.Find("TxtLog").GetComponent<Text>();
        ballPrefab = this.transform.Find("Ball");
        bulletPrefab = this.transform.Find("Bullet");
        NetScene.getInstance().client = new SocketClient();
        btnConnect.onClick.AddListener(delegate () {
            Connect();
        });
        btnClose.onClick.AddListener(delegate () {
            NetScene.getInstance().stopGame();
            stopGame();
        });
        btnLogin.onClick.AddListener(delegate () {
            Debug.Log("click login");
            //client.ClientWrite(Protocol.Move, "client send" + client.getLocalPort());
            NetScene.getInstance().client.ClientWrite(Protocol.Login, "login"); ;
        });
        btnStart.onClick.AddListener(delegate () {
            //client.ClientWrite(Protocol.Move, "client send" + client.getLocalPort());
            Debug.Log("click start");
            NetScene.getInstance().client.ClientWrite(Protocol.StartGame, "start game"); ;
        });
        btnShoot.onClick.AddListener(delegate () {
            //client.ClientWrite(Protocol.Move, "client send" + client.getLocalPort());
            Debug.Log("click shoot");
            NetMgr.getInstance().send(Protocol.Update, ByteUtil.intToBytes2(ActionType.shoot));

            //byte[] action = ByteUtil.intToBytes2(ActionType.keyboardMove);
            //byte[] content = ByteUtil.intToBytes2(3);
            //byte[] send = ByteUtil.bytesCombine(action, content);
            //NetMgr.getInstance().send(Protocol.Update, send);
        });
        Screen.fullScreenMode = FullScreenMode.Windowed;
        TrashManRecycleBin bin = new TrashManRecycleBin();
        bin.instancesToPreallocate = 20;
        bin.prefab = ballPrefab.gameObject;
        TrashMan.manageRecycleBin(bin);

        TrashManRecycleBin binBullet = new TrashManRecycleBin();
        binBullet.instancesToPreallocate = 200;
        binBullet.prefab = bulletPrefab.gameObject;
        TrashMan.manageRecycleBin(binBullet);
    }


    public void createBallView(int id)
    {
        Random.InitState(id);
        GameObject obj = TrashMan.spawn(ballPrefab.gameObject);
        BallView ball = obj.GetComponent<BallView>();
        listBallView.Add(ball);
        ball.uid = id;
        obj.transform.SetParent(this.transform, false);
        obj.transform.localPosition = Vector3.zero;
    }

    public void delBallView(int id)
    {
        for (int i = 0; i < listBallView.Count; i++)
        {
            BallView ball = listBallView[i];
            if (ball.uid == id)
            {
                listBallView.Remove(ball);
                TrashMan.despawn(ball.gameObject);
                return;
            }

        }
    }

    public void delAllBallView()
    {
        for (int i = 0; i < listBallView.Count; i++)
        {
            BallView ball = listBallView[i];
            TrashMan.despawn(ball.gameObject);
        }
        listBallView.Clear();
    }

    public void shoot(int id) 
    {
        for (int i = 0; i < listBallView.Count; i++)
        {
            BallView ball = listBallView[i];
            if (ball.uid == id)
            {
                ball.performShoot();
                return;
            }

        }
    }


    // Update is called once per frame
    void Connect()
    {
        if (!NetScene.getInstance().client.isConnected())
        {
            NetScene.getInstance().client.Connect("192.168.1.100", 1500);
            NetScene.getInstance().StartTreadUpdateByNetWork();
        }
        else
        {
            txtLog.text = "已建立连接";
        }
    }

    private void Update()
    {

        if(NetScene.getInstance().queMes.Count > 0)
        {
            MessageVO vo = NetScene.getInstance().queMes.Dequeue();
            Serializer.Deserialize(vo.protocol, vo.data);
        }

        if (NetScene.getInstance().client.getReadMsg() != null
        && NetScene.getInstance().client.getReadMsg().Count > 0)
        {
            string s = NetScene.getInstance().client.getReadMsg().Dequeue();
            txtLog.text = s;
            Debug.Log(s);

        }

        if (NetScene.getInstance().client.GetLog() != null
&& NetScene.getInstance().client.GetLog().Count > 0)
        {
            string s = NetScene.getInstance().client.GetLog().Dequeue();
            Debug.Log(s);
        }

        if (NetScene.getInstance().client != null)
        {
            count++;

            //client.ClientWrite(Protocol.Move, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" + count.ToString());
        }
    }


    void stopGame()
    {
        delAllBallView();
    }

}
