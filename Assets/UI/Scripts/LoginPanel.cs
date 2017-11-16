/// <summary>
/// 登录界面
/// </summary>
public class LoginPanel : UIPanel
{
    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init(params object[] args)
    {
        base.Init(args);
        SkinPath = "LoginPanel";
        Layer = PanelLayerEnum.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();
    }
}