using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proton
{
    /// <summary>
    /// UI 面板基类
    /// </summary>
    public class UIPanel : MonoBehaviour
    {
        // 面板名称
        public string PanelName;

        // 皮肤文件路径
        public string SkinPath;

        // 皮肤
        public GameObject Skin;

        // 面板层级
        public PanelLayerEnum Layer;

        // 面板参数
        protected object[] args;

        #region 生命周期方法

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="args">参数列表</param>
        public virtual void Init(params object[] args)
        {
            this.args = args;
            PanelName = GetType().ToString();
        }

        /// <summary>
        /// 显示前
        /// </summary>
        public virtual void OnShowing()
        {
        }

        /// <summary>
        /// 显示后
        /// </summary>
        public virtual void OnShowed()
        {
            Debug.Log("[界面已打开] " + PanelName);
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// 关闭前
        /// </summary>
        public virtual void OnClosing()
        {
        }

        /// <summary>
        /// 关闭后
        /// </summary>
        public virtual void OnClosed()
        {
            Debug.Log("[界面已关闭] " + PanelName);

            // 销毁对象
            Destroy(Skin);
            Destroy(this);
        }

        #endregion

        /// <summary>
        /// 关闭
        /// </summary>
        protected virtual void Close()
        {
            // Debug.Log("[关闭界面] " + PanelName);
            UIManager.Instance.ClosePanel(PanelName);
        }

        #region 操作方法

        #endregion
    }
}