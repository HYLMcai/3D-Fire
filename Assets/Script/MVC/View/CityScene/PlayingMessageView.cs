using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayingMessageView : View
{
    private GameModel gm;
    private Text hpText;
    private Text scoreText;
    private PlayerInfo playerInfo;

    public override MViewName Name => MViewName.PlayingMessageView;

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        switch (eventType)
        {
            case EventType.StartGame:
                MPlayerInfoArgs playerInfoArgs = mEventArgs as MPlayerInfoArgs;
                playerInfo = playerInfoArgs.PlayerInfo;
                hpText.text = playerInfo.HP.ToString();
                break;
            case EventType.EnemyDead:
                MEnemyDeadArgs enemyDeadArgs = mEventArgs as MEnemyDeadArgs;
                scoreText.text = (enemyDeadArgs.Score + int.Parse(scoreText.text)).ToString();
                break;
            case EventType.Win:
                gm.SavePlayerMoney(int.Parse(scoreText.text));
                break;
            case EventType.PlayerHPChange:
                MPlayerHPChange HPArgs = mEventArgs as MPlayerHPChange;
                hpText.text = HPArgs.CurHP.ToString();
                break;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        RegisterEvent(EventType.StartGame);
        RegisterEvent(EventType.EnemyDead);
        RegisterEvent(EventType.Win);
        RegisterEvent(EventType.PlayerHPChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            View CitySceneMenuView = GetView<CitySceneMenuView>(MViewName.CitySceneMenuView);
            CitySceneMenuView.SetActive(true);
            SendEvent(EventType.PlayingMenu, null);
        }
    }

    protected override void Initialize()
    {
        base.Initialize();
        gm = GetModel<GameModel>(MModelName.GameModel);

        hpText = transform.Find("HP/HPText").GetComponent<Text>();
        scoreText = transform.Find("Price/ScoreText").GetComponent<Text>();

        scoreText.text = "0";
    }
}
