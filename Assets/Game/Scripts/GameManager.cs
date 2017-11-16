using System.Collections;
using System.Collections.Generic;
using Proton;
using UnityEngine;

/// <summary>
/// 游戏管理类（Main）
/// </summary>
public class GameManager : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this);
        Application.runInBackground = true;

        // 启动网络
        NetworkManager.Instance.Start();

        // TODO: 打开登录面板

        Debug.Log("=== The Game is RUNNING... ===");
    }

    void FixedUpdate()
    {
        // 驱动网络
        NetworkManager.Instance.Update();
    }
}