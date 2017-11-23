using UnityEngine;
using UnityEngine.UI;

namespace Proton
{
    /// <summary>
    /// 提示消息框
    /// </summary>
    public class AlertPanel : UIPanel
    {
        // 消息内容
        private string _message;

        #region UI 组件

        private Text _messageText;
        private Button _confirmButton;

        #endregion

        #region 生命周期方法

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init(params object[] args)
        {
            base.Init(args);

            SkinPath = "AlertPanel";
            Layer = PanelLayerEnum.Alert;

            // 从参数列表中获取消息内容
            if (args.Length > 0)
            {
                _message = (string) args[0];
            }
        }

        /// <summary>
        /// 显示界面
        /// </summary>
        public override void OnShowing()
        {
            base.OnShowing();

            Transform skinTransform = Skin.transform;

            // 获取 UI 组件的引用
            _messageText = skinTransform.Find("MessageText").GetComponent<Text>();
            _confirmButton = skinTransform.Find("ConfirmBtn").GetComponent<Button>();

            // 设置消息内容
            _messageText.text = _message;

            // 为按钮添加点击事件
            _confirmButton.onClick.AddListener(OnConfirmClick);
        }

        #endregion

        /// <summary>
        /// 确认
        /// </summary>
        private void OnConfirmClick()
        {
            // 关闭消息框
            Close();
        }
    }
}