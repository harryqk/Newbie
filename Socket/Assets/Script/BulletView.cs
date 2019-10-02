using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletView : MonoBehaviour
{
    float speed = 360 * 1.2f * 1.2f;
    RectTransform rTran;
    public bool valid = false;
    // Start is called before the first frame update
    void Start()
    {
        rTran = this.transform.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (valid)
        {
            rTran.anchoredPosition = new Vector2(rTran.anchoredPosition.x + Time.deltaTime * speed, rTran.anchoredPosition.y);
        }
        if(rTran.anchoredPosition.x > 800)
        {
            valid = false;
            rTran.anchoredPosition = Vector2.zero;
            TrashMan.despawn(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("i am bullet trigger, i hitted");
        TrashMan.despawn(this.gameObject);
    }
}
