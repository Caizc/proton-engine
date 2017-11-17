using UnityEngine;

namespace Proton
{
    /// <summary>
    /// 网络配置
    /// </summary>
    [CreateAssetMenu(fileName = "NetworkConfig", menuName = "Proton/NetworkConfig", order = 0)]
    public class NetworkConfig : ScriptableObject
    {
        /// <summary>
        /// 服务端主机地址
        /// </summary>
        public string Host = "127.0.0.1";

        /// <summary>
        /// 服务端 Socket 监听端口号
        /// </summary>
        public int Port = 9527;
    }
}