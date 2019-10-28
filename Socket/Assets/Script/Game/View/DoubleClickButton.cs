using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleClickButton : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    EventHandler onPointerDown;
    EventHandler onPointerUp;

    public void addPointerDown(EventHandler down)
    {
        onPointerDown = down;
    }

    public void addPointerUp(EventHandler up)
    {
        onPointerUp = up;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(onPointerDown != null)
        {
            ClickEventArgs args = new ClickEventArgs();
            args.eventData = eventData;
            onPointerDown(this, args);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onPointerUp != null)
        {
            ClickEventArgs args = new ClickEventArgs();
            args.eventData = eventData;
            onPointerUp(this, args);
        }
    }

}

public class ClickEventArgs: EventArgs
{
    public PointerEventData eventData;
}
