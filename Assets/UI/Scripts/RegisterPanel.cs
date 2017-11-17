/// <summary>
/// 注册界面
/// </summary>
public class RegisterPanel : UIPanel
{
    #region 生命周期方法

    public override void Init(params object[] args)
    {
        base.Init(args);
        SkinPath = "RegisterPanel";
        Layer = PanelLayerEnum.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();
    }

    #endregion
}