namespace Proton
{
    /// <summary>
    /// 网络管理类
    /// </summary>
    public class NetworkManager
    {
        // 与服务端网络连接
        public Connection serverConn = new Connection();

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
            serverConn.Connect();
        }

        /// <summary>
        /// 每一帧处理网络事件
        /// </summary>
        public void Update()
        {
            // 驱动消息分发、心跳报告等
            serverConn.Update();
        }

        /// <summary>
        /// 网络连接是否可用
        /// </summary>
        /// <returns>网络连接是否可用</returns>
        public bool IsAvailable()
        {
            return serverConn.IsAlive();
        }

        /// <summary>
        /// 关闭网络
        /// </summary>
        public void Shutdown()
        {
            // 关闭 Socket 连接
            serverConn.Close();
        }
    }
}