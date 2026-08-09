using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Gun : MonoBehaviour,IReusable
{
    private float FireSpeed { get; set; }//射速(开火间隔)
    protected int BaseAttack { get; set; }//基础伤害
    private string User { get; set; }//用这把武器的对象
    protected int Level { get; set; }//等级
    public bool IsFire { get; set; }//敌人开火判断
    private float time = 0;       //负责开火时间计算


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

    public void Back()
    {
        this.FireSpeed = 0; ;
        this.BaseAttack = 0; ;
    }

    public void Take()
    {
        
    }
}
