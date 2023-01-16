using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace WindowControl
{
    public class WindowFuncs
    {
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, StringBuilder lParam);
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        /// <summary>  
        /// 通过全屏幕坐标获取控件句柄,不获取隐藏或禁止的窗口句柄
        /// </summary>  
        /// <param name="p">屏幕坐标</param>  
        /// <returns>返回值为包含该点的窗口的句柄。如果包含指定点的窗口不存在，返回值为NULL。如果该点在静态文本控件之上，返回值是在该静态文本控件的下面的窗口的句柄。</returns>  
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point p);

        /// <summary>
        /// 通过相对父控件的坐标获取子控件句柄,可获取隐藏或禁止的窗口句柄
        /// </summary>
        /// <param name="hWndParent">父控件句柄</param>
        /// <param name="p">相对父控件的坐标</param>
        /// <returns>如果坐标在父控件以外返回NULL，如果坐标在父控件以内但坐标位置没有子控件则返回父控件本身句柄，如果坐标位置有子控件返回子控件的句柄</returns>
        //[DllImport("user32.dll")]
        //public static extern IntPtr ChildWindowFromPoint(IntPtr hWndParent, Point p);

        /// <summary>
        /// 将屏幕坐标转换为控件的相对坐标
        /// </summary>
        /// <param name="hWnd">控件句柄</param>
        /// <param name="lpPoint">引用坐标参数：使用前用屏幕坐标赋值，转换成功则变成控件相对坐标</param>
        /// <returns>是否成功</returns>
        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);
        /// <summary>
        /// 将坐标从当前窗体转化成全屏幕的坐标
        /// </summary>
        /// <param name="hwnd">控件句柄</param>
        /// <param name="lpPoint">把坐标从当前窗体转化成全屏幕</param>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "ClientToScreen")]
        public static extern int ClientToScreen(IntPtr hwnd, ref Point lpPoint);

        /// <summary>
        /// 获取窗口的父窗口句柄，如果是顶层窗口返回0
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <returns>父窗口句柄</returns>
        [DllImport("user32")]
        public static extern IntPtr GetParent(IntPtr hwnd);

        /// <summary>  
        /// 得到此控件的类名  
        /// </summary>  
        /// <param name="hWnd"></param>  
        /// <param name="classname">接收数据的要首先给出空间</param>  
        /// <param name="nlndex">所要取得的最大字符数，如果设置为0 则什么都没有</param>  
        /// <returns></returns>  
        //[DllImport("user32.dll", EntryPoint = "GetClassName")]
        //public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        ///<summary>
        ///获取控件的名称（标题）
        ///</summary>
        ///<param name="hwnd">控件句柄</param>
        ///<param name="lpString">存储字符的对象</param>
        ///<param name="nMaxCount">获取字符的最大长度</param>
        ///<returns>返回字符字节长度（一个中文字符算2个）</returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 获取窗口的创建者（线程或进程）
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="lpdwProcessId">进程ID</param>
        /// <returns>线程号</returns>
        //[DllImport("user32.dll")]
        //public static extern int GetWindowThreadProcessId(IntPtr hWnd, ref int lpdwProcessId);

        //应用程序发送此消息来复制对应窗口的文本到缓冲区
        public static int WM_GETTEXT = 0x0D;
        //得到与一个窗口有关的文本的长度（不包含空字符）
        public static int WM_GETTEXTLENGTH = 0x0E;
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 改变窗口的位置与状态
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="hWndlnsertAfter">窗口的 Z 顺序</param>
        /// <param name="X">位置X</param>
        /// <param name="Y">位置Y</param>
        /// <param name="cx">大小Width</param>
        /// <param name="cy">大小Height</param>
        /// <param name="Flags">选项</param>
        /// <returns></returns>

        //hWndInsertAfter 参数可选值:
        public static int HWND_TOP = 0; //{在前面}
        public static int HWND_BOTTOM = 1; //{在后面}
        public static int HWND_TOPMOST = -1; //{在前面, 位于任何顶部窗口的前面}
        public static int HWND_NOTOPMOST = -2; //{在前面, 位于其他顶部窗口的后面}
        //uFlags 参数可选值:
        public static int SWP_NOSIZE = 1; //{忽略 cx、cy, 保持大小}
        public static int SWP_NOMOVE = 2; //{忽略 X、Y, 不改变位置}
        public static int SWP_NOZORDER = 4; //{忽略 hWndInsertAfter, 保持 Z 顺序}
        public static int SWP_NOREDRAW = 8; //{不重绘}
        public static int SWP_NOACTIVATE = 10; //{不激活}
        public static int SWP_FRAMECHANGED = 20; //{强制发送 WM_NCCALCSIZE 消息, 一般只是在改变大小时才发送此消息}
        public static int SWP_SHOWWINDOW = 40; //{显示窗口}
        public static int SWP_HIDEWINDOW = 80; //{隐藏窗口}
        public static int SWP_NOCOPYBITS = 100; //{丢弃客户区}
        public static int SWP_NOOWNERZORDER = 200; //{忽略 hWndInsertAfter, 不改变 Z 序列的所有者}
        public static int SWP_NOSENDCHANGING = 400; //{不发出 WM_WINDOWPOSCHANGING 消息}
        public static int SWP_DRAWFRAME = SWP_FRAMECHANGED; //{画边框}
        public static int SWP_NOREPOSITION = SWP_NOOWNERZORDER;
        public static int SWP_DEFERERASE = 2000; //{防止产生 WM_SYNCPAINT 消息}
        public static int SWP_ASYNCWINDOWPOS = 4000; //{若调用进程不拥有窗口, 系统会向拥有窗口的线程发出需求}
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);

        public static IntPtr GetRoot(IntPtr intPtr)
        {
            while (GetParent(intPtr) != IntPtr.Zero)
                intPtr = GetParent(intPtr);
            return intPtr;
        }

        public IntPtr GetHandleFromCursor(bool childrenOrRoot)
        {
            //this.Text = title + string.Format("坐标：X={0},Y={1}", Cursor.Position.X, Cursor.Position.Y);

            IntPtr intPtr = WindowFromPoint(new Point(Cursor.Position.X, Cursor.Position.Y));
            IntPtr cHandle = intPtr;
            IntPtr rHandle;

            int len = SendMessage(intPtr, WM_GETTEXTLENGTH, 0, 0);
            StringBuilder sb = new StringBuilder(len + 1);
            SendMessage(intPtr, WM_GETTEXT, sb.Capacity, sb);
            //this.textBox_cText.Text = sb.ToString();

            sb = new StringBuilder(255);
            //len = GetClassName(intPtr, sb, sb.Capacity);
            //this.textBox_cClassName.Text = sb.ToString();

            IntPtr pHwnd = GetParent(intPtr);
            if (pHwnd != IntPtr.Zero)
            {
                rHandle = pHwnd;
                sb.Clear();
                //len = GetWindowText(pHwnd, sb, sb.Capacity);
                //textBox_pTitle.Text = sb.ToString();
                sb.Clear();
                //len = GetClassName(pHwnd, sb, sb.Capacity);
                //textBox_pClassName.Text = sb.ToString();
            }
            else
            {
                rHandle = IntPtr.Zero;
                //textBox_pTitle.Text = "";
                //textBox_pClassName.Text = "";
            }
            /*
            int pID = 0;
            int threadID = GetWindowThreadProcessId(hwnd, ref pID);
            textBox_id.Text = pID.ToString();
            textBox_description.Text = ""; textBox_fileName.Text = ""; textBox_Company.Text = "";
            try
            {
                System.Diagnostics.Process process = System.Diagnostics.Process.GetProcessById(pID);
                if (process != null)
                {
                    textBox_fileName.Text = process.MainModule.FileName;
                    System.Diagnostics.FileVersionInfo info = System.Diagnostics.FileVersionInfo.GetVersionInfo(process.MainModule.FileName);
                    textBox_description.Text = info.FileDescription;
                    textBox_Company.Text = info.LegalCopyright;
                }
            }
            catch { }*/
            if (childrenOrRoot)
                return cHandle;
            else
            {
                while (GetParent(rHandle) != IntPtr.Zero)
                    rHandle = GetParent(rHandle);
                return rHandle;
            }
        }

        //指定扩展窗口，以便控制窗口的透明度
        [DllImport("user32", EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);
        //设置窗体的透明度，dwFlags指定为2时，bAlpha才能起作用
        [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        private static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, int bAlpha, int dwFlags);

        private const int GWL_EXSTYLE = (-20); //设置为分层窗口 20 才能进行控制
        private const uint WS_EX_LAYERED = 0x80000;
        private const int LWA_ALPHA = 0x2;

        public void ChangeOpacity(IntPtr intPtr, int windowOpacity)
        {
            SetWindowLong(intPtr, GWL_EXSTYLE, WS_EX_LAYERED);
            SetLayeredWindowAttributes(intPtr, 0, windowOpacity, LWA_ALPHA);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public extern static int GetClientRect(IntPtr hWnd, ref RECT rc);

        public static Rectangle GetClientRectangle(IntPtr intPtr)
        {
            RECT rect = new RECT();
            GetClientRect(intPtr, ref rect);

            return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }

        public static Rectangle GetWindowRectangle(IntPtr intPtr)
        {
            RECT rect = new RECT();
            GetWindowRect(intPtr, ref rect);
            return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }

        public struct RECT
        {
            public int Left;                             //最左坐标
            public int Top;                             //最上坐标
            public int Right;                           //最右坐标
            public int Bottom;                        //最下坐标
        }

    }

    public class OtherFuncs
    {
        public Point GetMousePosition()
        {
            return new Point(Cursor.Position.X, Cursor.Position.Y);
        }

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt(
                                        IntPtr hdcDest,    //目标DC的句柄
                                        int nXDest,        //目标DC的矩形区域的左上角的x坐标
                                        int nYDest,        //目标DC的矩形区域的左上角的y坐标
                                        int nWidth,        //目标DC的句型区域的宽度值
                                        int nHeight,       //目标DC的句型区域的高度值
                                        IntPtr hdcSrc,     //源DC的句柄
                                        int nXSrc,         //源DC的矩形区域的左上角的x坐标
                                        int nYSrc,         //源DC的矩形区域的左上角的y坐标
                                        System.Int32 dwRo  //光栅的处理数值
                                        );

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int GetWindowRect(IntPtr intPtr, out Rectangle lpRect);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public extern static IntPtr GetWindowDC(IntPtr intPtr);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public extern static int ReleaseDC(IntPtr intPtr, IntPtr hDC);

        public static Bitmap GetWindowBitmap(IntPtr intPtr)
        {
            Bitmap bitmap;

            if (intPtr != IntPtr.Zero)
            {
                try
                {
                    GetWindowRect(intPtr, out Rectangle rect);  //获得目标窗体的大小
                    bitmap = new Bitmap(rect.Width - rect.X, rect.Height - rect.Y);
                    //Console.WriteLine(rect);

                    Graphics g1 = Graphics.FromImage(bitmap);
                    IntPtr hdc1 = GetWindowDC(intPtr);
                    IntPtr hdc2 = g1.GetHdc();  //得到Bitmap的DC
                    BitBlt(hdc2, 0, 0, rect.Width, rect.Height, hdc1, 0, 0, 13369376);
                    g1.ReleaseHdc(hdc2);  //释放掉Bitmap的DC
                    ReleaseDC(intPtr, hdc1);
                    //bitmap.Save("map.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    //以JPG文件格式保存
                    return bitmap;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}