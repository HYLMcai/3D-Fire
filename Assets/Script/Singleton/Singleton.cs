using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : Singleton<T>
{
    protected Singleton() { }
    private static T instance;
    public static T GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("Singleton");
            instance = go.AddComponent<T>();
            DontDestroyOnLoad(go);
        }
        return instance;
    }
}
