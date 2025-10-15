using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolLibrary
{
   public class IntByteCode
    {

        #region INT TO BYTE[]
        
        /// <summary>
        /// 按操作系统 低位再前
        /// </summary>
        public void IntToByte1()
        {
            int num = 123456;
            byte[] bytes = BitConverter.GetBytes(num);
        }

        /// <summary>
        /// 转成高位在前
        /// </summary>
        public void IntToByte2()
        {
            int num = 123456;
            byte[] bytes = BitConverter.GetBytes(num);
            Array.Reverse(bytes);
        }


        /// <summary>
        /// 转成低位在前
        /// </summary>
        public void IntToByte3()
        {
            int num = 123456;
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(num & 0xff);
            bytes[1] = (byte)(num >> 8 & 0xff);
            bytes[2] = (byte)(num >> 16 & 0xff);
            bytes[3] = (byte)(num >> 24 & 0xff);
            Array.Reverse(bytes);
        }

        /// <summary>
        /// 转成高位在前
        /// </summary>
        public void IntToByte4()
        {
            int num = 123456;
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(num >> 24 & 0xff);
            bytes[1] = (byte)(num >> 16 & 0xff);
            bytes[2] = (byte)(num >> 8 & 0xff);
            bytes[3] = (byte)(num & 0xff);
            Array.Reverse(bytes);
        }
        #endregion


        #region BYTE[] TO INT

        /// <summary>
        /// 按操作系统 低位再前
        /// </summary>
        public void ByteToInt1()
        {
            byte[] bytes = { 64, 226, 1, 0 }; 
            int num = BitConverter.ToInt32(bytes, 0); 
        }

        /// <summary>
        /// 转成高位在前
        /// </summary>
        public void ByteToInt2()
        {
            byte[] bigEndianBytes = { 0, 1, 226, 64 }; // 大端序字节数组
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bigEndianBytes); // 转为小端序（适配系统默认）
            }
            int num = BitConverter.ToInt32(bigEndianBytes, 0); // 123456
        }


        /// <summary>
        /// 低位在前
        /// </summary>
        public void ByteToInt3()
        {
            byte[] bytes = { 64, 226, 1, 0 }; // 小端序：[0~7位, 8~15位, 16~23位, 24~31位]

            int num =
                ( bytes[0] & 0xFF) |                   // 0~7位：直接保留（无需移位）
                ((bytes[1] & 0xFF) << 8) |            // 8~15位：左移 8 位
                ((bytes[2] & 0xFF) << 16) |           // 16~23位：左移 16 位
                ((bytes[3] & 0xFF) << 24);            // 24~31位：左移 24 位

            Console.WriteLine(num); // 输出：123456
        }


        /// <summary>
        /// 高位在前
        /// </summary>
        public void ByteToInt4()
        {
            byte[] bytes = { 0, 1, 226, 64 }; // 大端序：[24~31位, 16~23位, 8~15位, 0~7位]

            int num =
                ((bytes[0] & 0xFF) << 24) |           // 24~31位：左移 24 位
                ((bytes[1] & 0xFF) << 16) |           // 16~23位：左移 16 位
                ((bytes[2] & 0xFF) << 8) |            // 8~15位：左移 8 位
                 (bytes[3] & 0xFF);                    // 0~7位：直接保留

            Console.WriteLine(num); // 输出：123456
        }
        #endregion


    }
}
