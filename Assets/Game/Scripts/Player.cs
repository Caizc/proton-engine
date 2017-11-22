/// <summary>
/// 玩家
/// </summary>
public class Player
{
    private int _sid;

    private string _id;

    private string _nickName;

    private int _team;

    /// <summary>
    /// 序号
    /// </summary>
    public int Sid
    {
        get { return _sid; }

        set { _sid = value; }
    }

    /// <summary>
    /// ID
    /// </summary>
    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName
    {
        get { return _nickName; }
        set { _nickName = value; }
    }

    /// <summary>
    /// 所属队伍
    /// </summary>
    public int Team
    {
        get { return _team; }
        set { _team = value; }
    }
}