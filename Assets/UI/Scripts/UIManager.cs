using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI 管理类
/// </summary>
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    /// <summary>
    /// 获取 UIManager 实例
    /// </summary>
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }

    /// <summary>
    /// 私有构造方法，防止单例模式下产生多个类的实例
    /// </summary>
    private UIManager()
    {
        // nothing to do here.
    }

    void Awake()
    {
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    /// <typeparam name="T">UIPanel 子类</typeparam>
    /// <param name="skinPath"></param>
    /// <param name="args">参数列表</param>
    public void OpenPanel<T>(string skinPath, params object[] args) where T : UIPanel
    {
    }
}