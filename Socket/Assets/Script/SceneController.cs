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
    Text txtLog;
    long count = 10000000;
    Transform ballPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        btnConnect = this.transform.Find("BtnConnect").GetComponent<Button>();
        btnClose = this.transform.Find("BtnClose").GetComponent<Button>();
        btnLogin = this.transform.Find("BtnLogin").GetComponent<Button>();
        btnStart = this.transform.Find("BtnStart").GetComponent<Button>();
        txtLog = this.transform.Find("TxtLog").GetComponent<Text>();
        ballPrefab = this.transform.Find("Ball");
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
    }


    public void createBallView(int id)
    {
        Random.InitState(id);
        GameObject obj = Instantiate(ballPrefab.gameObject);
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
                Destroy(ball.gameObject);
            }

        }
    }

    public void delAllBallView()
    {
        for (int i = 0; i < listBallView.Count; i++)
        {
            BallView ball = listBallView[i];
            Destroy(ball.gameObject);
        }
        listBallView.Clear();
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
