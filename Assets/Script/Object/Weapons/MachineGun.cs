using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class MachineGun : Gun
{
    Transform FirePoint;//�ӵ����ɵ�

    // Start is called before the first frame update
    void Start()
    {
        FirePoint = transform.Find("FirePoint");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Shooting()
    {
        base.Shooting();
        GameObject go = Game.GetInstance().ObjectPool.Take("Amors/Bullet");
        Bullet bullet = go.GetComponent<Bullet>();
        go.transform.position = FirePoint.transform.position;
        go.transform.rotation = Quaternion.LookRotation(GetAimDirection(FirePoint));
        bullet.Load(Level, 0, BaseAttack);
    }
}
