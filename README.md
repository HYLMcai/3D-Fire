# 3D-Fire（第三人称射击游戏）

一款使用 Unity 开发的 3D 第三人称射击（TPS）游戏：玩家在科幻城市中与多波 AI 敌人作战，赚取金钱，在战斗间隙升级属性、更换武器。

---

## 技术栈

- **引擎**：Unity 2022.3.62f3c1（3D）
- **语言**：C#
- **架构**：手写事件驱动 MVC 框架（无第三方 MVC 库）
- **寻路**：Unity AI Navigation（NavMeshAgent）
- **UI**：UGUI
- **存档**：XML

---

## 项目亮点

1. **手写 MVC 游戏框架** —— 纯 C# 手写事件驱动架构，静态 `MVC` 注册表统一管理 `Model` / `View`，事件经 `MVC.SendEvent()` 广播、`View` 按 `EventType` 订阅分发；数据层（Model）与表现层（View）解耦，View 随场景自动注册 / 注销。
2. **通用对象池系统** —— `IReusable` 接口 + 对象池管理器（`PoolManager` / `Pool`），统一接管子弹、激光、敌人等高频生成 / 销毁对象，避免频繁 `Instantiate` / `Destroy` 的 GC 开销。
3. **可扩展实体体系** —— 抽象 `Role` / `Gun` / `Amor` 三层基类，多态支撑 6 种敌人、6 种武器、2 种弹丸（子弹 / 激光），新增单位只需实现子类、不改框架核心。
4. **配置与逻辑分离** —— 数值表（`StaticData`）+ XML 玩家数据（`Utils` 读写）与游戏逻辑解耦，武器 / 敌人 / 弹药配置集中管理。
5. **通用单例封装** —— 泛型 `Singleton<T>` 统一对象池、数据表等全局服务，`Game` 作为入口单例跨场景常驻。

---

## 架构概览

```
Assets/
├── Script/                 # 全部游戏逻辑代码
│   ├── Game.cs             # 入口单例（初始化单例、注册 Model、切场景）
│   ├── MVC/                # 手写 MVC 框架（Model / View / 事件）
│   ├── Object/             # 游戏实体
│   │   ├── Person/         # Role / Player / Enemy 及敌人子类
│   │   ├── Weapons/        # Gun 基类及各武器子类
│   │   └── Amors/          # Amor 基类及 Bullet / Laser
│   ├── Pool/               # 对象池
│   ├── Data/               # 静态数值表（StaticData）+ XML 读写（Utils）
│   ├── Common/             # 工具（Utils 等）
│   └── Singleton/          # 泛型单例封装
├── Resources/              # 自建 Prefab 与玩家数据（保留）
│   └── Prefabs/            # Player / Enemy / Weapons / Amors / UI
├── Scenes/                 # 0.Initial / 1.StartScene / 2.City
├── Animation/              # Mixamo 动画动作资源（保留）
├── QuickOutline/           # 网格描边效果（MIT，保留）
└── Basic Shooter Pack/     # 武器 / 角色模型与动画（保留）
```

**事件流转**：`Game.Start()` → 初始化 `PoolManager` / `StaticData` → 注册 `GameModel` → `LoadScene(1)` 进入主城；`MVC.SendEvent()` 广播给订阅的 View，View 在 `HandleEvent()` 中分发处理。

---

### 运行步骤

1. 用 **Unity 2022.3.62f3c1** 或更高版本打开本项目。
2. 补齐上述素材，或联系仓库所有者获取完整工程。
3. 打开场景 `0.Initial`，点击 Play 运行（会自动切到主城场景）。

---

## 操作方式

- **主城（1.StartScene）**：点击可交互物体（仓库 / 升级 / 开始）进入对应面板。
- **仓库面板**：背包式配装界面，点击装备按钮更换两把武器。
- **战斗（2.City）**：`WASD` 移动、鼠标瞄准转向、`1` / `2` 切换武器、鼠标左键（`Fire1`）开火。

---

## 许可证

本项目代码仅供学习与作品集展示使用。
