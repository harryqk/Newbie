using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    Button btnStart;
    Button btnClose;
    SocketClient client;
    // Start is called before the first frame update
    void Start()
    {
        btnStart = this.transform.Find("BtnStart").GetComponent<Button>();
        btnClose = this.transform.Find("BtnClose").GetComponent<Button>();
        client = new SocketClient();
        btnStart.onClick.AddListener(delegate () {
            Connect();
        });
        btnClose.onClick.AddListener(delegate () {
            client.Close();
        });
    }

    // Update is called once per frame
    void Connect()
    {
        client.Connect("192.168.1.102", 1500);
    }

    private void Update()
    {
        if(client.getReadMsg() != null
        && client.getReadMsg().Count > 0)
        {
            string s = client.getReadMsg().Dequeue();
            Debug.Log(s);
        }
        }
}
