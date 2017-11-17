using Proton;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 登录界面
/// </summary>
public class LoginPanel : UIPanel
{
    private InputField _idInput;
    private InputField _pwInput;
    private Button _loginButton;
    private Button _registerButton;

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

        // 获取 UI 元素
        _idInput = skinTransform.Find("IDInput").GetComponent<InputField>();
        _pwInput = skinTransform.Find("PWInput").GetComponent<InputField>();
        _loginButton = skinTransform.Find("LoginBtn").GetComponent<Button>();
        _registerButton = skinTransform.Find("RegisterBtn").GetComponent<Button>();

        // 为按钮添加点击事件
        _loginButton.onClick.AddListener(OnLoginClick);
        _registerButton.onClick.AddListener(OnRegisterClick);
    }

    #endregion

    /// <summary>
    /// 登录
    /// </summary>
    public void OnLoginClick()
    {
        if (_idInput.text.Equals("") || _pwInput.text.Equals(""))
        {
            UIManager.Instance.ShowAlertPanel("必须填写用户名和密码！");
            return;
        }

        // 组装登录协议消息
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString("Login");
        proto.AddString(_idInput.text);
        proto.AddString(_pwInput.text);

        // 发送登录协议
        NetworkManager.Instance.serverConn.Send(proto, "Login", OnLoginCallback);
    }

    /// <summary>
    /// 注册
    /// </summary>
    public void OnRegisterClick()
    {
        // 打开注册界面
        UIManager.Instance.OpenPanel<RegisterPanel>("");
    }

    /// <summary>
    /// 登录后的回调方法
    /// </summary>
    /// <param name="proto">协议消息</param>
    private void OnLoginCallback(Protocol proto)
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

            // 设置玩家 ID
            PlayerManager.Instance.PlayerID = _idInput.text;

            // 关闭登录界面
            Close();
        }
        else
        {
            UIManager.Instance.ShowAlertPanel("登录失败，请检测用户名和密码！");
        }
    }
}