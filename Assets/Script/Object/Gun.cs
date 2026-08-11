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
    /// 计算子弹发射方向（玩家指向鼠标地面点，敌人指向玩家位置）
    /// </summary>
    protected Vector3 GetAimDirection(Transform firePoint)
    {
        Vector3 target;
        if (User == "Player")
        {
            target = GameObject.Find("Player(Clone)").GetComponent<Player>().AimTarget;
        }
        else
        {
            target = GameObject.Find("Player(Clone)").transform.position;
        }
        Vector3 direction = target - firePoint.position;
        direction.y = 0;
        return direction.normalized;
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
