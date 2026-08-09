using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartView : View
{
    GameModel gm;

    private bool IsTouchObject;//检测是否停在可互动物品上
    private int layer;
    private GameObject selectObject;//鼠标停的物体
    private PlayerInfo playerInfo = new PlayerInfo();

    public override MViewName Name => MViewName.StartView;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        CastRay();
        if (IsPointerOverUI()) return;
        OutLine();
        if (Input.GetMouseButton(0))
        {
            switch (selectObject.name)
            {
                case "Warehouse":
                    playerInfo = gm.GetPlayerInfo();
                    MPlayerInfoArgs argsWarehouse = new MPlayerInfoArgs(playerInfo);
                    SendEvent(EventType.Warehouse, argsWarehouse);
                    //打开仓库
                    View WarehouseView = GetView<WarehouseView>(MViewName.WarehouseView);
                    WarehouseView.SetActive(true);
                    break;
                case "LevelUp":
                    playerInfo = gm.GetPlayerInfo();
                    MPlayerInfoArgs argsLevelUp = new MPlayerInfoArgs(playerInfo);
                    SendEvent(EventType.LevelUp, argsLevelUp);
                    //打开升级页面
                    View LevelUpView = GetView<LevelUpView>(MViewName.LevelUpView);
                    LevelUpView.SetActive(true);
                    break;
                case "Start":
                    playerInfo = gm.GetPlayerInfo();
                    MPlayerInfoArgs argsPlayingMessage = new MPlayerInfoArgs(playerInfo);
                    SendEvent(EventType.StartGame, argsPlayingMessage);
                    Game.GetInstance().LoadScene(2);
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            View StartSceneMenuView = GetView<StartSceneMenuView>(MViewName.StartSceneMenuView);
            StartSceneMenuView.SetActive(true);
        }
    }

    private RaycastHit CastRay()
    {
        Ray pos = Camera.main.ScreenPointToRay(Input.mousePosition);
        layer = LayerMask.GetMask("Interaction");
        RaycastHit hit;
        IsTouchObject = Physics.Raycast(pos, out hit, 100, layer);
        return hit;
    }

    private void OutLine()
    {
        if (selectObject == null)
        {
            if (IsTouchObject)
            {
                selectObject = CastRay().collider.gameObject;
                selectObject.GetComponent<Outline>().enabled = true;
            }
        }
        else if (IsTouchObject == false || selectObject != CastRay().collider.gameObject)
        {
            selectObject.GetComponent<Outline>().enabled = false;
            selectObject = null;
        }
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        // 获取所有被射线击中的UI元素
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        throw new System.NotImplementedException();
    }

    protected override void Initialize()
    {
        base.Initialize();
        gm = GetModel<GameModel>(MModelName.GameModel);
    }
}
