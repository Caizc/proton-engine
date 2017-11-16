namespace Proton
{
    /// <summary>
    /// 网络管理类
    /// </summary>
    public class NetworkManager
    {
        public Connection conn = new Connection();

        private static NetworkManager _instance;

        /// <summary>
        /// 获取 NetworkManager 实例
        /// </summary>
        public static NetworkManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NetworkManager();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 私有构造方法，防止单例模式下产生多个类的实例
        /// </summary>
        private NetworkManager()
        {
            // nothing to do here.
        }

        /// <summary>
        /// 启动网络初始化
        /// </summary>
        public void Start()
        {
        }

        public void Update()
        {
        }
    }
}