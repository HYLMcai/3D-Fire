using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CitySceneMenuView : View
{
    private Button continueGameBtn;//回到游戏按钮
    private Button backHomeBtn;//回到主菜单按钮
    private Button exitGameBtn;//回到游戏按钮

    public override MViewName Name => MViewName.CitySceneMenuView;

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        switch(eventType)
        {
            case EventType.PlayingMenu:
                Time.timeScale = 0f;
                break;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        RegisterEvent(EventType.PlayingMenu);
        SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Initialize()
    {
        base.Initialize();
        continueGameBtn = transform.Find("MenuBox/BackGame").GetComponent<Button>();
        backHomeBtn = transform.Find("MenuBox/BackHome").GetComponent<Button>();
        exitGameBtn = transform.Find("MenuBox/ExitGame").GetComponent<Button>();

        continueGameBtn.onClick.AddListener(ContinueGame);
        backHomeBtn.onClick.AddListener(BackHome);
        exitGameBtn.onClick.AddListener(ExitGame);
    }

    private void ContinueGame()
    {
        Time.timeScale = 1.0f;
        SetActive(false);
    }

    private void BackHome()
    {
        Time.timeScale = 1.0f;
        Game.GetInstance().ObjectPool.Clear();
        Game.GetInstance().LoadScene(1);
        
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
