using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using UnityEngine;

public class Spawn : View
{
    private Player player;

    private GameModel gm;//获取Model
    private Vector3[] spawnPoint = new Vector3[4];//敌人出生点
    private List<EnemyInfo> allEnemysData;//敌人数据

    private int maxEnemyCount = 10;//最大刷怪数
    private float minSpawnInterval = 1.2f;//最小刷怪间隔
    private float maxSpawnInterval = 2.5f;//最大刷怪间隔
    private int currentSpawnEnemyCount = 0;//当前已生成敌人数量
    private int deadEnemyCount = 0;//当前已死亡敌人数量

    public override MViewName Name => MViewName.Spawn;

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        
    }

    protected override void Initialize()
    {
        base.Initialize();
        gm = GetModel<GameModel>(MModelName.GameModel);

        spawnPoint[0] = new Vector3(51, -0.83f, 51);
        spawnPoint[1] = new Vector3(51, -0.83f, 24);
        spawnPoint[2] = new Vector3(-14, -0.83f, 4);
        spawnPoint[3] = new Vector3(-18, -0.83f, 6);

        // 初始化随机数
        allEnemysData = Game.GetInstance().StaticData.GetAllEnemysInfo();
    }

    protected override void Start()
    {
        base.Start();
        SpawnPlayer();
        StartCoroutine(SpawnEnemyLoop());
    }

    void Update()
    {

    }

    private void SpawnPlayer()
    {
        GameObject go = Game.GetInstance().ObjectPool.Take("Player");
        player = go.GetComponent<Player>();
        player.DeadEvent += PlayerDead;
        player.HpEvent += PlayerHpChange;

        MPlayerObjectArgs args = new MPlayerObjectArgs(go);
        SendEvent(EventType.PlayerInit, args);
    }

    IEnumerator SpawnEnemyLoop()
    {
        float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        yield return new WaitForSeconds(spawnInterval);

        //刷怪
        while (currentSpawnEnemyCount < maxEnemyCount)
        {
            //随机取一个敌人
            int randomEnemyID = Random.Range(0, allEnemysData.Count);
            EnemyInfo curEnemyInfo = allEnemysData[randomEnemyID];

            //随机取一个出生点
            int randomSpawnPoint = Random.Range(0, this.spawnPoint.Length);
            Vector3 tempSpawnPoint = this.spawnPoint[randomSpawnPoint];

            //生成敌人对象
            GameObject go = Game.GetInstance().ObjectPool.Take("Enemy/" + curEnemyInfo.PrefabName);

            //敌人配置
            go.transform.position = tempSpawnPoint;
            Enemy enemy = go.GetComponent<Enemy>();
            enemy.Load(curEnemyInfo);
            enemy.DeadEvent += EnemyDead;

            //生成敌人+1
            currentSpawnEnemyCount++;

            yield return new WaitForSeconds(4f);
        }
        
    }

    private void PlayerHpChange(int curHP,int maxHP)
    {
        MPlayerHPChange args = new MPlayerHPChange(curHP);
        SendEvent(EventType.PlayerHPChange, args);
    }

    private void PlayerDead(Role role)
    {
        StartCoroutine(PoolBack(role));
        SendEvent(EventType.Lose, null);
        View LoseView = GetView<LoseView>(MViewName.LoseView);
        LoseView.SetActive(true);
    }

    private void EnemyDead(Role role)
    {
        //回收
        StartCoroutine(PoolBack(role));
        deadEnemyCount++;

        //获取敌人
        Enemy tempEnemy = role as Enemy;

        //发送死亡事件
        MEnemyDeadArgs args = new MEnemyDeadArgs(tempEnemy.Score);
        SendEvent(EventType.EnemyDead, args);

        
        //玩家没死敌人死光的情况
        if (!player.IsDead && deadEnemyCount == maxEnemyCount)
        {
            //游戏胜利
            SendEvent(EventType.Win, null);
            View WinView = GetView<WinView>(MViewName.WinView);
            WinView.SetActive(true);
        }
    }

    IEnumerator PoolBack(Role role)
    {
        yield return new WaitForSeconds(4f);
        Game.GetInstance().ObjectPool.Back(role.gameObject);
    }
}
