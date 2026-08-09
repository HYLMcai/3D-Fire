using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinView : View
{
    private Button backHome;

    public override MViewName Name => MViewName.WinView;

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        switch (eventType)
        {
            case EventType.Win:
                StartCoroutine(EndAnimation());
                break;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        RegisterEvent(EventType.Win);
        SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Initialize()
    {
        base.Initialize();
        backHome = transform.Find("MenuBox/BackHome").GetComponent<Button>();

        backHome.onClick.AddListener(BackHome);
    }

    private void BackHome()
    {
        Time.timeScale = 1.0f;
        Game.GetInstance().ObjectPool.Clear();
        Game.GetInstance().LoadScene(1);

    }

    IEnumerator EndAnimation()
    {
        yield return new WaitForSeconds(4f);
        Time.timeScale = 0f;
    }
}
