using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneMenuView : View
{
    private Button exitBtn;//离开按钮
    private Button continueBtn;//继续按钮

    public override MViewName Name => MViewName.StartSceneMenuView;

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Initialize()
    {
        base.Initialize();

        exitBtn = transform.Find("MenuBox/Yes").GetComponent<Button>();
        continueBtn = transform.Find("MenuBox/No").GetComponent<Button>();

        exitBtn.onClick.AddListener(ExitGame);
        continueBtn.onClick.AddListener(ContinueGame);
    }

    private void ContinueGame()
    {
        SetActive(false);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

}
