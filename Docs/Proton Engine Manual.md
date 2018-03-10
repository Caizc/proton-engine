# Proton Engine Manual

# 模块组成

* **UI** - 通用 UI 框架，目前仅具备简单的玩家登录界面和房间界面
* **GamePlay** - 游戏玩法逻辑，目前仅将 Unity Tutorials 中的 [SpaceShooter]() 移植过来作为简单的游戏玩法
* **Corgi Engine** - 2D 游戏工具箱，目前仅整合了 Corgi Engine 的通用工具包
* **Framework** - 通用系统工具箱，目前还未添加任何组件
* **RealSync** - 帧同步框架，目前已将 TrueSync 从其原生依赖的 [PUN](https://www.photonengine.com/zh-CN/PUN) 组件中剥离并整合进来（更多细节可参考[这篇笔记的「TrueSync 从 PUN 基础上剥离」部分](https://github.com/Caizc/cai-knowledge-base/blob/master/hacking/Lockstep%20Synchronization.md)），但后续计划在摸清其实现原理细节后，自定制一个简化版作为替代
* **Networking** - 网络通信框架（横跨客户端和服务端），目前是一个基于 TCP 和异步 Socket 的简易通信模块，后续计划会是一个支持 UDP/KCP 等更高效协议、高可靠、高并发的网络通信框架（类似 PUN）
* **Server** - 服务端程序，目前参考的是《Unity 3D 网络游戏实战》一书中的范例实现，将其从 C# 实现修改为 Java 实现，并做了些许优化。计划中服务端程序不会重点关注，仅为实现客户端的网络同步等提供必要的支持

# 模块功能说明

# 系统运作原理

# 帧同步算法流程

# UML 图表

# 主要 API 说明

# 部署运行操作说明

# 开发计划

* [x] 最小网络框架整合：

    - UI
    - Networking
    - Server

* [ ] 加入帧同步框架：RealSync
* [ ] 将 Corgi Engine demo 改造为网络游戏：Corgi Engine
* [ ] 利用 Corgi Engine 定制平台游戏玩法：GamePlay
* [ ] 完善系统框架，加入商业游戏所需的特性（资源/代码热更新等）：Framework
* [ ] 增强网络模块，使用更高效的网络协议
* [ ] 游戏性能调优
* [ ] 游戏功能完善

---

change log: 

	- 创建（2017-11-14）
	- 更新（2017-11-21）
	- 增加模块简介（2018-03-10）

---

