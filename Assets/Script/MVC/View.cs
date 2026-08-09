using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//要显示，继承mono
public enum MViewName
{
    CitySceneMenuView,
    StartSceneMenuView,
    LoseView,
    MenuView,
    WinView,
    PlayingMessageView,
    Spawn,
    LevelUpView,
    StartView,
    WarehouseView,
}
public abstract class View : MonoBehaviour
{
    //标识
    public abstract MViewName Name { get; }

    //注册要关注的事件
    protected List<EventType> attentionEvents = new List<EventType>();

    //注册
    protected void RegisterEvent(EventType eventType)
    {
        if (ContainEventType(eventType))
        {
            return;
        }
        attentionEvents.Add(eventType);
    }
    //取消注册
    protected void UnregisterEvent(EventType eventType)
    {
        if (!ContainEventType(eventType))
        {
            return;
        }
        attentionEvents.Remove(eventType);
    }
    //取消注册所有
    protected void UnregisterAll()
    {
        attentionEvents.Clear();
    }

    public bool ContainEventType(EventType eventType)
    {
        return attentionEvents.Contains(eventType);
    }
    public abstract void HandleEvent(EventType eventType, MEventArgs mEventArgs);

    //获取对象
    protected T GetModel<T>(MModelName name)
        where T:Model
    {
        return MVC.GetModel<T>(name);
    }

    protected T GetView<T>(MViewName name)
        where T : View
    {
        return MVC.GetView<T>(name);
    }

    protected void SendEvent(EventType eventType,MEventArgs args)
    {
        //mvc.sendevent;
        MVC.SendEvent(eventType, args);
    }

    protected virtual void Awake()
    {
        MVC.RegisterView(this);
        Initialize();
    }

    protected virtual void Start()
    {

    }
    protected virtual void OnDestroy()
    {
        MVC.UnRegisterView(this);
    }

    //初始化
    protected virtual void Initialize() { }

    public virtual void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }
}
