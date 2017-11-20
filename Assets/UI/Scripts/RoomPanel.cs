/// <summary>
/// 房间界面
/// </summary>
public class RoomPanel : UIPanel
{
    #region UI 组件

    #endregion

    #region 生命周期方法

    public override void Init(params object[] args)
    {
        base.Init(args);

        SkinPath = "RoomPanel";
        Layer = PanelLayerEnum.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();
    }

    #endregion
}