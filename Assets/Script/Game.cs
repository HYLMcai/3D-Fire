using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private static Game instance;
    public static Game GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }

    public PoolManager ObjectPool;
    public StaticData StaticData;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //获取单例
        ObjectPool = PoolManager.GetInstance();
        StaticData = StaticData.GetInstance();
        //注册GameModel
        GameModel gameModel = new GameModel();
        MVC.RegisterModel(gameModel);
        ////进入游戏
        LoadScene(1);
    }

    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }
}
