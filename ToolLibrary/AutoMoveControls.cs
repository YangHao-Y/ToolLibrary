using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolLibrary
{
    public class AutoMoveControls
    {

        private Dictionary<string, string> dcControlsPoint = new Dictionary<string, string>();


        /// <summary>
        /// 取得所有的控件名称
        /// </summary>
        /// <param name="ctrlParent"></param>
        private void GetControls(Control ctrlParent)
        {
            foreach (Control ctrlChild in ctrlParent.Controls)
            {
                string keyName = ctrlParent.Name + "." + ctrlChild.Name;

                if (dcControlsPoint.ContainsKey(keyName) == false)
                {
                    dcControlsPoint.Add(keyName, "");
                    if (ctrlChild.Controls.Count > 0)
                    {
                        GetControls(ctrlChild);
                    }
                }
            }
        }



        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hform"></param>
        /// <param name="bchangefont"></param>
        public AutoMoveControls(Form hform, bool bchangefont = true)
        {
            if (hform != null)
            {
                hform.Resize += new EventHandler(Form_Resize);

                bChangeFont = bchangefont;
                bLoadFlag = true;
                x = hform.Width;
                y = hform.Height;
                pForm = hform;
                dcControlsPoint.Clear();

                GetControls(hform);

                setTag(hform);

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hform"></param>
        /// <param name="bchangefont"></param>
        /// <param name="winState"></param>
        public AutoMoveControls(Form hform, bool bchangefont, FormWindowState winState)
        {
            if (hform != null)
            {
                hform.Resize += new EventHandler(Form_Resize);

                bChangeFont = bchangefont;
                bLoadFlag = true;
                x = hform.Width;
                y = hform.Height;
                pForm = hform;
                dcControlsPoint.Clear();

                GetControls(hform);

                setTag(hform);

                hform.WindowState = winState;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void MoveAll()
        {
            if (bLoadFlag)
            {
                if (pForm != null)
                {
                    float newx = (pForm.Width) / x;
                    float newy = (pForm.Height) / y;
                    setControls(newx, newy, pForm);
                }
            }
        }

        private float x = 0.0f;//定义当前窗体的宽度
        private float y = 0.0f;//定义当前窗体的高度
        private bool bLoadFlag = false;
        private Form pForm = null;
        private bool bChangeFont = true;

        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                string keyName = con.Parent.Name + "." + con.Name;
                dcControlsPoint[keyName] = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size; ;

                // con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;

                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }
            }
        }



        private void setControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                string keyName = con.Parent.Name + "." + con.Name;


                //获取控件的Tag属性值，并分割后存储字符串数组
                //  string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
                try
                {
                    string[] mytag = dcControlsPoint[keyName].Split(new char[] { ';' });

                    //根据窗体缩放的比例确定控件的值，宽度
                    con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newx);
                    con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newy);//高度
                    con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newx);//左边距
                    con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newy);//顶边距
                    if (bChangeFont)
                    {
                        Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                        con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    }
                    if (con.Controls.Count > 0)
                    {
                        setControls(newx, newy, con);
                    }
                }
                catch (Exception ex)
                {
                    string ss = ex.Message;
                }

            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            MoveAll();
        }
    }
}
