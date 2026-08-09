using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour, IReusable
{
    public event Action<int, int> HpEvent;
    public event Action<Role> DeadEvent;
    int curHp;
    int maxHp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int CurHp
    {
        get { return this.curHp; }
        set
        {
            value = Mathf.Clamp(value, 0, maxHp);
            if (value == this.curHp)
            {
                return;
            }
            //¥Ê÷µ
            this.curHp = value;
            if (HpEvent != null)
            {
                HpEvent(this.curHp, this.maxHp);
            }

            if (this.curHp <= 0)
            {
                if (DeadEvent != null)
                {
                    DeadEvent(this);
                }
            }
        }
    }

    public int MaxHp
    {
        get { return this.maxHp; }
        set
        {
            value = Mathf.Clamp(value, 0, int.MaxValue);
            if (value == this.maxHp)
            {
                return;
            }
            //¥Ê÷µ
            this.maxHp = value;
            if (HpEvent != null)
            {
                HpEvent(this.curHp, this.maxHp);
            }
        }
    }

    //À¿Õˆ≈–∂œ
    public bool IsDead { get { return this.curHp <= 0; } }

    //ø€—™¥¶¿Ì
    public virtual void TakeDamge(int hit)
    {
        if (IsDead)
        {
            return;
        }
        this.CurHp -= hit; 
    }

    public virtual void Back()
    {
        DeadEvent = null;
        HpEvent = null;

        curHp = 0;
        maxHp = 0;
    }

    public virtual void Take()
    {
        
    }
}
