using System.Collections.Generic;

namespace Proton
{
    /// <summary>
    /// RealSync 管理类
    /// </summary>
    public class RealSyncManager
    {
        /// <summary>
        /// 需要同步操作的玩家列表
        /// </summary>
        public static List<Player> Players = new List<Player>();

        public bool IsBattleStart;

        // TODO: 接收到服务端发送数据的事件回调。这里跟底层消息分发的事件回调机制有所重复，应该可以再精简去掉一次回调
        public delegate void EventCallback(byte eventCode, object content, int senderId);

        public static EventCallback OnEventCall;

        /// <summary>
        /// 构造方法
        /// </summary>
        public RealSyncManager()
        {
            // 清空玩家列表
            Players.Clear();
        }

        /// <summary>
        /// 开始同步
        /// </summary>
        /// <param name="proto">消息协议</param>
        public void StartSync(ProtocolBytes proto)
        {
            // 解析协议
            int start = 0;

            string protoName = proto.GetString(start, ref start);
            if (protoName != ProtocolType.FIGHT)
            {
                return;
            }

            // 玩家总数
            int playerCount = proto.GetInt(start, ref start);

            //TODO: 清理战斗场景

            // 保存每一个同步玩家的信息
            for (int i = 0; i < playerCount; i++)
            {
                string id = proto.GetString(start, ref start);
                int team = proto.GetInt(start, ref start);

                // TODO: unused
                int unused = proto.GetInt(start, ref start);

                Player player = new Player();
                player.Sid = i;
                player.Id = id;
                player.NickName = id;
                player.Team = team;

                // 添加到需要同步操作的玩家列表中
                Players.Add(player);
            }

            // 向消息分发管理器注册接收到 TrueSync 同步数据后的回调方法
            NetworkManager.Instance.ServerConn.AddEventListener(ProtocolType.TRUESYNC, RecvTrueSyncDataCallback);

            IsBattleStart = true;
        }

        /// <summary>
        /// 接收到的 TrueSync 数据后的回调方法
        /// </summary>
        private void RecvTrueSyncDataCallback(Protocol proto)
        {
            ProtocolBytes responseProto = (ProtocolBytes) proto;

            int start = 0;

            // 协议名称，暂时无用，但需要从字节流中先读取出来
            string unused = responseProto.GetString(start, ref start);

            byte eventCode = responseProto.GetByte(start, ref start);
            byte[] data = responseProto.GetBytes(start, ref start);

            // TODO: 事件编码和发送玩家 ID 暂时写死（协议内容中可以解析到每个指令的玩家 ID）
            OnEventCall(eventCode, data, -1);
        }
    }
}