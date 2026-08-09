using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bullet : Amor
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * 30f * Time.deltaTime);
    }

    public void Load(int level,int baseAttack)
    {
        Load(level, 0, baseAttack);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        //¹¥»÷ÅÐ¶¨
        if (other.tag == "Person")
        {
            Role role = other.gameObject.GetComponent<Role>();
            role.TakeDamge(Attack);
        }
        HitObject();
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
