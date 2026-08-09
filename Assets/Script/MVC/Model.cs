using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MModelName
{
    GameModel,
}

public abstract class Model
{
    //标识
    public abstract MModelName Name { get; }

    public void SendEvent(EventType eventType,MEventArgs mEventArgs)
    {
        //MVC调度中心
        MVC.SendEvent(eventType, mEventArgs);
    }
}
