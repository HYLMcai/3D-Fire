using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseView : View
{
    private Button saveBtn;               //保存按钮
    private Button levelBtn;              //退出按钮
    private Text weapon1Text;             //槽位1武器显示
    private Text weapon2Text;             //槽位2武器显示

    private GameObject content;           //滚动列表的 Content 节点
    private GameObject weaponSlotPrefab;  //武器格子预制体（Resources 加载）
    private List<GunInfo> gunInfos;       //全部武器数据列表
    private PlayerInfo playerInfo;        //当前玩家数据
    private int equippedGunID_1;          //当前装备的武器1 ID
    private int equippedGunID_2;          //当前装备的武器2 ID

    GameModel gm;

    public override MViewName Name => MViewName.WarehouseView;

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        switch (eventType)
        {
            case EventType.Warehouse:
                MPlayerInfoArgs args = mEventArgs as MPlayerInfoArgs;
                playerInfo = args.PlayerInfo;
                equippedGunID_1 = playerInfo.GunID_1;
                equippedGunID_2 = playerInfo.GunID_2;
                // 更新槽位显示文本
                UpdateSlotDisplay();
                // 刷新武器格子列表
                RefreshGrid();
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

        // 通过 Resources 加载武器格子预制体，无需手动拖拽
        weaponSlotPrefab = Resources.Load<GameObject>("Prefabs/UI/WeaponSlot");

        // 获取全部 6 把武器的数据
        gunInfos = gm.GetAllWeaponData();
    }

    protected override void Start()
    {
        base.Start();
        RegisterEvent(EventType.Warehouse);
        SetActive(false);
    }

    /// <summary>
    /// 刷新武器格子列表：清空 Content 下所有子物体，遍历全部武器 Instantiate 生成格子
    /// </summary>
    private void RefreshGrid()
    {
        // 清空旧的格子
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        // 为每把武器创建一个格子
        foreach (var gunInfo in gunInfos)
        {
            GameObject slot = Instantiate(weaponSlotPrefab, content.transform);

            // 设置武器名称
            Text nameText = slot.transform.Find("WeaponName").GetComponent<Text>();
            nameText.text = gunInfo.PrefabNameCN;

            // 设置攻击力
            Text attackText = slot.transform.Find("AttackText").GetComponent<Text>();
            attackText.text = "攻击力：" + gunInfo.BaseAttack;

            // 设置射速
            Text fireSpeedText = slot.transform.Find("FireSpeedText").GetComponent<Text>();
            fireSpeedText.text = "射速：" + gunInfo.FireSpeed;

            // 获取两个装备按钮
            Button slot1Btn = slot.transform.Find("EquipToSlot1Btn").GetComponent<Button>();
            Button slot2Btn = slot.transform.Find("EquipToSlot2Btn").GetComponent<Button>();

            // 用局部变量捕获，避免 Lambda 闭包陷阱
            var capturedGunInfo = gunInfo;

            // 先清除旧监听器，再添加新监听器，防止泄漏
            slot1Btn.onClick.RemoveAllListeners();
            slot1Btn.onClick.AddListener(() => EquipWeapon(1, capturedGunInfo));

            slot2Btn.onClick.RemoveAllListeners();
            slot2Btn.onClick.AddListener(() => EquipWeapon(2, capturedGunInfo));

            // 判断该武器是否已被装备：已装备的格子置灰并禁用按钮
            bool isEquipped = (gunInfo.ID == equippedGunID_1 || gunInfo.ID == equippedGunID_2);
            if (isEquipped)
            {
                // 装备到槽位1和槽位2的按钮均禁用
                slot1Btn.interactable = false;
                slot2Btn.interactable = false;
                // 调整格子透明度表示已装备
                CanvasGroup cg = slot.GetComponent<CanvasGroup>();
                if (cg == null) cg = slot.AddComponent<CanvasGroup>();
                cg.alpha = 0.5f;
            }
        }
    }

    /// <summary>
    /// 将武器装备到指定槽位
    /// </summary>
    private void EquipWeapon(int slotIndex, GunInfo gunInfo)
    {
        switch (slotIndex)
        {
            case 1:
                equippedGunID_1 = gunInfo.ID;
                break;
            case 2:
                equippedGunID_2 = gunInfo.ID;
                break;
        }
        // 更新显示并刷新列表
        UpdateSlotDisplay();
        RefreshGrid();
    }

    /// <summary>
    /// 根据已装备的 GunID 更新槽位显示文本
    /// </summary>
    private void UpdateSlotDisplay()
    {
        foreach (var gunInfo in gunInfos)
        {
            if (gunInfo.ID == equippedGunID_1)
                weapon1Text.text = gunInfo.PrefabNameCN;
            if (gunInfo.ID == equippedGunID_2)
                weapon2Text.text = gunInfo.PrefabNameCN;
        }
    }

    /// <summary>
    /// 保存装备选择：将 GunID 写入 PlayerInfo 并持久化到 XML
    /// </summary>
    private void SavePlayerWeapon()
    {
        playerInfo.GunID_1 = equippedGunID_1;
        playerInfo.GunID_2 = equippedGunID_2;
        gm.SavePlayerInfo(playerInfo);
        Debug.Log("武器保存成功");
    }

    /// <summary>
    /// 关闭背包面板
    /// </summary>
    private void Leave()
    {
        SetActive(false);
    }
}
