using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolLibrary;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string result = await HttpCode.HttpPostAsync("http://192.168.2.134:500", "");
            int a = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //short num = 12345;
            //byte[] bytes = BitConverter.GetBytes(num);





            int num = 123456;
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(num & 0xFF);          // 取最低 8 位
            bytes[1] = (byte)((num >> 8) & 0xFF);   // 右移 8 位，取次低 8 位
            bytes[2] = (byte)((num >> 16) & 0xFF);  // 右移 16 位，取次高 8 位
            bytes[3] = (byte)((num >> 24) & 0xFF);  // 右移 24 位，取最高 8 位




        }
    }
}
