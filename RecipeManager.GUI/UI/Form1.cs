using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace UI
{
    public partial class Form1 : Form
    {
        // DllImport 属性用于创建圆角区域
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        public Form1()
        {
            InitializeComponent();

            // 设置窗体的圆角区域
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // 加载其他窗体到 Panel 中的方法
        private void LoadFormIntoPanel(Form childForm)
        {
            // 清除 Panel 中已有的控件
            panel1.Controls.Clear();

            // 将子窗体设置为非顶层窗体
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // 添加子窗体到 Panel 并显示
            panel1.Controls.Add(childForm);
            panel1.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        // 按钮点击事件，点击后加载其他窗体
        private void RMF_Click(object sender, EventArgs e)
        {
            // 加载关于recipe management form的内容
            
            
        }
    }
}
