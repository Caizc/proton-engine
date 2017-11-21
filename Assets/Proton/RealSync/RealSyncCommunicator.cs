using Proton;

/// <summary>
/// 负责与网络通信底层 API 交互的接口，代替 TrueSync 依赖于 PUN 的原生实现
/// </summary>
public class RealSyncCommunicator : ICommunicator
{
    /// <summary>
    /// 返回本地与服务端之间的通信延迟
    /// </summary>
    /// <returns>往返时间</returns>
    public int RoundTripTime()
    {
        return (int) NetworkManager.Instance.NetworkStatus.RoundTripTime;
    }

    /// <summary>
    /// 向服务端提交一个广播给指定玩家的事件
    /// </summary>
    /// <param name="eventCode">事件编码</param>
    /// <param name="message">事件中的消息</param>
    /// <param name="reliable">是否保证消息传递的可靠性</param>
    /// <param name="toPlayers">接收该事件的玩家</param>
    public void OpRaiseEvent(byte eventCode, object message, bool reliable, int[] toPlayers)
    {
        // 组装协议
        ProtocolBytes proto = new ProtocolBytes();

        // 使用 "TrueSyncData" 来标识通过 TrueSync 处理的消息
        proto.AddString("TrueSyncData");
        // 添加事件编码
        proto.AddByte(eventCode);
        // TODO: 这里直接将 object 强转为 byte[]，可能导致该接口不通用，只能提交已经序列化为字节数组的消息
        proto.AddBytes((byte[]) message);

        // 向 Server 发送消息
        NetworkManager.Instance.ServerConn.Send(proto);
    }

    /// <summary>
    /// 添加一个处理接收到的消息的监听器
    /// </summary>
    /// <param name="onEventReceived">OnEventReceived 委托的实现</param>
    public void AddEventListener(OnEventReceived onEventReceived)
    {
        // TODO: TrueSync 消息监听
//        SpaceBattle.OnEventCall += delegate(byte eventCode, object content, int senderId)
//        {
//            onEventReceived(eventCode, content);
//        };
    }
}