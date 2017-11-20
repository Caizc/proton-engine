using Proton;
using UnityEngine;

/// <summary>
/// 网络状态
/// </summary>
public class NetworkStatus
{
    /// <summary>
    /// 平均延迟（毫秒）
    /// </summary>
    public float RoundTripTime
    {
        get { return _lastRtt; }
    }

    // 监听是否启用
    private bool _isActive;

    // 计数器
    private int _tick;

    // 总延迟 = 延迟1 + 延迟2 + 延迟3 + ...，单位为秒
    private float _rttSum;

    // 延迟计数（每次往 RTTSum 中加入一个延迟，该值加一）
    private int _rttCount;

    // 最后一个平均延迟 = 总延迟/延迟计数，单位为毫秒
    private float _lastRtt;

    /// <summary>
    /// 初始化网络状态监听
    /// </summary>
    public void Init()
    {
        // 向消息分发器注册接收到 Ping 消息事件的监听回调方法
        NetworkManager.Instance.ServerConn.AddEventListener(ProtocolType.PING, RecvPingCallback);

        Pause();
    }

    /// <summary>
    /// 开始监听
    /// </summary>
    public void Start()
    {
        _isActive = true;
    }

    /// <summary>
    /// 暂停监听
    /// </summary>
    public void Pause()
    {
        _isActive = false;
    }

    /// <summary>
    /// 停止网络状态监听
    /// </summary>
    public void Shutdown()
    {
        Pause();

        // 从消息分发器移除接收到 Ping 消息事件的监听回调方法
        NetworkManager.Instance.ServerConn.RemoveEventListener(ProtocolType.PING, RecvPingCallback);
    }

    /// <summary>
    /// 每一帧执行一次
    /// </summary>
    public void Update()
    {
        if (_isActive)
        {
            // 每隔 5 帧向服务端发送一个 Ping 消息，以测试网络延迟
            if (_tick++ >= 4)
            {
                // 组装 Ping 协议消息
                ProtocolBytes proto = new ProtocolBytes();
                proto.AddString(ProtocolType.PING);
                proto.AddFloat(Time.time);

                // 发送
                NetworkManager.Instance.ServerConn.Send(proto);

                _tick = 0;
            }
        }
    }

    /// <summary>
    /// 接收到 Ping 消息应答的回调方法
    /// </summary>
    /// <param name="proto"></param>
    private void RecvPingCallback(Protocol proto)
    {
        ProtocolBytes responseProto = (ProtocolBytes) proto;

        int start = 0;

        // 协议名称，暂时无用，但需要从字节流中先读取出来
        string unused = responseProto.GetString(start, ref start);

        // 获取 Ping 消息时间戳
        float pingTime = responseProto.GetFloat(start, ref start);

        // 计算网络延迟
        AddRtt(Time.time - pingTime);
    }

    /// <summary>
    /// 延迟累计
    /// </summary>
    /// <param name="rtt">网络延迟，单位为秒</param>
    private void AddRtt(float rtt)
    {
        _rttSum += rtt;
        _rttCount++;

        // 每累积 5 个延迟数据则计算一次平均延迟
        if (_rttCount > 4)
        {
            // 计算平均延迟，并转换为毫秒
            _lastRtt = _rttSum / _rttCount * 1000f;
            ResetRtt();
        }
    }

    /// <summary>
    /// 重置延迟相关计数器
    /// </summary>
    private void ResetRtt()
    {
        _rttSum = 0.0f;
        _rttCount = 0;
    }
}