using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StaticData : Singleton<StaticData>
{
    Dictionary<int, AmorInfo> Amors = new Dictionary<int, AmorInfo>();
    Dictionary<int, EnemyInfo> Enemys = new Dictionary<int, EnemyInfo>();
    Dictionary<int, GunInfo> Guns = new Dictionary<int, GunInfo>();

    private void Start()
    {
        Initial();
    }

    protected void Initial()
    {
        InitAmors();
        InitEnemys();
        InitGuns();
    }

    void InitAmors()
    {
        Amors.Add(0, new AmorInfo() { ID = 0, PrefabName = "Bullet", BackTime = 5f });
        Amors.Add(1, new AmorInfo() { ID = 1, PrefabName = "Laser", BackTime = 0.1f });
    }

    void InitEnemys()
    {
        Enemys.Add(0, new EnemyInfo() { ID = 0, PrefabName = "LaserGunEnemy", Level = 0, HP = 15, Price = 25, Career = "Gunner" });
        Enemys.Add(1, new EnemyInfo() { ID = 1, PrefabName = "LaserSniperRifleEnemy", Level = 0, HP = 10, Price = 30, Career = "Sniper" });
        Enemys.Add(2, new EnemyInfo() { ID = 2, PrefabName = "MachineGunEnemy", Level = 0, HP = 30, Price = 45, Career = "Gunner" });
        Enemys.Add(3, new EnemyInfo() { ID = 3, PrefabName = "RifleEnemy", Level = 0, HP = 20, Price = 20, Career = "Gunner" });
        Enemys.Add(4, new EnemyInfo() { ID = 4, PrefabName = "SniperRifleEnemy", Level = 0, HP = 10, Price = 50, Career = "Sniper" });
        Enemys.Add(5, new EnemyInfo() { ID = 5, PrefabName = "SubmachineGunEnemy", Level = 0, HP = 10, Price = 15, Career = "Gunner" });
    }

    void InitGuns()
    {
        Guns.Add(0, new GunInfo() { ID = 0, PrefabName = "LaserGun", BaseAttack = 4, FireSpeed = 0.3f, PrefabNameCN="¿ÿ…‰«π" });
        Guns.Add(1, new GunInfo() { ID = 1, PrefabName = "LaserSniperRifle", BaseAttack = 8, FireSpeed = 1.6f, PrefabNameCN = "¿ÿ…‰æ—ª˜«π" });
        Guns.Add(2, new GunInfo() { ID = 2, PrefabName = "MachineGun", BaseAttack = 5, FireSpeed = 0.15f, PrefabNameCN = "ª˙«π" });
        Guns.Add(3, new GunInfo() { ID = 3, PrefabName = "Rifle", BaseAttack = 5, FireSpeed = 0.25f, PrefabNameCN = "≤Ω«π" });
        Guns.Add(4, new GunInfo() { ID = 4, PrefabName = "SniperRifle", BaseAttack = 15, FireSpeed = 2.2f, PrefabNameCN = "æ—ª˜«π" });
        Guns.Add(5, new GunInfo() { ID = 5, PrefabName = "SubmachineGun", BaseAttack = 1, FireSpeed = 0.1f, PrefabNameCN = "≥Â∑Ê«π" });
    }

    public List<EnemyInfo> GetAllEnemysInfo()
    {
        return Enemys.Values.ToList<EnemyInfo>();
    }

    public List<GunInfo> GetAllGunsInfo()
    {
        return Guns.Values.ToList<GunInfo>();
    }

    public AmorInfo GetAmorInfo(int amorID)
    {
        return Amors[amorID];
    }
    
    public EnemyInfo GetEnemyInfo(int enemyID)
    {
        return Enemys[enemyID];
    }

    public GunInfo GetGunInfo(int gunID)
    {
        return Guns[gunID];
    }
}
