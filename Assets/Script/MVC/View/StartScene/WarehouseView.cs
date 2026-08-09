using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseView : View
{
    private Button saveBtn;//保存按钮
    private Button levelBtn;//离开按钮
    private Text weapon1Text;//武器1显示
    private Text weapon2Text;//武器2显示

    private List<GameObject> warehouseWeapon = new List<GameObject>();//仓库武器

    private GameObject content;//预制件父物体
    private Dictionary<string, GunInfo> weapons = new Dictionary<string, GunInfo>();//仓库
    private PlayerInfo playerInfo = new PlayerInfo();//保存玩家信息
    private List<GunInfo> gunInfos;//枪数据

    GameModel gm;//获取模型层

    public override MViewName Name => MViewName.WarehouseView;

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        switch(eventType)
        {
            case EventType.Warehouse:
                MPlayerInfoArgs args = mEventArgs as MPlayerInfoArgs;
                foreach(var guninfo in gunInfos)
                {
                    if (args.PlayerInfo.Weapon_1 == guninfo.PrefabName)
                    {
                        weapon1Text.text = guninfo.PrefabNameCN;
                    }
                    if (args.PlayerInfo.Weapon_2 == guninfo.PrefabName)
                    {
                        weapon2Text.text = guninfo.PrefabNameCN;
                    }
                }
                playerInfo = args.PlayerInfo;
                InitWeaponData();
                break;
        }
    }

    protected override void Initialize()
    {
        base.Initialize();
        gm = GetModel<GameModel>(MModelName.GameModel);

        content = transform.Find("MessageBox/Scroll View/Viewport/Content").gameObject;

        saveBtn = transform.Find("MessageBox/Save").GetComponent<Button>();
        levelBtn = transform.Find("MessageBox/Exit").GetComponent<Button>();
        saveBtn.onClick.AddListener(SavePlayerWeapon);
        levelBtn.onClick.AddListener(Leave);

        weapon1Text = transform.Find("Weapon1/WeaponMessage").GetComponent<Text>();
        weapon2Text = transform.Find("Weapon2/WeaponMessage").GetComponent<Text>();

        gunInfos = gm.GetAllWeaponData();

        for (int i = 0; i < 4; i++)
        {
            warehouseWeapon.Add(content.transform.Find("WarehouseWeapon" + i).gameObject);
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        RegisterEvent(EventType.Warehouse);
        SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EquipWeapon(int index,string name)
    {
        //装备武器
        switch(index)
        {
            case 1:
                weapon1Text.text = name;
                break;
            case 2:
                weapon2Text.text = name;
                break;
            default:
                break;
        }
        InitWeaponData();
    }

    private void SavePlayerWeapon()
    {
        //将玩家装备的武器数据保存
        playerInfo.Weapon_1 = weapon1Text.text;
        playerInfo.Weapon_2 = weapon2Text.text;
        foreach (var guninfo in gunInfos)
        {
            if (weapon1Text.text == guninfo.PrefabNameCN)
            {
                playerInfo.Weapon_1 = guninfo.PrefabName;
                playerInfo.GunID_1 = guninfo.ID;
            }
            if (weapon2Text.text == guninfo.PrefabNameCN)
            {
                playerInfo.Weapon_2 = guninfo.PrefabName;
                playerInfo.GunID_2 = guninfo.ID;
            }
        }
        gm.SavePlayerInfo(playerInfo);
        Debug.Log("保存成功");
    }

    private void Leave()
    {
        //离开该页面
        SetActive(false);
    }

    private void InitWeaponData()
    {
        int i = 0;
        //获取所有枪械信息
        foreach (var gunInfo in gunInfos)
        {
            if (weapons == null) weapons.Add(gunInfo.PrefabName, gunInfo);
            if (gunInfo.PrefabNameCN == weapon1Text.text || gunInfo.PrefabNameCN == weapon2Text.text) continue;
            warehouseWeapon[i].name = gunInfo.PrefabName;
            warehouseWeapon[i].transform.GetComponent<Text>().text = gunInfo.PrefabNameCN;
            warehouseWeapon[i].transform.Find("Weapon1Btn").GetComponent<Button>().onClick.AddListener(() => EquipWeapon(1, gunInfo.PrefabNameCN));
            warehouseWeapon[i].transform.Find("Weapon2Btn").GetComponent<Button>().onClick.AddListener(() => EquipWeapon(2, gunInfo.PrefabNameCN));
            i++;
        }
    }
}
