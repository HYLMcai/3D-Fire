using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amor : MonoBehaviour,IReusable
{
    protected int Level { get; set; }//伤害等级
    protected int BaseAttack { get; set; }//基础伤害
    protected int Attack { get => BaseAttack + Level; }//实际伤害
    protected float BackTime { get; set; }//回收时间
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Load(int level,int amorID,int baseAttack)
    {
        AmorInfo info = Game.GetInstance().StaticData.GetAmorInfo(amorID);

        this.Level = level;
        this.BaseAttack = baseAttack;
        this.BackTime = info.BackTime;

        StartCoroutine(AmorDestroy());
    }

    IEnumerator AmorDestroy()
    {
        yield return new WaitForSeconds(BackTime);
        Game.GetInstance().ObjectPool.Back(this.gameObject);
    }

    public void HitObject()
    {
        //对象池回收
        Game.GetInstance().ObjectPool.Back(this.gameObject);
        StopCoroutine(AmorDestroy());
    }

    public virtual void Take()
    {
        
    }

    public virtual void Back()
    {
        this.Level = 0;
        this.BaseAttack = 0;
        this.BackTime = 0;
    }
}
