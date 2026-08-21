using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SubmachineGunEnemy : Enemy
{
    private SubmachineGun gun;//获取武器

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        gun = transform.Find("Model/Weapon/SubmachineGun").GetComponent<SubmachineGun>();
        gun.Load(0, 5, "Enemy");
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
        // 锁定当前玩家位置，0.1 秒后朝锁定位置开火
        gun.AimTargetOverride = player.transform.position;
        yield return new WaitForSeconds(0.1f);
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
