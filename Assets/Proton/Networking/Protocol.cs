namespace Proton
{
    /// <summary>
    /// 协议基类
    /// </summary>
    public class Protocol
    {
        /// <summary>
        /// 解码 readbuf 中从 start 开始的 length 字节
        /// </summary>
        /// <param name="readbuf">字节数组</param>
        /// <param name="start">开始的字节下标</param>
        /// <param name="length">解码的字节长度</param>
        /// <returns>协议类的实例</returns>
        public virtual Protocol Decode(byte[] readbuf, int start, int length)
        {
            return new Protocol();
        }

        /// <summary>
        /// 编码
        /// </summary>
        /// <returns>消息字节数组</returns>
        public virtual byte[] Encode()
        {
            return new byte[] { };
        }

        /// <summary>
        /// 获取协议名称
        /// </summary>
        /// <returns>协议名称</returns>
        public virtual string GetName()
        {
            return "";
        }

        /// <summary>
        /// 获取协议描述
        /// </summary>
        /// <returns>协议描述</returns>
        public virtual string GetDesc()
        {
            return "";
        }
    }
}