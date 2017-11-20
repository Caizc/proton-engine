namespace Proton
{
    /// <summary>
    /// 网络管理类
    /// </summary>
    public class NetworkManager
    {
        // 与服务端网络连接
        public Connection ServerConn = new Connection();

        // 网络状态
        public NetworkStatus NetworkStatus = new NetworkStatus();

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
        /// 启动网络
        /// </summary>
        public void Start()
        {
            // 连接服务端
            ServerConn.Connect();

            // 初始化网络状态监听
            NetworkStatus.Init();
        }

        /// <summary>
        /// 每一帧处理网络事件
        /// </summary>
        public void Update()
        {
            // 驱动消息分发、心跳报告等
            ServerConn.Update();

            // 驱动网络状态监听
            NetworkStatus.Update();
        }

        /// <summary>
        /// 网络连接是否可用
        /// </summary>
        /// <returns>网络连接是否可用</returns>
        public bool IsAvailable()
        {
            return ServerConn.IsAlive();
        }

        /// <summary>
        /// 关闭网络
        /// </summary>
        public void Shutdown()
        {
            // 停止网络状态监听
            NetworkStatus.Shutdown();

            // 关闭 Socket 连接
            ServerConn.Close();
        }
    }
}