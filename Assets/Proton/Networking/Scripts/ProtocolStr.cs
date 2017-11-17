using System.Text;

namespace Proton
{
    /// <summary>
    /// 字符串协议
    /// </summary>
    public class ProtocolStr : Protocol
    {
        /// <summary>
        /// 协议数据包中的字符串
        /// </summary>
        private string _str;

        public string Str
        {
            get { return _str; }
            set { _str = value; }
        }

        /// <summary>
        /// 解码 readbuf 中从 start 开始的 length 字节
        /// </summary>
        public override Protocol Decode(byte[] readbuf, int start, int length)
        {
            ProtocolStr proto = new ProtocolStr();

            proto.Str = Encoding.UTF8.GetString(readbuf, start, length);
            return proto;
        }

        /// <summary>
        /// 编码
        /// </summary>
        public override byte[] Encode()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(_str);
            return bytes;
        }

        /// <summary>
        /// 获取协议名称
        /// </summary>
        public override string GetName()
        {
            if (_str.Length == 0)
            {
                return "";
            }

            string name = _str.Split(',')[0];
            return name;
        }

        /// <summary>
        /// 获取协议描述
        /// </summary>
        public override string GetDesc()
        {
            return _str;
        }
    }
}