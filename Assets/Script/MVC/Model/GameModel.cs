using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameModel : Model
{
    public override MModelName Name => MModelName.GameModel;

    public const int LevelUpMoney = 100;//每次升级消耗金币
    public const int MaxDamageLevel = 5;//伤害等级上限
    public const int MaxHP = 200;//血量最大值
    public const float MaxSpeed = 6f;//移速最大值
    public const int BaseHp = 100;// 血量初始值
    public const float BaseSpeed = 4f;//移速初始值
    public const int MaxEnemyNum = 20;//敌人数量

    private PlayerInfo tempPlayerInfo;//暂存玩家数据

    public void SavePlayerMoney(int money)
    {
        tempPlayerInfo = new PlayerInfo();
        Utils.LoadPlayer(ref tempPlayerInfo);
        tempPlayerInfo.Money += money;
        SavePlayerInfo(tempPlayerInfo);
    }

    //保存，更新玩家数据
    public void SavePlayerInfo(PlayerInfo playerInfo)
    {
        Utils.SavePlayerInfo(playerInfo);
    }

    //获取当前玩家数据
    public PlayerInfo GetPlayerInfo()
    {
        tempPlayerInfo = new PlayerInfo();
        Utils.LoadPlayer(ref tempPlayerInfo);
        return tempPlayerInfo;
    }

    //升级伤害
    public bool DamageLevelUP(PlayerInfo playerInfo)
    {
        if (playerInfo.Level >= MaxDamageLevel || playerInfo.Money < LevelUpMoney) return false;

        playerInfo.Level++;
        playerInfo.Money -= LevelUpMoney;
        Utils.SavePlayerInfo(playerInfo);
        return true;
    }

    //升级血量
    public bool HPLevelUP(PlayerInfo playerInfo)
    {
        if (playerInfo.HP >= MaxHP || playerInfo.Money < LevelUpMoney) return false;

        playerInfo.HP += 10;
        playerInfo.Money -= LevelUpMoney;
        Utils.SavePlayerInfo(playerInfo);
        return true;
    }

    //升级移动速度
    public bool SpeedLevelUP(PlayerInfo playerInfo)
    {
        if (playerInfo.MoveSpeed > MaxSpeed || playerInfo.Money < LevelUpMoney) return false;

        playerInfo.MoveSpeed += 0.2f;
        playerInfo.Money -= LevelUpMoney;
        Utils.SavePlayerInfo(playerInfo);
        return true;
    }

    //获取所有枪械数据
    public List<GunInfo> GetAllWeaponData()
    {
        return Game.GetInstance().StaticData.GetAllGunsInfo();
    }

    public List<EnemyInfo> GetEnemyData()
    {
        return Game.GetInstance().StaticData.GetAllEnemysInfo();
    }
}
