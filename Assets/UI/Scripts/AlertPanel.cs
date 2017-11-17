/// <summary>
/// 提示消息框
/// </summary>
public class AlertPanel : UIPanel
{
    public override void Init(params object[] args)
    {
        base.Init(args);
        SkinPath = "AlertPanel";
        Layer = PanelLayerEnum.Alert;
    }

    public override void OnShowing()
    {
        base.OnShowing();
    }
}