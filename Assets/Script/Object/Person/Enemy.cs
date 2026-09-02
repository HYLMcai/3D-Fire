using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Role
{
    int score = 0;

    private Animator ani;
    protected float distance;
    protected GameObject player;
    protected NavMeshAgent nav;
    protected GameObject enemyModel;
    protected bool FirePrepare { get; set; }
    private bool deathHandled = false;

    public const float FIRE_DISTANCE = 12f;

    public int Score
    {
        get { return this.score; }
    }

    public int Price { get; set; }

    public void Load(EnemyInfo enemyInfo)
    {
        this.score = enemyInfo.Price;
        this.MaxHp = enemyInfo.HP;
        this.CurHp = this.MaxHp;
        this.Price = enemyInfo.Price;
    }

    protected virtual void Start()
    {
        player = GameObject.Find("Player(Clone)").gameObject;
        enemyModel = transform.Find("Model").gameObject;
        nav = transform.GetComponent<NavMeshAgent>();
        nav.updateRotation = false;
        ani = enemyModel.GetComponent<Animator>();
        ani.applyRootMotion = false;
    }

    protected virtual void Update()
    {
        if (!IsDead)
        {
            nav.SetDestination(player.transform.position);
            distance = Vector3.Distance(transform.position, player.transform.position);
            Turn();
        }
        else if (!deathHandled)
        {
            deathHandled = true;
            nav.isStopped = true;
            nav.updatePosition = false;
            ani.applyRootMotion = true;
            StopAllCoroutines();
            // 强制关闭所有子节点武器的开火状态，防止协程中断后 gun.IsFire 仍为 true；同时隐藏枪械，避免死亡后枪浮空
            foreach (var gun in GetComponentsInChildren<Gun>())
            {
                gun.IsFire = false;
                gun.gameObject.SetActive(false);
            }
        }
        AnimationController();
    }

    private void Turn()
    {
        Vector3 turnVector3 = player.transform.position - transform.position;
        Quaternion turnQuaternion = Quaternion.LookRotation(new Vector3(turnVector3.x, 0, turnVector3.z));
        Quaternion targetRotation = turnQuaternion * Quaternion.Euler(0, 50, 0);
        enemyModel.transform.rotation = Quaternion.Lerp(enemyModel.transform.rotation, targetRotation, 2 * Time.deltaTime);
    }

    public override void Take()
    {
        base.Take();
    }
    public override void Back()
    {
        base.Back();
        MaxHp = 0;
        CurHp = 0;
        score = 0;
        deathHandled = false;
        if (ani != null) ani.applyRootMotion = false;
        // 复活时重新显示枪械（死亡时已隐藏，需含未激活对象）
        foreach (var gun in GetComponentsInChildren<Gun>(true))
        {
            gun.gameObject.SetActive(true);
        }
    }

    private void AnimationController()
    {
        ani.SetBool("Dead", IsDead);
    }
}
