/// <summary>
/// 玩家管理类
/// </summary>
public class PlayerManager
{
    // 当前玩家
    public Player CurrentPlayer = new Player();

    private static PlayerManager _instance;

    /// <summary>
    /// 获取 PlayerManager 实例
    /// </summary>
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerManager();
            }
            return _instance;
        }
    }

    /// <summary>
    /// 私有构造方法，防止单例模式下产生多个类的实例
    /// </summary>
    private PlayerManager()
    {
        // nothing to do here.
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        // 默认玩家 ID 为“游客”
        CurrentPlayer.Id = "tourist";
    }
}