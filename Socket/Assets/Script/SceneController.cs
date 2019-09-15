using System.Collections;
using System.Collections.Generic;
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

    List<Ball> listBall = new List<Ball>();
    Button btnStart;
    Button btnClose;
    Button btnSend;
    public SocketClient client;
    Text txtLog;
    long count = 10000000;
    Transform ballPrefab;
    int MyId = 0;

    public void SetMyId(int id)
    {
        MyId = id;
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;
        btnStart = this.transform.Find("BtnStart").GetComponent<Button>();
        btnClose = this.transform.Find("BtnClose").GetComponent<Button>();
        btnSend = this.transform.Find("BtnSend").GetComponent<Button>();
        txtLog = this.transform.Find("TxtLog").GetComponent<Text>();
        ballPrefab = this.transform.Find("Ball");
        client = new SocketClient();
        btnStart.onClick.AddListener(delegate () {
            Connect();
        });
        btnClose.onClick.AddListener(delegate () {
            client.Close();
        });
        btnSend.onClick.AddListener(delegate () {
            //client.ClientWrite(Protocol.Move, "client send" + client.getLocalPort());
            client.ClientWrite(Protocol.StartGame, "start game"); ;
        });
    }

    public void createBall(int id)
    {
        Random.InitState(client.getLocalPort());
        GameObject obj = Instantiate(ballPrefab.gameObject);
        Ball ball = obj.GetComponent<Ball>();
        ball.SetUid(id);
        if(id == MyId)
        {
            ball.SetIsMe(true);
        }

        listBall.Add(ball);
        obj.transform.SetParent(this.transform, false);
        obj.transform.localPosition = Vector3.zero;
    }

    public void delBall(int id)
    {
        for (int i = 0; i < listBall.Count; i++)
        {
            Ball ball = listBall[i];
            if (ball.GetUid() == id)
            {
                Destroy(ball.gameObject);
            }

        }
    }

    public void ballMove(int id, int dir)
    {
        for(int i = 0; i < listBall.Count; i++)
        {
            Ball ball = listBall[i];
            if (ball.GetUid() == id) 
            {
                ball.SetDir(dir);
            }

        }
    }

    public void startGame()
    {
        for (int i = 0; i < listBall.Count; i++)
        {
            Ball ball = listBall[i];
            ball.SetMove(true);
        }
    }

    // Update is called once per frame
    void Connect()
    {
        client.Connect("192.168.1.100", 1500);
    }

    private void Update()
    {
        if(client.getReadMsg() != null
        && client.getReadMsg().Count > 0)
        {
            string s = client.getReadMsg().Dequeue();
            txtLog.text = s;
            Debug.Log(s);

        }

        if (client.getMoveMsg() != null
&& client.getMoveMsg().Count > 0)
        {
            MessageVO data = client.getMoveMsg().Dequeue();
            Serializer.Seriaize(data.protocol, data.data);

        }

        if (client.GetLog() != null
&& client.GetLog().Count > 0)
        {
            string s = client.GetLog().Dequeue();
            Debug.Log(s);
        }

        if (client != null)
        {
            count++;

            //client.ClientWrite(Protocol.Move, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" + count.ToString());
        }
    }



}
