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
using WindowControl;
using Rotate;
using Box2DEngine;

namespace WindowDummy
{
    public partial class WindowDummyInstance : Form
    {
        public IntPtr intPtr;
        public IntPtr intPtr_p;
        public Rectangle p_Rectangle;
        public Rectangle clientRectangle;
        public Image srcImage = null;

        private mPoint p1;

        public static event Action OnWindowResize;

        public WindowDummyInstance()
        {
            InitializeComponent();
            timer1.Tick += UpdateEvent;
            timer1.Start();
        }

        private void WindowDummyInstance_Load(object sender, EventArgs e)
        {
            Start();
        }

        //第零步：初始化参数
        //第一步：找到父窗口的RECT（然后设置文本）
        //第二步：截图并填充图像
        //第三步：找到根窗口句柄（然后设置文本）
        //第四步：将本实例放到窗口位置
        //TODO：
        //第五步：旋转+移动
        //第六步：映射
        //第七步：监测鼠标移动窗口
        //第八步：监测窗口大小更新

        private void Start()
        {
            //第一步：找到根窗口句柄（然后设置文本）
            intPtr_p = WindowFuncs.GetRoot(intPtr);
            IntPtrLabel.Text = "IntPtr:" + Convert.ToString(intPtr);
            IntPtrLabel_p.Text = "父窗口的IntPtr:" + Convert.ToString(intPtr_p);

            //第二步：截图并填充图像，检查IntPtr是否有效
            srcImage = OtherFuncs.GetWindowBitmap(intPtr_p);
            if (srcImage != null)
            {
                WindowPic.Width = srcImage.Width;
                WindowPic.Height = srcImage.Height;
                this.Width = srcImage.Width;
                this.Height = srcImage.Height;
            }
            else
            {
                MessageBox.Show("无效的IntPtr!");
                this.Close();
            }

            //第三步：找到父窗口的RECT（然后设置文本）
            p_Rectangle = WindowFuncs.GetWindowRectangle(intPtr_p);
            RECTLabel_p.Text = Convert.ToString(p_Rectangle);

            //第四步：将本实例放到窗口位置
            this.Location = p_Rectangle.Location;

            //第五步：旋转+移动
            //计算旋转中心
            p1.X = this.Width / 2;
            p1.Y = this.Height / 2;


            //第七步：监测鼠标移动窗口
            var point_p = new Point();

            WindowFuncs.ClientToScreen(intPtr_p, ref point_p);
            clientRectangle = new Rectangle(point_p.X - p_Rectangle.X, point_p.Y - p_Rectangle.Y, p_Rectangle.Width, p_Rectangle.Height);

            //Console.WriteLine(clientRectangle);

            //第八步：监测窗口大小更新

        }

        private void OnUpdate()
        {
            srcImage = OtherFuncs.GetWindowBitmap(intPtr_p);
            WindowFuncs.SetWindowPos(intPtr_p, -2, this.Location.X, this.Location.Y, srcImage.Width, srcImage.Height, 4);
        }

        #region ResizeWindow

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        //常量
        public const int WM_SYSCOMMAND = 0x0112;

        //窗体移动
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        //改变窗体大小
        public const int WMSZ_LEFT = 0xF001;
        public const int WMSZ_RIGHT = 0xF002;
        public const int WMSZ_TOP = 0xF003;
        public const int WMSZ_TOPLEFT = 0xF004;
        public const int WMSZ_TOPRIGHT = 0xF005;
        public const int WMSZ_BOTTOM = 0xF006;
        public const int WMSZ_BOTTOMLEFT = 0xF007;
        public const int WMSZ_BOTTOMRIGHT = 0xF008;

        //窗体大小的改变object

        private void WindowDummyInstance_MouseDown(object sender, MouseEventArgs e)
        {
            Rectangle tl = new Rectangle(0, 0, 8, 8);
            Rectangle tr = new Rectangle(this.Width - 8, 0, 8, 8);
            Rectangle dr = new Rectangle(0, this.Height - 8, 8, 8);
            Rectangle dl = new Rectangle(this.Width - 8, this.Height - 8, 8, 8);
            if (tl.Contains(this.PointToClient(Cursor.Position)))
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, (int)WMSZ_TOPLEFT, 0);
                //SendMessage(intPtr_p, WM_SYSCOMMAND, (int)WMSZ_TOPLEFT, 0);
                this.Cursor = Cursors.Arrow;
            }
            else if (tr.Contains(this.PointToClient(Cursor.Position)))
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, (int)WMSZ_TOPRIGHT, 0);
                //SendMessage(intPtr_p, WM_SYSCOMMAND, (int)WMSZ_TOPRIGHT, 0);
                this.Cursor = Cursors.Arrow;
            }
            else if (dr.Contains(this.PointToClient(Cursor.Position)))
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, (int)WMSZ_BOTTOMLEFT, 0);
                //SendMessage(intPtr_p, WM_SYSCOMMAND, (int)WMSZ_BOTTOMLEFT, 0);
                this.Cursor = Cursors.Arrow;
            }
            else if (dl.Contains(this.PointToClient(Cursor.Position)))
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, (int)WMSZ_BOTTOMRIGHT, 0);
                //SendMessage(intPtr_p, WM_SYSCOMMAND, (int)WMSZ_BOTTOMRIGHT, 0);
                this.Cursor = Cursors.Arrow;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void WindowPic_MouseDown(object sender, MouseEventArgs e)
        {
            //窗体移动
            //Console.WriteLine(p_Rectangle.Contains(this.PointToClient(Cursor.Position)));
            //Console.WriteLine(clientRectangle.Contains(this.PointToClient(Cursor.Position)));
            if (!clientRectangle.Contains(this.PointToClient(Cursor.Position)))
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
                OnUpdate();
            }

            Rectangle tl = new Rectangle(0, 0, 8, 8);
            Rectangle tr = new Rectangle(this.Width - 8, 0, 8, 8);
            Rectangle dr = new Rectangle(0, this.Height - 8, 8, 8);
            Rectangle dl = new Rectangle(this.Width - 8, this.Height - 8, 8, 8);
            if (tl.Contains(this.PointToClient(Cursor.Position)))
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, (int)WMSZ_TOPLEFT, 0);
                this.Cursor = Cursors.Arrow;
            }
            else if (tr.Contains(this.PointToClient(Cursor.Position)))
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, (int)WMSZ_TOPRIGHT, 0);
                this.Cursor = Cursors.Arrow;
            }
            else if (dr.Contains(this.PointToClient(Cursor.Position)))
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, (int)WMSZ_BOTTOMLEFT, 0);
                this.Cursor = Cursors.Arrow;
            }
            else if (dl.Contains(this.PointToClient(Cursor.Position)))
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, (int)WMSZ_BOTTOMRIGHT, 0);
                this.Cursor = Cursors.Arrow;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
            }

            //Reflection
            mPoint p3;
            var _ = this.PointToClient(Cursor.Position);
            mPoint p2;
            p2.X = _.X;
            p2.Y = _.Y;
            Rotate.Rotate.RotateAngle(p1, p2, 30, out p3);
        }


        private void WindowDummyInstance_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        #endregion

        private void WindowDummyInstance_MouseUp(object sender, MouseEventArgs e)
        {
            OnUpdate();
        }

        private void WindowPic_MouseUp(object sender, MouseEventArgs e)
        {
            OnUpdate();
        }

        private void UpdateEvent(object sender, EventArgs e)
        {
            Rectangle tl = new Rectangle(0, 0, 8, 8);
            Rectangle tr = new Rectangle(this.Width - 8, 0, 8, 8);
            Rectangle dr = new Rectangle(0, this.Height - 8, 8, 8);
            Rectangle dl = new Rectangle(this.Width - 8, this.Height - 8, 8, 8);
            if (tl.Contains(this.PointToClient(Cursor.Position)))
            {
                this.Cursor = Cursors.SizeNWSE;
            }
            else if (tr.Contains(this.PointToClient(Cursor.Position)))
            {
                this.Cursor = Cursors.SizeNESW;
            }
            else if (dr.Contains(this.PointToClient(Cursor.Position)))
            {
                this.Cursor = Cursors.SizeNESW;
            }
            else if (dl.Contains(this.PointToClient(Cursor.Position)))
            {
                this.Cursor = Cursors.SizeNWSE;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int angle = 30;
            //调用方法获取旋转后的图像
            Image rotateImage = Rotate.Rotate.GetRotateImage(srcImage, angle);
            Graphics graphics = WindowPic.CreateGraphics();
            graphics.Clear(Color.Gray);
            //将旋转后的图像显示到pictureBox里
            graphics.DrawImage(rotateImage, new Rectangle(0, 0, rotateImage.Width, rotateImage.Height));
            //显示图像所占矩形区域
            graphics.DrawRectangle(new Pen(Color.Yellow), new Rectangle(0, 0, rotateImage.Width, rotateImage.Height));
            WindowPic.Width = rotateImage.Width;
            WindowPic.Height = rotateImage.Height;
            this.Width = rotateImage.Width;
            this.Height = rotateImage.Height;
        }
    }
}