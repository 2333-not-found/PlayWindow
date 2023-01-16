using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace ResizeWindow
{
    public class ResizeWindow
    {
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

        //窗体移动this 
        private void panel_title_bar_MouseDown(object sender, MouseEventArgs e)
        {
            Rectangle tl = new Rectangle(0, 0, 8, 8);
            Rectangle tr = new Rectangle(this.Width - 8, 0, 8, 8);
            if (!tl.Contains(this.PointToClient(Cursor.Position)) && !tr.Contains(this.PointToClient(Cursor.Position)))
            {
                ReleaseCapture();

                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            }
        }

        //窗体大小的改变object

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Rectangle tl = new Rectangle(0, 0, 5, 5);
            Rectangle tr = new Rectangle(this.Width - 5, 0, 5, 5);
            Rectangle dr = new Rectangle(0, this.Height - 5, 5, 5);
            Rectangle dl = new Rectangle(this.Width - 5, this.Height - 5, 5, 5);
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
        }

        private void UpdateEvent(object sender, EventArgs e)
        {
            Rectangle tl = new Rectangle(0, 0, 5, 5);
            Rectangle tr = new Rectangle(this.Width - 5, 0, 5, 5);
            Rectangle dr = new Rectangle(0, this.Height - 5, 5, 5);
            Rectangle dl = new Rectangle(this.Width - 5, this.Height - 5, 5, 5);
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
    }
}
