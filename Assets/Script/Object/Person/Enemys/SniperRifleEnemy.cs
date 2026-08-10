using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SniperRifleEnemy : Enemy
{
    private SniperRifle gun;//��ȡ����

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        gun = transform.Find("Model/Weapon/SniperRifle").GetComponent<SniperRifle>();
        gun.Load(0, 4, "Enemy");
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (distance <= FIRE_DISTANCE && FirePrepare == false && !IsDead)
        {
            FirePrepare = true;
            StartCoroutine(SpinerFire());
        }
    }

    IEnumerator SpinerFire()
    {
        //����ֹͣ�ƶ���������׼
        nav.isStopped = true;
        yield return new WaitForSeconds(3f);
        //����
        gun.IsFire = true;
        yield return new WaitForSeconds(0.1f);
        gun.IsFire = false;
        FirePrepare= false;
        // 死亡后不再恢复移动，防止覆盖死亡动画的冻结效果
        if (!IsDead) nav.isStopped = false;
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
