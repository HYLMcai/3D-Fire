using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MVC
{
    //将MV注册进来
    private static Dictionary<MModelName, Model> models = new Dictionary<MModelName, Model>();
    private static Dictionary<MViewName, View> views = new Dictionary<MViewName, View>();
    //模型层注册
    public static void RegisterModel(Model model)
    {
        if (models.ContainsKey(model.Name))
        {
            Debug.LogError("模型层重复注册" + model.Name);
            return;
        }
        models.Add(model.Name, model);
    }
    //注册视图层
    public static void RegisterView(View view)
    {
        if (views.ContainsKey(view.Name))
        {
            Debug.LogError("视图层重复注册" + view.Name);
            return;
        }
        views.Add(view.Name, view);
        
    }
    public static void UnRegisterView(View view)
    {
        if (!views.ContainsKey(view.Name))
        {
            Debug.LogError("视图层不存在，不能移除" + view.Name);
            return;
        }
        views.Remove(view.Name);
    }

    public static T GetModel<T>(MModelName name)
        where T:Model
    {
        Model model = null;
        models.TryGetValue(name, out model);
        return model as T;
    }
    public static T GetView<T>(MViewName name) 
        where T : View
    {
        View view = null;
        views.TryGetValue(name, out view);
        return view as T;
    }
    public static void SendEvent(EventType eventType,MEventArgs mEventArgs)
    {
        foreach(View view in views.Values)
        {
            if (view.ContainEventType(eventType))
            {
                view.HandleEvent(eventType, mEventArgs);
            }
        }
    }
}
