using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Gun : MonoBehaviour,IReusable
{
    private float FireSpeed { get; set; }//射速（冷却时间）
    protected int BaseAttack { get; set; }//基础伤害
    private string User { get; set; }//使用者（Player 或 Enemy）
    protected int Level { get; set; }//等级
    public bool IsFire { get; set; }//开火开关
    public Vector3? AimTargetOverride { get; set; }//敌人锁定目标点，优先级高于实时玩家位置
    private float time = 0;       //冷却计时


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        time += Time.deltaTime;
        if (User == "Player")
        {
            if (Input.GetButton("Fire1") && time >= FireSpeed)
            {
                Shooting();
                time = 0;
            }
        }
        else
        {
            //敌人开火逻辑
            if (time >= FireSpeed && IsFire)
            {
                Shooting();
                time = 0;
            }
        }
    }

    public void Load(int level,int gunID,string user)
    {
        GunInfo info = Game.GetInstance().StaticData.GetGunInfo(gunID);

        this.FireSpeed = info.FireSpeed;
        this.BaseAttack = info.BaseAttack;
        this.Level = level;
        this.User = user;
    }

    public virtual void Shooting()
    {
        //子类重写
    }

    /// <summary>
    /// 计算子弹发射方向（玩家指向鼠标地面点，敌人优先用锁定目标否则指向玩家位置）
    /// </summary>
    protected Vector3 GetAimDirection(Transform firePoint)
    {
        Vector3 target;
        if (User == "Player")
        {
            target = GameObject.Find("Player(Clone)").GetComponent<Player>().AimTarget;
        }
        else
        {
            // 敌人优先使用锁定目标，否则用实时玩家位置
            target = AimTargetOverride ?? GameObject.Find("Player(Clone)").transform.position;
            AimTargetOverride = null;
        }
        Vector3 direction = target - firePoint.position;
        direction.y = 0;
        return direction.normalized;
    }

    public void Back()
    {
        this.FireSpeed = 0; ;
        this.BaseAttack = 0; ;
    }

    public void Take()
    {

    }
}
