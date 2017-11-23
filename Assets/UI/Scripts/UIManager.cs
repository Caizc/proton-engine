using System;
using System.Collections.Generic;
using UnityEngine;

namespace Proton
{
    /// <summary>
    /// UI 管理类
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        // UI 画布
        private GameObject _canvas;

        // UI 层级表
        private Dictionary<PanelLayerEnum, Transform> _layerDict = new Dictionary<PanelLayerEnum, Transform>();

        // UI 面板表
        private Dictionary<string, UIPanel> _panelDict = new Dictionary<string, UIPanel>();

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
            _instance = this;

            // 初始化 UI 层级
            InitUILayer();
        }

        /// <summary>
        /// 打开面板
        /// </summary>
        /// <typeparam name="T">UIPanel 子类</typeparam>
        /// <param name="skinPath"></param>
        /// <param name="args">参数列表</param>
        public void OpenPanel<T>(string skinPath, params object[] args) where T : UIPanel
        {
            // 面板名称（请确保面板[资源名称]与其[脚本名称]一致）
            string panelName = typeof(T).ToString();

            // 面板已打开，结束该方法流程
            if (_panelDict.ContainsKey(panelName))
            {
                return;
            }

            // 为 UICanvas 附加与面板对应的处理脚本
            UIPanel panel = _canvas.AddComponent<T>();

            panel.Init(args);
            _panelDict.Add(panelName, panel);

            // 为面板加载皮肤
            skinPath = (skinPath == null || skinPath.Equals("")) ? panel.SkinPath : skinPath;
            GameObject skin = Resources.Load<GameObject>(skinPath);
            if (skin == null)
            {
                Debug.LogError("[空引用错误] 无法加载面板皮肤 [" + skinPath + "]！");
                return;
            }

            panel.Skin = Instantiate(skin);

            // 设置面板对象在 Scene 中的层级
            PanelLayerEnum layer = panel.Layer;
            Transform parentTransform = _layerDict[layer];

            Transform panelTransform = panel.Skin.transform;
            panelTransform.SetParent(parentTransform, false);

            // 打开面板
            panel.OnShowing();
            panel.OnShowed();
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        /// <param name="name">面板名字</param>
        public void ClosePanel(string name)
        {
            UIPanel panel = _panelDict[name];

            if (panel == null)
            {
                return;
            }

            // 关闭面板
            panel.OnClosing();
            _panelDict.Remove(name);
            panel.OnClosed();
        }

        /// <summary>
        /// 显示提示消息框
        /// </summary>
        /// <param name="message">提示消息</param>
        public void ShowAlertPanel(string message)
        {
            OpenPanel<AlertPanel>("", message);
        }

        /// <summary>
        /// 初始化 UI 层级
        /// </summary>
        private void InitUILayer()
        {
            // 获取 Scene 中 UICanvas 对象引用
            _canvas = GameObject.Find("UICanvas");
            if (_canvas == null)
            {
                Debug.LogError("[空引用错误] 无法获得 GameObject UICanvas 的引用！");
                return;
            }

            // 保存 UI 层级的 Transform
            foreach (PanelLayerEnum panelLayer in Enum.GetValues(typeof(PanelLayerEnum)))
            {
                string name = panelLayer.ToString();
                Transform transform = _canvas.transform.Find(name);
                _layerDict.Add(panelLayer, transform);
            }
        }
    }
}