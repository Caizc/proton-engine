/// <summary>
/// 游戏大厅界面
/// </summary>
public class LobbyPanel : UIPanel
{
    public override void Init(params object[] args)
    {
        base.Init(args);
        SkinPath = "LobbyPanel";
        Layer = PanelLayerEnum.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();
    }
}