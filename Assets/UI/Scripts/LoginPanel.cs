using Proton;
using UnityEngine;
using UnityEngine.UI;

namespace Proton
{
    /// <summary>
    /// 登录界面
    /// </summary>
    public class LoginPanel : UIPanel
    {
        #region UI 组件

        private InputField _idInput;
        private InputField _pwInput;
        private Button _loginButton;
        private Button _registerButton;

        #endregion

        #region 生命周期方法

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init(params object[] args)
        {
            base.Init(args);

            SkinPath = "LoginPanel";
            Layer = PanelLayerEnum.Panel;
        }

        /// <summary>
        /// 显示界面
        /// </summary>
        public override void OnShowing()
        {
            base.OnShowing();

            Transform skinTransform = Skin.transform;

            // 获取 UI 组件的引用
            _idInput = skinTransform.Find("IDInput").GetComponent<InputField>();
            _pwInput = skinTransform.Find("PWInput").GetComponent<InputField>();
            _loginButton = skinTransform.Find("LoginBtn").GetComponent<Button>();
            _registerButton = skinTransform.Find("RegisterBtn").GetComponent<Button>();

            // 为按钮添加点击事件监听
            _loginButton.onClick.AddListener(OnLoginClick);
            _registerButton.onClick.AddListener(OnRegisterClick);
        }

        #endregion

        /// <summary>
        /// 登录
        /// </summary>
        private void OnLoginClick()
        {
            // 登录前的用户名密码校验
            if (_idInput.text.Equals("") || _pwInput.text.Equals(""))
            {
                UIManager.Instance.ShowAlertPanel("必须填写用户名和密码！");
                return;
            }

            // 检查网络连接是否可用
            if (!NetworkManager.Instance.IsAvailable())
            {
                UIManager.Instance.ShowAlertPanel("与服务器的连接已断开，请重启游戏！");
                return;
            }

            // 组装登录协议消息
            ProtocolBytes proto = new ProtocolBytes();
            proto.AddString(ProtocolType.LOGIN);
            proto.AddString(_idInput.text);
            proto.AddString(_pwInput.text);

            // 发送登录协议
            NetworkManager.Instance.ServerConn.Send(proto, ProtocolType.LOGIN, RecvLoginCallback);
        }

        /// <summary>
        /// 注册
        /// </summary>
        private void OnRegisterClick()
        {
            // 打开注册界面
            UIManager.Instance.OpenPanel<RegisterPanel>("");
        }

        /// <summary>
        /// 接收到登录消息后的回调方法
        /// </summary>
        /// <param name="proto">协议消息</param>
        private void RecvLoginCallback(Protocol proto)
        {
            ProtocolBytes responseProto = (ProtocolBytes) proto;

            int start = 0;

            // 协议名称，暂时无用，但需要从字节流中先读取出来
            string unused = responseProto.GetString(start, ref start);

            int result = responseProto.GetInt(start, ref start);
            if (result == 0)
            {
                UIManager.Instance.ShowAlertPanel("登录成功！");

                // 打开游戏大厅界面
                UIManager.Instance.OpenPanel<LobbyPanel>("");

                // 设置玩家 ID 和昵称
                PlayerManager.Instance.CurrentPlayer.Id = _idInput.text;
                PlayerManager.Instance.CurrentPlayer.NickName = _idInput.text;

                // 关闭登录界面
                Close();
            }
            else
            {
                UIManager.Instance.ShowAlertPanel("登录失败，请检测用户名和密码！");
            }
        }
    }
}