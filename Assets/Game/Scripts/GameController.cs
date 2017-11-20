using System.Collections;
using Proton;
using UnityEngine;

/// <summary>
/// 游戏控制器（Main）
/// </summary>
public class GameController : MonoBehaviour
{
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
        UIManager.Instance.OpenPanel<LoginPanel>("");
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private IEnumerator Init()
    {
        yield return null;

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