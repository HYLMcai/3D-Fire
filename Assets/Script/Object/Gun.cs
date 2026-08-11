using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Gun : MonoBehaviour,IReusable
{
    private float FireSpeed { get; set; }//����(������)
    protected int BaseAttack { get; set; }//�����˺�
    private string User { get; set; }//����������Ķ���
    protected int Level { get; set; }//�ȼ�
    public bool IsFire { get; set; }//���˿����ж�
    private float time = 0;       //���𿪻�ʱ�����


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
            //���˿����߼�
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

    /// <summary>
    /// 计算从 firePoint 指向鼠标地面命中点的方向
    /// </summary>
    protected Vector3 GetAimDirection(Transform firePoint)
    {
        Player player = GameObject.Find("Player(Clone)").GetComponent<Player>();
        Vector3 target = player.AimTarget;
        return (target - firePoint.position).normalized;
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
