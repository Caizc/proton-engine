using System.Collections.Generic;

namespace Proton
{
    /// <summary>
    /// 消息分发器
    /// </summary>
    public class MessageDistributor
    {
        // 待处理的消息列表
        public List<Protocol> MessageList = new List<Protocol>();

        // 消息监听器委托
        public delegate void MessageListener(Protocol proto);

        // 每一帧处理的消息数
        private const int PROCESS_COUNT_PER_FRAME = 15;

        // 事件消息监听表（事件消息监听器委托注册一次后，持续监听该类型的消息）
        private Dictionary<string, MessageListener> _eventDict = new Dictionary<string, MessageListener>();

        // 动作消息监听表（动作消息监听器委托注册一次后，在接收到应答消息后即移除）
        private Dictionary<string, MessageListener> _actionDict = new Dictionary<string, MessageListener>();

        /// <summary>
        /// 每一帧处理消息列表中的 N 个消息包
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < PROCESS_COUNT_PER_FRAME; i++)
            {
                if (MessageList.Count > 0)
                {
                    // 向对应消息监听器分发消息列表中的消息包
                    DistributeMessage(MessageList[0]);

                    lock (MessageList)
                    {
                        MessageList.RemoveAt(0);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 添加事件监听器
        /// </summary>
        /// <param name="protoName">协议名称</param>
        /// <param name="listener">事件监听器</param>
        public void AddEventListener(string protoName, MessageListener listener)
        {
            if (_eventDict.ContainsKey(protoName))
            {
                _eventDict[protoName] += listener;
            }
            else
            {
                _eventDict[protoName] = listener;
            }
        }

        /// <summary>
        /// 移除事件监听器
        /// </summary>
        /// <param name="protoName">协议名称</param>
        /// <param name="listener">事件监听器</param>
        public void RemoveEventListener(string protoName, MessageListener listener)
        {
            if (_eventDict.ContainsKey(protoName))
            {
                _eventDict[protoName] -= listener;

                if (_eventDict[protoName] == null)
                {
                    _eventDict.Remove(protoName);
                }
            }
        }

        /// <summary>
        /// 添加动作监听器
        /// </summary>
        /// <param name="protoName">协议名称</param>
        /// <param name="listener">动作监听器</param>
        public void AddActionListener(string protoName, MessageListener listener)
        {
            if (_actionDict.ContainsKey(protoName))
            {
                _actionDict[protoName] += listener;
            }
            else
            {
                _actionDict[protoName] = listener;
            }
        }

        /// <summary>
        /// 移除动作监听器
        /// </summary>
        /// <param name="protoName">协议名称</param>
        /// <param name="listener">动作监听器</param>
        public void RemoveActionListener(string protoName, MessageListener listener)
        {
            if (_actionDict.ContainsKey(protoName))
            {
                _actionDict[protoName] -= listener;

                if (_actionDict[protoName] == null)
                {
                    _actionDict.Remove(protoName);
                }
            }
        }

        /// <summary>
        /// 向各消息监听器分发消息协议包
        /// </summary>
        /// <param name="proto">消息协议包</param>
        private void DistributeMessage(Protocol proto)
        {
            // 获取协议名称
            string protoName = proto.GetName();

            // 分发事件应答消息包
            if (_eventDict.ContainsKey(protoName))
            {
                _eventDict[protoName](proto);
            }

            // 分发动作应答消息包
            if (_actionDict.ContainsKey(protoName))
            {
                _actionDict[protoName](proto);

                // 移除动作监听
                _actionDict[protoName] = null;
                _actionDict.Remove(protoName);
            }
        }
    }
}