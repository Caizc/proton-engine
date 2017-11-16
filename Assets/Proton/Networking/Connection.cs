using System;
using System.Net.Sockets;
using UnityEngine;

namespace Proton
{
    /// <summary>
    /// 网络连接类
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// 连接状态
        /// </summary>
        public enum Status
        {
            // 未连接
            DISCONNECTED,

            // 已连接
            CONNECTED
        }

        // 本连接实例的连接状态
        public Status ConnStatus = Status.DISCONNECTED;

        // 客户端 Socket
        private Socket _socket;

        // 默认的数据传输协议
        private Protocol _proto = new ProtocolBytes();

        // 默认的接收数据缓冲区大小
        private const int BUFFER_SIZE = 1024;

        // 接收数据缓冲区
        private byte[] _dataBuffer = new byte[BUFFER_SIZE];

        // 当前缓存的字节数
        private int _bufferCount = 0;

        // 消息包长度
        private Int32 _msgLength = 0;

        // 表示消息包长度的字节数据（4 个字节）
        private byte[] _msgLenBytes = new byte[sizeof(UInt32)];

        // 消息分发器
        private MessageDistributor _msgDistributor = new MessageDistributor();

        /// <summary>
        /// 每一帧处理消息分发
        /// </summary>
        public void Update()
        {
            // 分发待处理消息列表中的消息
            _msgDistributor.Update();

            if (ConnStatus == Status.CONNECTED)
            {
                // TODO: 发送心跳消息
            }
        }

        /// <summary>
        /// 与服务端建立 Socket 连接
        /// </summary>
        /// <param name="host">服务端主机地址</param>
        /// <param name="port">服务端 Socket 监听端口</param>
        /// <returns>连接是否建立成功</returns>
        public bool Connect(string host, int port)
        {
            // 如果连接已建立，则不再重复建立连接
            if (ConnStatus == Status.CONNECTED)
            {
                return true;
            }

            try
            {
                // 初始化客户端 Socket
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // 连接服务端
                _socket.Connect(host, port);
                // 异步接收服务端发来的数据
                _socket.BeginReceive(_dataBuffer, _bufferCount, BUFFER_SIZE - _bufferCount, SocketFlags.None,
                    DataReceivedCallback,
                    _dataBuffer);

                // 设置连接状态为“已连接”
                ConnStatus = Status.CONNECTED;

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("[建立连接出错] 与服务端 " + host + ":" + port + " 建立 Socket 连接失败！\n" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 关闭 Socket 连接
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            try
            {
                _socket.Close();

                ConnStatus = Status.DISCONNECTED;

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("[关闭连接出错] 关闭 Socket 连接失败！\n" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 向服务端发送数据
        /// </summary>
        /// <param name="proto">协议消息包</param>
        /// <returns>是否发送成功</returns>
        public bool Send(Protocol proto)
        {
            if (ConnStatus != Status.CONNECTED)
            {
                Debug.LogWarning("[连接未就绪] 网络连接未就绪，无法发送信息！");
                return false;
            }

            // 打包数据
            byte[] data = proto.Pack();
            // 发送数据
            _socket.Send(data);

            return true;
        }

        /// <summary>
        /// 向服务端发送数据，并注册动作监听器
        /// </summary>
        /// <param name="proto">协议消息包</param>
        /// <param name="protoName">协议名称</param>
        /// <param name="listener">动作监听器</param>
        /// <returns></returns>
        public bool Send(Protocol proto, string protoName, MessageDistributor.MessageListener listener)
        {
            if (ConnStatus != Status.CONNECTED)
            {
                return false;
            }

            // 注册动作监听器
            _msgDistributor.AddActionListener(protoName, listener);

            return Send(proto);
        }

        /// <summary>
        /// 向服务端发送数据，并注册动作监听器
        /// </summary>
        /// <param name="proto">协议消息包</param>
        /// <param name="listener">动作监听器</param>
        /// <returns></returns>
        public bool Send(Protocol proto, MessageDistributor.MessageListener listener)
        {
            string protoName = proto.GetName();
            return Send(proto, protoName, listener);
        }

        /// <summary>
        /// 数据接收回调方法
        /// </summary>
        private void DataReceivedCallback(IAsyncResult ar)
        {
            try
            {
                int dataCount = _socket.EndReceive(ar);
                _bufferCount += dataCount;

                // 读取并处理数据
                ProcessData();

                // 继续异步接收数据
                _socket.BeginReceive(_dataBuffer, _bufferCount, BUFFER_SIZE - _bufferCount, SocketFlags.None,
                    DataReceivedCallback,
                    _dataBuffer);
            }
            catch (Exception e)
            {
                Debug.LogError("\n" + e.Message);
            }
        }

        /// <summary>
        /// 从缓冲区中读取并处理数据
        /// </summary>
        private void ProcessData()
        {
            // 缓存的字节数小于 4 个字节，即消息包长度信息不完整，暂不处理
            if (_bufferCount < sizeof(Int32))
            {
                return;
            }

            // 获取消息包长度
            Array.Copy(_dataBuffer, _msgLenBytes, sizeof(Int32));
            _msgLength = BitConverter.ToInt32(_msgLenBytes, 0);

            // 消息包未接收完整，暂不处理
            if (_bufferCount < sizeof(Int32) + _msgLength)
            {
                return;
            }

            // 解码消息包
            Protocol msgProto = _proto.Decode(_dataBuffer, sizeof(Int32), _msgLength);

            // 添加到待处理的消息列表中
            lock (_msgDistributor.MessageList)
            {
                _msgDistributor.MessageList.Add(msgProto);
            }

            // 缓冲区中余下未处理的字节数
            int bytesRemaining = _bufferCount - _msgLength - sizeof(Int32);

            // TODO: 这种做法比较低效
            // 清除缓冲区中已处理的消息字节
            Array.Copy(_dataBuffer, sizeof(Int32) + _msgLength, _dataBuffer, 0, bytesRemaining);

            // 如果缓冲区中还有未处理的消息，则递归调用 ProcessData 方法，实现循环处理
            _bufferCount = bytesRemaining;
            if (_bufferCount > 0)
            {
                ProcessData();
            }
        }
    }
}