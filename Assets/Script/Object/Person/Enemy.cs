using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Role
{
    int score = 0;//分数

    private Animator ani;//角色动画控制器
    protected float distance;//敌人与玩家间距离
    protected GameObject player;//获取玩家
    protected NavMeshAgent nav;//获取nav组件
    protected GameObject enemyModel;//获取角色模型
    protected bool FirePrepare {  get; set; }//是否进行开火行为

    public const float FIRE_DISTANCE = 12f;//距离判定，够距离就开火

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
    }

    protected virtual void Update()
    {
        nav.SetDestination(player.transform.position);
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (IsDead) nav.updatePosition = false;
        if (!IsDead) Turn();
        AnimationController();
    }

    private void Turn()
    {
        Vector3 turnVector3 = player.transform.position - transform.position;
        Quaternion turnQuaternion = Quaternion.LookRotation(new Vector3(turnVector3.x, 0, turnVector3.z));
        Quaternion targetRotation = turnQuaternion * Quaternion.Euler(0, 50, 0);
        //减慢转向速度
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
    }

    private void AnimationController()
    {
        ani.SetBool("Dead", IsDead);
    }
}
