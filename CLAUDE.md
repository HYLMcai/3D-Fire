# CLAUDE.md

此文件为 Claude Code（claude.ai/code）在此仓库中工作时提供指导。

## 项目概述

Unity 2022.3.62f3c1 3D 第三人称射击游戏。玩家在科幻城市中与多波 AI 敌人作战，赚取金钱用于在战斗间隙升级属性与更换武器。

## 构建与开发

- **编辑器版本：** 2022.3.62f3c1（见 `ProjectSettings/ProjectVersion.txt`）
- 在 IDE 中打开 `3D-Fire.sln` 进行 C# 编辑，或直接在 Unity Editor 中打开项目文件夹。
- 没有自定义构建脚本；使用 Unity Editor 的 **File → Build Settings** 进行构建。
- 此项目不存在自动化测试。

## 架构

### 自定义 MVC 框架（`Assets/Script/MVC/`）

此项目使用的是手写 MVC 系统，而非第三方库：

- **`MVC`** —— 静态类，作为中心注册表。持有 `Model` 字典（键为 `MModelName` 枚举）和 `View` 字典（键为 `MViewName` 枚举）。View 在 `Awake()` 时注册自身，在 `OnDestroy()` 时注销。`MVC.SendEvent()` 将事件广播给所有已注册且订阅了该事件的 View。
- **`Model`** —— 抽象基类。Model 通过 `MVC.SendEvent()` 发送事件，但不接收事件。目前仅有一个 Model：`GameModel`。
- **`View`** —— 抽象 `MonoBehaviour` 基类。每个 View 需要覆写 `Name`（返回其在注册表中的枚举键值）、`HandleEvent()`（事件分发）以及 `Initialize()`（初始化设置 —— 由 `Awake` 调用）。View 在其 `Start()` 中调用 `RegisterEvent(EventType)` 订阅事件。
- **事件** —— `EventType` 枚举列出所有事件类型。`MEventArgs` 有多个带类型的子类（`MPlayerInfoArgs`、`MEnemyDeadArgs`、`MPlayerHPChange`、`MPlayerObjectArgs`），携带数据负载。

### 游戏入口（`Assets/Script/Game.cs`）

`Game` 是一个 `MonoBehaviour` 单例（手动实现，未使用 `Singleton<T>`）。在 `Start()` 中依次：
1. 初始化 `PoolManager` 和 `StaticData` 单例
2. 向 MVC 注册 `GameModel`
3. 调用 `LoadScene(1)` → 进入 StartScene

该 GameObject 被标记为 `DontDestroyOnLoad`，因此跨场景持久存在。

### 场景流程

1. **Scene 0（`0.Initial.unity`）** —— 启动场景，仅包含 `Game` GameObject
2. **Scene 1（`1.StartScene.unity`）** —— 主城场景。可交互物体（仓库、升级、开始）通过 `StartView` 响应鼠标点击。含武器更换面板（`WarehouseView`）和属性升级面板（`LevelUpView`）。
3. **Scene 2（`2.City.unity`）** —— 战斗场景。敌人通过 `Spawn` View 分批生成。玩家使用 WASD 移动 + 鼠标瞄准。

场景切换直接使用 `SceneManager.LoadScene(index)`（无异步加载）。

### 对象与继承体系

所有战斗对象实现 `IReusable` 接口以支持对象池：

- **`Role`** —— 所有有血量对象的基类。管理 `CurHp`/`MaxHp`（带钳位 setter），触发 `HpEvent` 和 `DeadEvent` 这两个 C# 事件。实现 `TakeDamge()` 方法。
  - **`Player`**（`Assets/Script/Object/Person/Player/Player.cs`） —— WASD 移动，鼠标射线转向（读取 "Ground" 层），武器切换（1/2 键），通过 `Input.GetButton("Fire1")` 开火。属性通过 `Utils.LoadPlayer()` 从 XML 读取。
  - **`Enemy`**（`Assets/Script/Object/Person/Enemy.cs`） —— 基于 NavMeshAgent 向玩家寻路。基于距离的开火触发：当与玩家距离 ≤ `FIRE_DISTANCE`（12 单位）时进入开火协程。每个子类（`LaserGunEnemy`、`MachineGunEnemy` 等）实例化各自的 `Gun` 并实现对应的 `GunnerFire()` 或等效协程模式。
- **`Gun`**（`Assets/Script/Object/Gun.cs`） —— 武器基类。跟踪 `FireSpeed`、`BaseAttack`、`Level`。在 `Update()` 中，当玩家按住 Fire1 或敌人 `IsFire` 标志为 true 且冷却完毕时开火。调用虚方法 `Shooting()`，子类覆写以从 `FirePoint` 变换生成弹丸。
  - 每个武器子类（如 `LaserGun`、`MachineGun`）覆写 `Shooting()`，从对象池实例化对应的 `Amor` 类型。
- **`Amor`**（弹药/弹丸，`Assets/Script/Object/Amor.cs`） —— 弹丸基类。`Load()` 时启动协程，在 `BackTime` 秒后将对象归还对象池。`Attack` 属性 = `BaseAttack + Level`。`HitObject()` 取消自动归还，立即回收。
  - **`Bullet`** —— 以 30 单位/秒的速度前移，碰撞任意 "Person" 标签物体时触发伤害。
  - **`Laser`** —— 使用 `LineRenderer` 实现即时命中扫描（100 单位光束），碰撞任意 "Person" 标签物体时触发伤害。

### 对象池（`Assets/Script/Pool/`）

- **`IReusable`** —— 接口，定义 `Take()`（对象被取出时调用）和 `Back()`（对象归还池时调用；在此重置状态）。
- **`Pool`** —— 包装单个预制体。从 `Resources/Prefabs/{path}` 加载。维护非活跃列表和活跃列表。`Take()` 时实例化或复用已有对象，调用 `IReusable.Take()`。`Back()` 时停用对象并调用 `IReusable.Back()`。
- **`PoolManager`** —— 单例（`Singleton<PoolManager>`）。使用 `PoolManager.GetInstance().Take("子目录/预制体名")` 生成对象，使用 `PoolManager.GetInstance().Back(gameObject)` 归还对象。首次使用时自动创建池。`Clear()` 销毁所有池内实例 —— 在场景切换时调用。

### 数据层（`Assets/Script/Data/`）

- **`StaticData`** —— 单例，持有硬编码的 `AmorInfo`、`EnemyInfo`、`GunInfo` 字典。定义了所有敌人类型（ID、血量、赏金、职业）、武器类型（ID、基础攻击、射速）以及弹药类型（ID、自动回收时间）。通过 `StaticData.GetInstance()` 访问。
- **`PlayerInfo`** —— 纯数据类：Level、HP、GunID_1/2、MoveSpeed、Money。
- **`Utils`** —— 静态工具类。`LoadPlayer(ref PlayerInfo)` 通过 `XmlDocument` 从 `Resources/PlayerData/PlayerData.xml` 读取玩家数据。`SavePlayerInfo(PlayerInfo)` 写回同一 XML 文件。

### 单例模式（`Assets/Script/Singleton/Singleton.cs`）

泛型 `Singleton<T>`，约束 `T : Singleton<T>`。创建一个名为 "Singleton" 的新 GameObject，添加组件并标记 `DontDestroyOnLoad`。`PoolManager` 和 `StaticData` 使用此模式。注意：`Game` 类未使用此泛型单例 —— 它自行实现了更简单的单例。

### 关键 Unity 层与标签

- **"Ground" 层** —— 玩家鼠标射线瞄准/转向所用。
- **"Interaction" 层** —— `StartView` 检测主城中可点击物体（仓库、升级、开始）所用。
- **"Person" 标签** —— 弹丸检测角色命中所用。

### Resources 路径

所有运行时加载的预制体位于 `Assets/Resources/Prefabs/`。子目录：`Weapons/`、`Enemy/`、`Amors/`，以及根目录下的 `Player` 预制体。PoolManager 自动在所有路径前加上 `"Prefabs/"` 前缀。

### 第三方资源

- **QuickOutline**（`Assets/QuickOutline/`） —— Chris Nolet 的网格描边效果。`StartView` 用于可交互物体的悬停高亮。
- **AI Navigation**（`com.unity.ai.navigation@1.1.7`） —— Unity 的 NavMesh 寻路系统，用于敌人 AI 移动。
- **PolygonSciFiCity** —— 场景美术资源包。
- **Basic Shooter Pack** —— 武器/角色模型与动画资源包。
