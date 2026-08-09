using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserGunEnemy : Enemy
{
    private LaserGun gun;//ªÒ»°Œ‰∆˜

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        gun = transform.Find("Model/Weapon/LaserGun").GetComponent<LaserGun>();
        gun.Load(0, 0, "Enemy");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (distance <= FIRE_DISTANCE && FirePrepare == false && !IsDead)
        {
            FirePrepare = true;
            StartCoroutine(GunnerFire());
        }
    }

    IEnumerator GunnerFire()
    {
        gun.IsFire = true;
        yield return new WaitForSeconds(3f);
        gun.IsFire = false;
        yield return new WaitForSeconds(3f);
        FirePrepare = false;
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
