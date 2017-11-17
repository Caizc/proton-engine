using System;
using System.Linq;
using System.Text;

namespace Proton
{
    /// <summary>
    /// 字节流协议
    /// </summary>
    public class ProtocolBytes : Protocol
    {
        /// <summary>
        /// 协议数据包中的字节流
        /// </summary>
        private byte[] _bytes;

        public byte[] Bytes
        {
            get { return _bytes; }
            set { _bytes = value; }
        }

        /// <summary>
        /// 解码 readbuf 中从 start 开始的 length 字节
        /// </summary>
        public override Protocol Decode(byte[] readbuf, int start, int length)
        {
            ProtocolBytes proto = new ProtocolBytes();

            if (length > 0)
            {
                proto.Bytes = new byte[length];
                Array.Copy(readbuf, start, proto.Bytes, 0, length);
            }

            return proto;
        }

        /// <summary>
        /// 编码
        /// </summary>
        public override byte[] Encode()
        {
            return _bytes;
        }

        /// <summary>
        /// 获取协议名称
        /// </summary>
        public override string GetName()
        {
            return GetString(0);
        }

        /// <summary>
        /// 获取协议描述
        /// </summary>
        public override string GetDesc()
        {
            if (_bytes == null)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _bytes.Length; i++)
            {
                int data = _bytes[i];
                sb.Append(data).Append(" ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 添加一个字节
        /// </summary>
        /// <param name="bt"></param>
        public void AddByte(byte bt)
        {
            AddBytes(new[] {bt});
        }

        /// <summary>
        /// 获取一个字节
        /// </summary>
        public byte GetByte(int start, ref int end)
        {
            return GetBytes(start, ref end).First();
        }

        /// <summary>
        /// 添加字节数组
        /// </summary>
        public void AddBytes(byte[] bytes)
        {
            Int32 bytesLen = bytes.Length;
            byte[] bytesLenBytes = BitConverter.GetBytes(bytesLen);

            if (_bytes == null)
            {
                _bytes = bytesLenBytes.Concat(bytes).ToArray();
            }
            else
            {
                _bytes = _bytes.Concat(bytesLenBytes).Concat(bytes).ToArray();
            }
        }

        /// <summary>
        /// 从字节流中获取从下标 start 开始到 end 结束的字节数组
        /// </summary>
        public byte[] GetBytes(int start, ref int end)
        {
            if (_bytes == null)
            {
                return null;
            }

            if (_bytes.Length < start + sizeof(Int32))
            {
                return null;
            }

            Int32 bytesLen = BitConverter.ToInt32(_bytes, start);
            if (_bytes.Length < start + sizeof(Int32) + bytesLen)
            {
                return null;
            }

            byte[] bytes = new byte[bytesLen];

            // TODO: 这两种从字节数组中获取部分字节数组的方式，哪种更高效一些呢？
            // way 1:
            // bytes = _bytes.Skip(start + sizeof(Int32)).Take(bytesLen).ToArray();
            // way 2:
            Array.Copy(_bytes, start + sizeof(Int32), bytes, 0, bytesLen);

            end = start + sizeof(Int32) + bytesLen;

            return bytes;
        }

        /// <summary>
        /// 添加字符串
        /// </summary>
        public void AddString(string str)
        {
            Int32 strLen = str.Length;
            byte[] strLenBytes = BitConverter.GetBytes(strLen);

            byte[] strBytes = Encoding.UTF8.GetBytes(str);
            if (_bytes == null)
            {
                _bytes = strLenBytes.Concat(strBytes).ToArray();
            }
            else
            {
                _bytes = _bytes.Concat(strLenBytes).Concat(strBytes).ToArray();
            }
        }

        /// <summary>
        /// 从字节流中获取从下标 start 开始到 end 结束的字符串
        /// </summary>
        public string GetString(int start, ref int end)
        {
            if (_bytes == null)
            {
                return "";
            }

            if (_bytes.Length < start + sizeof(Int32))
            {
                return "";
            }

            Int32 strLen = BitConverter.ToInt32(_bytes, start);
            if (_bytes.Length < start + sizeof(Int32) + strLen)
            {
                return "";
            }

            string str = Encoding.UTF8.GetString(_bytes, start + sizeof(Int32), strLen);

            end = start + sizeof(Int32) + strLen;

            return str;
        }

        /// <summary>
        /// 从字节流中获取从下标 start 开始的一个字符串
        /// </summary>
        public string GetString(int start)
        {
            int end = 0;
            return GetString(start, ref end);
        }

        /// <summary>
        /// 添加一个整数
        /// </summary>
        public void AddInt(int num)
        {
            byte[] numBytes = BitConverter.GetBytes(num);

            if (_bytes == null)
            {
                _bytes = numBytes;
            }
            else
            {
                _bytes = _bytes.Concat(numBytes).ToArray();
            }
        }

        /// <summary>
        /// 从字节流中获取从下标 start 开始到 end 结束的整数
        /// </summary>
        public int GetInt(int start, ref int end)
        {
            if (_bytes == null)
            {
                return 0;
            }

            if (_bytes.Length < start + sizeof(Int32))
            {
                return 0;
            }

            int num = BitConverter.ToInt32(_bytes, start);

            end = start + sizeof(Int32);

            return num;
        }

        /// <summary>
        /// 从字节流中获取从下标 start 开始的一个整数
        /// </summary>
        public int GetInt(int start)
        {
            int end = 0;
            return GetInt(start, ref end);
        }

        /// <summary>
        /// 添加一个浮点数
        /// </summary>
        public void AddFloat(float num)
        {
            byte[] numBytes = BitConverter.GetBytes(num);

            if (_bytes == null)
            {
                _bytes = numBytes;
            }
            else
            {
                _bytes = _bytes.Concat(numBytes).ToArray();
            }
        }

        /// <summary>
        /// 从字节流中获取从下标 start 开始到 end 结束的浮点数
        /// </summary>
        public float GetFloat(int start, ref int end)
        {
            if (_bytes == null)
            {
                return 0.0f;
            }

            if (_bytes.Length < start + sizeof(float))
            {
                return 0.0f;
            }

            float num = BitConverter.ToSingle(_bytes, start);

            end = start + sizeof(float);

            return num;
        }

        /// <summary>
        /// 从字节流中获取从下标 start 开始的一个浮点数
        /// </summary>
        public float GetFloat(int start)
        {
            int end = 0;
            return GetFloat(start, ref end);
        }
    }
}