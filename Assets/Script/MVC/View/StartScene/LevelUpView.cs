using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpView : View
{
    private Button levelBtn;//离开按钮

    private Button damageLevel;//伤害等级
    private Button hpLevel;//血量等级
    private Button speedLevel;//速度等级

    private Text damegeMessage;//伤害等级
    private Text hpMessage;//血量等级
    private Text speedMessage;//速度等级

    private Text money;//钱

    private PlayerInfo playerInfo = new PlayerInfo();//保存玩家信息
    
    GameModel gm;//获取模型层处理数据


    public override MViewName Name => MViewName.LevelUpView;

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        switch (eventType)
        {
            case EventType.LevelUp:
                MPlayerInfoArgs args = mEventArgs as MPlayerInfoArgs;
                playerInfo = args.PlayerInfo;
                Refresh();
                break;
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        gm = GetModel<GameModel>(MModelName.GameModel);

        levelBtn = transform.Find("MessageBox/Exit").GetComponent<Button>();
        levelBtn.onClick.AddListener(Leave);

        damageLevel = transform.Find("MessageBox/Damage/LevelUPBtn").GetComponent<Button>();
        hpLevel = transform.Find("MessageBox/HP/LevelUPBtn").GetComponent<Button>();
        speedLevel = transform.Find("MessageBox/Speed/LevelUPBtn").GetComponent<Button>();

        damageLevel.onClick.AddListener(() => LevelUpBtn("damage"));
        hpLevel.onClick.AddListener(() => LevelUpBtn("hp"));
        speedLevel.onClick.AddListener(() => LevelUpBtn("speed"));

        damegeMessage = transform.Find("MessageBox/Damage/LV").GetComponent<Text>();
        hpMessage = transform.Find("MessageBox/HP/LV").GetComponent<Text>();
        speedMessage = transform.Find("MessageBox/Speed/LV").GetComponent<Text>();

        money = transform.Find("MessageBox/Money").gameObject.GetComponent<Text>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        RegisterEvent(EventType.LevelUp);
        SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LevelUpBtn(string index)
    {
        bool isSuccess = false;
        switch (index)
        {
            case "damage":
                isSuccess = gm.DamageLevelUP(playerInfo);
                break;
            case "hp":
                isSuccess = gm.HPLevelUP(playerInfo);
                break;
            case "speed":
                isSuccess = gm.SpeedLevelUP(playerInfo);
                break;
            default:
                break;
        }
        if (isSuccess) Refresh();
        playerInfo = gm.GetPlayerInfo();
        RefreshBtnState();
    }

    private void Refresh()
    {
        money.text = "资金：" + playerInfo.Money.ToString();
        damegeMessage.text = "LV." + playerInfo.Level;
        hpMessage.text = "LV." + (playerInfo.HP - 100) / 10;
        speedMessage.text = "LV." + (int)Mathf.Round((playerInfo.MoveSpeed - 4) * 5);
    }

    private void Leave()
    {
        //离开该页面
        SetActive(false);
    }

    private void RefreshBtnState()
    {
        var playerInfo = gm.GetPlayerInfo();
        if (playerInfo == null) return;

        damageLevel.interactable = playerInfo.Level < GameModel.MaxDamageLevel;
        damageLevel.transform.Find("Text").GetComponent<Text>().text = damageLevel.interactable ? "升级" : "Max";

        hpLevel.interactable = playerInfo.HP < GameModel.MaxHP;
        hpLevel.transform.Find("Text").GetComponent<Text>().text = hpLevel.interactable ? "升级" : "Max";

        speedLevel.interactable = playerInfo.MoveSpeed < GameModel.MaxSpeed;
        speedLevel.transform.Find("Text").GetComponent<Text>().text = speedLevel.interactable ? "升级" : "Max";
    }
}
