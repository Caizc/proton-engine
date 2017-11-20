using Proton;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 注册界面
/// </summary>
public class RegisterPanel : UIPanel
{
    #region UI 组件

    private InputField _idInput;
    private InputField _pwInput;
    private InputField _repeatPwInput;
    private Button _registerButton;
    private Button _closeButton;

    #endregion

    #region 生命周期方法

    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init(params object[] args)
    {
        base.Init(args);

        SkinPath = "RegisterPanel";
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
        _repeatPwInput = skinTransform.Find("RepeatPwInput").GetComponent<InputField>();
        _registerButton = skinTransform.Find("RegisterBtn").GetComponent<Button>();
        _closeButton = skinTransform.Find("CloseBtn").GetComponent<Button>();

        // 为按钮添加点击事件监听
        _registerButton.onClick.AddListener(OnRegisterClick);
        _closeButton.onClick.AddListener(OnCloseClick);
    }

    #endregion

    /// <summary>
    /// 注册
    /// </summary>
    private void OnRegisterClick()
    {
        // 注册前的用户名密码校验
        if (_idInput.text.Equals("") || _pwInput.text.Equals(""))
        {
            UIManager.Instance.ShowAlertPanel("必须填写用户名和密码！");
            return;
        }

        if (!_pwInput.text.Equals(_repeatPwInput.text))
        {
            UIManager.Instance.ShowAlertPanel("两次输入的密码必须一致！");
            return;
        }

        // 检查网络连接是否可用
        if (!NetworkManager.Instance.IsAvailable())
        {
            UIManager.Instance.ShowAlertPanel("与服务器的连接已断开，请重启游戏！");
            return;
        }

        // 组装注册协议消息
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString(ProtocolType.REGISTER);
        proto.AddString(_idInput.text);
        proto.AddString(_pwInput.text);

        // 发送注册协议
        NetworkManager.Instance.serverConn.Send(proto, ProtocolType.REGISTER, OnRegisterCallback);
    }

    /// <summary>
    /// 注册后的回调方法
    /// </summary>
    /// <param name="proto">协议消息</param>
    private void OnRegisterCallback(Protocol proto)
    {
        ProtocolBytes responseProto = (ProtocolBytes) proto;

        int start = 0;

        // 协议名称，暂时无用，但需要从字节流中先读取出来
        string unused = responseProto.GetString(start, ref start);

        int result = responseProto.GetInt(start, ref start);
        if (result == 0)
        {
            UIManager.Instance.ShowAlertPanel("注册成功！");

            // 关闭注册界面
            Close();
        }
        else
        {
            UIManager.Instance.ShowAlertPanel("注册失败，请尝试其它用户名！");
        }
    }

    /// <summary>
    /// 关闭
    /// </summary>
    private void OnCloseClick()
    {
        Close();
    }
}