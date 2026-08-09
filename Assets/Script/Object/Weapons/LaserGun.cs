using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Gun
{
    Transform FirePoint;//子弹生成点

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
        GameObject go = Game.GetInstance().ObjectPool.Take("Amors/Laser");
        Laser laser = go.GetComponent<Laser>();
        go.transform.position = FirePoint.transform.position;
        go.transform.rotation = FirePoint.transform.rotation;
        laser.Load(Level, 1, BaseAttack);
    }
}
