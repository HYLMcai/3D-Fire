using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Amor
{
    LineRenderer laserAmor;
    //搞个敌人表，记录是否打过
    
    // Start is called before the first frame update
    void Start()
    {
        laserAmor = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        laserAmor.SetPosition(0, transform.position);
        laserAmor.SetPosition(1, transform.position + transform.forward * 100);
    }

    public void Load(int level, int baseAttack)
    {
        Load(level, 1, baseAttack);
    }

    private void OnTriggerEnter(Collider other)
    {
        //攻击判定
        if (other.tag == "Person")
        {
            Role role = other.gameObject.GetComponent<Role>();
            role.TakeDamge(Attack);
        }
    }

    public override void Take()
    {
        base.Take();
    }

    public override void Back()
    {
        base.Back();
    }
}
