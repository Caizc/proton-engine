using System.Collections;
using Proton;
using UnityEngine;
using UnityEngine.SceneManagement;
using UIManager = Proton.UIManager;

/// <summary>
/// 游戏控制器（Main）
/// </summary>
public class GameController : MonoBehaviour
{
    private RealSyncManager _realSyncManager;

    void Start()
    {
        DontDestroyOnLoad(this);
        Application.runInBackground = true;

        // 加载游戏
        Reload();

        Debug.Log("=== The Game is RUNNING... ===");
    }

    /// <summary>
    /// 重新加载游戏
    /// </summary>
    public void Reload()
    {
        // 在协程中执行游戏初始化，避免阻塞主进程造成界面卡顿
        StartCoroutine("Init");

        // 打开登录界面
        Proton.UIManager.Instance.OpenPanel<LoginPanel>("");
    }

    /// <summary>
    /// 开始战斗
    /// </summary>
    /// <param name="proto">协议消息</param>
    public void BeginBattle(ProtocolBytes proto)
    {
        // 开始监听网络延迟
        NetworkManager.Instance.NetworkStatus.Start();

        // 开始战斗同步
        _realSyncManager = new RealSyncManager();
        _realSyncManager.StartSync(proto);

        // 加载战斗场景
//        SceneManager.LoadScene("Demo/TrueSync/_Scenes/Helloworld");
        SceneManager.LoadScene("Demo/SpaceShooter/_Scenes/Battle");
    }

    /// <summary>
    /// 结束战斗
    /// </summary>
    public void EndBattle()
    {
        // 停止监听网络延迟
        NetworkManager.Instance.NetworkStatus.Shutdown();

        // 停止战斗同步

        // 卸载战斗场景

        // 回到游戏大厅
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private IEnumerator Init()
    {
        yield return null;

        // TODO: 启动游戏即刻连接网络，还是用户登录时再连接网络更好呢？

        // 启动网络
        NetworkManager.Instance.Start();

        // 初始化玩家信息
        PlayerManager.Instance.Init();
    }

    private void FixedUpdate()
    {
        // 驱动网络模块
        NetworkManager.Instance.Update();
    }
}