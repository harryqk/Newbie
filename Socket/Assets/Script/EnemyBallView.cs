using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBallView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("i am enemy trigger, i hitted");
        TrashMan.despawn(this.gameObject);
    }
}
