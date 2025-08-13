using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ToolLibrary
{
    public class TcpSocketClient
    {
        static byte[] ReadBytes = new byte[1024];
        public System.Net.Sockets.TcpClient tcpClient;
        public string adderss = "192.168.1.50";
        public string port = "5000";

        public void ConnectToServer()
        {
            tcpClient = new System.Net.Sockets.TcpClient();//构造Socket
            tcpClient.BeginConnect(IPAddress.Parse(adderss), int.Parse(port), Lianjie, null);//开始异步
        }

        void Lianjie(IAsyncResult ar)
        {
            try
            {
                if (!tcpClient.Connected)
                {
                    tcpClient = new System.Net.Sockets.TcpClient();
                    tcpClient.BeginConnect(IPAddress.Parse(adderss), int.Parse(port), Lianjie, null);

                }
                else
                {

                    tcpClient.EndConnect(ar);//结束异步连接
                    tcpClient.GetStream().BeginRead(ReadBytes, 0, ReadBytes.Length, ReceiveCallBack, null);
                }
            }
            catch (Exception ex)
            {

            }
        }

        void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int len = tcpClient.GetStream().EndRead(ar);//结束异步读取
                if (len > 0)
                {

                    string str = System.Text.Encoding.ASCII.GetString(ReadBytes, 0, len);
                    Uri.UnescapeDataString(str);
                    tcpClient.GetStream().BeginRead(ReadBytes, 0, ReadBytes.Length, ReceiveCallBack, null);
                }
                else
                {
                    tcpClient = null;
                    tcpClient = new System.Net.Sockets.TcpClient();//构造Socket
                    tcpClient.BeginConnect(IPAddress.Parse(adderss), int.Parse(port), Lianjie, null);//开始异步
                }
            }
            catch (Exception ex)

            {
            }
        }

        public void SendMessage(string msg)
        {
            byte[] msgBytes = StrToHexByte(msg);
            tcpClient.GetStream().BeginWrite(msgBytes, 0, msgBytes.Length, (ar) =>
            {
                tcpClient.GetStream().EndWrite(ar);//结束异步发送
            }, null);//开始异步发送
        }

        public byte[] StrToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        public void Close()
        {
            tcpClient.Close();
        }
    }
}
