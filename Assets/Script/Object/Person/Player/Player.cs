using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Role
{

    private GameObject playerModel;//角色模型
    private Animator ani;//角色动画控制器
    private GameObject weapon;//武器挂载点
    private PlayerInfo playerInfo = new PlayerInfo();
    private GameObject usingWeapon;//当前使用的武器
    private GameObject weapon1;//一号武器
    private GameObject weapon2;//二号武器

    //角色控制参数
    private bool isMoving = false;//角色移动状态

    private List<int> GunID = new List<int>();//装备的武器ID列表
    private int Level { get; set; }//伤害等级
    private float Speed { get; set; }//角色移动速度
    private float FireSpeed { get; set; }//角色开火移动速度
    public bool IsFireing { get; set; }//角色开火状态

    // Start is called before the first frame update
    void Start()
    {
        playerModel = transform.Find("Model").gameObject;
        ani = playerModel.GetComponent<Animator>();
        weapon = transform.Find("Model/Weapon").gameObject;
        Utils.LoadPlayer(ref playerInfo);
        Load(playerInfo);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Turn();
        PlayerAnimationController();
        ChangeWeapon();
    }

    public void Load(PlayerInfo playerinfo)
    {
        this.Level = playerinfo.Level;//初始值0 升级+1
        this.MaxHp = playerinfo.HP;//初始值100 升级+10
        this.CurHp = this.MaxHp;
        this.Speed = playerinfo.MoveSpeed;//初始值为3 升级+0.2
        this.FireSpeed = this.Speed - 1;
        this.GunID.Add(playerinfo.GunID_1);
        this.GunID.Add(playerinfo.GunID_2);

        // 通过 GunID 从 StaticData 反查 PrefabName 来加载武器
        string prefabName1 = Game.GetInstance().StaticData.GetGunInfo(playerinfo.GunID_1).PrefabName;
        string prefabName2 = Game.GetInstance().StaticData.GetGunInfo(playerinfo.GunID_2).PrefabName;
        this.weapon1 = Game.GetInstance().ObjectPool.Take("Weapons/" + prefabName1);
        this.weapon2 = Game.GetInstance().ObjectPool.Take("Weapons/" + prefabName2);
        //将武器挂载到Player的武器节点下
        weapon1.transform.parent = weapon.transform;
        weapon2.transform.parent = weapon.transform;
        weapon1.transform.localPosition = Vector3.zero;
        weapon1.transform.localEulerAngles = Vector3.zero;
        weapon2.transform.localPosition = Vector3.zero;
        weapon2.transform.localEulerAngles = Vector3.zero;
        //初始化武器属性
        Gun gun1 = weapon1.GetComponent<Gun>();
        Gun gun2 = weapon2.GetComponent<Gun>();
        gun1.Load(Level, GunID[0], "Player");
        gun2.Load(Level, GunID[1], "Player");
        weapon2.SetActive(false);
        this.usingWeapon = this.weapon1;//默认持有一号位武器
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 playerMove = new Vector3(horizontal, 0, vertical);
        if (Input.GetButton("Fire1"))
        {
            transform.Translate(playerMove * FireSpeed * Time.deltaTime);
            IsFireing = true;
        }
        else
        {
            transform.Translate(playerMove * Speed * Time.deltaTime);
            IsFireing = false;
        }

        isMoving = horizontal != 0 || vertical != 0;
        if (isMoving)
        {
            weapon1.transform.localPosition = new Vector3(0.155f, 0, 0);
            weapon2.transform.localPosition = new Vector3(0.155f, 0, 0);
        }
        else
        {
            weapon1.transform.localPosition = Vector3.zero;
            weapon2.transform.localPosition = Vector3.zero;
        }
    }

    private void Turn()
    {
        Ray pos = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layer = LayerMask.GetMask("Ground");
        RaycastHit hit;
        bool isTouchFloor = Physics.Raycast(pos, out hit, 100, layer);
        if (isTouchFloor)
        {
            Vector3 turnVector3 = hit.point - transform.position;
            Quaternion turnQuaternion = Quaternion.LookRotation(new Vector3(turnVector3.x, 0, turnVector3.z));
            playerModel.transform.rotation = turnQuaternion * Quaternion.Euler(0, 50, 0);
        }
    }

    private void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)&& usingWeapon != weapon1)
        {
            weapon2.SetActive(false);
            weapon1.SetActive(true);
            usingWeapon = weapon1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && usingWeapon != weapon2)
        {
            weapon1.SetActive(false);
            weapon2.SetActive(true);
            usingWeapon = weapon2;
        }
    }

    private void PlayerAnimationController()
    {
        ani.SetBool("Moving", isMoving);
        ani.SetBool("Fireing", IsFireing);
        ani.SetBool("Dead", IsDead);
    }

    public override void Take()
    {
        base.Take();
    }

    public override void Back()
    {
        base.Back();
        this.Level = 0;
        this.MaxHp = 0;
        this.CurHp = 0;
        this.Speed = 0;
        this.GunID.Clear();
        this.FireSpeed = 0;
        this.weapon1 = null;
        this.weapon2 = null;
        this.usingWeapon = null;
        Game.GetInstance().ObjectPool.Back(weapon1);
        Game.GetInstance().ObjectPool.Back(weapon2);
    }
}
