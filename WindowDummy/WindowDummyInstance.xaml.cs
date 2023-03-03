using Box2DSharp.Dynamics;
using Rotate;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using System.Windows.Threading;
using WindowControl;
using Box2DEngine;

namespace WindowDummy
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WindowDummyInstance : Window
    {
        public IntPtr intPtr;//窗口的IntPtr
        public IntPtr intPtr_p;//根窗口的IntPtr
        public IntPtr this_intPtr;//Dummy的IntPtr
        public System.Drawing.Rectangle p_Rectangle;
        public System.Drawing.Rectangle this_Rectangle;
        public System.Drawing.Bitmap srcImage = null;
        public Tumbler tumbler;
        public Body body;

        public static readonly bool CurrentTransparent = false;
        private readonly DispatcherTimer UpdateTimer = new DispatcherTimer();

        /// <summary>
        /// 主窗体句柄
        /// </summary>
        public static HwndSource Hwnd;
        /// <summary>
        /// 获取窗体句柄
        /// </summary>
        /// <param name="window">窗体</param>
        public static IntPtr GetWindowHwndSource(DependencyObject window, bool isHwnd = true)
        {
            var formDependency = PresentationSource.FromDependencyObject(window);
            HwndSource winformWindow = formDependency as HwndSource;
            if (isHwnd)
                Hwnd = winformWindow;
            return winformWindow.Handle;
        }

        public WindowDummyInstance()
        {
            InitializeComponent();
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            WindowChrome.SetWindowChrome(this,
                new WindowChrome { GlassFrameThickness = WindowChrome.GlassFrameCompleteThickness, CaptionHeight = 0 });
            var visualTree = new FrameworkElementFactory(typeof(Border));
            visualTree.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Window.BackgroundProperty));
            var childVisualTree = new FrameworkElementFactory(typeof(ContentPresenter));
            childVisualTree.SetValue(UIElement.ClipToBoundsProperty, true);
            visualTree.AppendChild(childVisualTree);
            Template = new ControlTemplate
            {
                TargetType = typeof(Window),
                VisualTree = visualTree,
            };
            _dwmEnabled = Win32.Dwmapi.DwmIsCompositionEnabled();
            if (_dwmEnabled)
            {
                _hwnd = new WindowInteropHelper(this).EnsureHandle();
                Loaded += PerformanceDesktopTransparentWindow_Loaded;
                Background = Brushes.Transparent;
            }
            else
            {
                AllowsTransparency = true;
                Background = BrushCreator.GetOrCreate("#0100000");
                _hwnd = new WindowInteropHelper(this).EnsureHandle();
            }

            SetTransparentHitThrough();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //第零步：初始化参数
            UpdateTimer.Tick += UpdateEvent;
            intPtr_p = WindowFuncs.GetRoot(intPtr);
            this.Title = intPtr_p.ToString();
            this_intPtr = GetWindowHwndSource(this);
            var rect = WindowFuncs.GetWindowRectangle(intPtr_p);
            UserData userData = new UserData()
            {
                intPtr_p = intPtr_p,
                this_intPtr = this_intPtr,
                rect = rect
            };
            tumbler.AddBody(intPtr_p, new Vector2(rect.X, rect.Y), userData);
            body = tumbler.GetBody(intPtr_p);
            WindowFuncs.ChangeOpacity(intPtr_p, 20);

            //第二步：截图并填充图像
            srcImage = OtherFuncs.GetWindowBitmap(intPtr_p);
            WindowPic.Width = srcImage.Width;
            WindowPic.Height = srcImage.Height;
            this.Width = srcImage.Width;
            this.Height = srcImage.Height;

            //第三步：找到父窗口的RECT
            p_Rectangle = WindowFuncs.GetWindowRectangle(intPtr_p);

            //第四步：将本实例放到窗口位置
            this.Top = p_Rectangle.Location.X;
            this.Left = p_Rectangle.Location.Y;

            //第八步：监测窗口大小更新

            UpdateTimer.Start();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            tumbler.World.DestroyBody(body);
            WindowFuncs.ChangeOpacity(intPtr_p, 255);
        }

        private void UpdateEvent(object sender, EventArgs e)
        {
            if (body == null)
            {
                body = tumbler.GetBody(intPtr_p);
                return;
            }

            /*// Check position
            System.Drawing.Graphics currentGraphics = System.Drawing.Graphics.FromHwnd(new WindowInteropHelper(this).Handle);

            var mousePoint = MouseHook.MouseHook.GetMousePos();
            if (VisualTreeHelper.HitTest((Grid)this.Content, new Point(mousePoint.X - this.Left, mousePoint.Y - this.Top)) != null)
            {
                if (CurrentTransparent == true)
                {
                    SetTransparentNotHitThrough();
                }
                CurrentTransparent = false;
            }
            else
            {
                if (CurrentTransparent == false)
                {
                    SetTransparentHitThrough();
                }
                CurrentTransparent = true;
            }*/

            //更新视图
            WindowPic.Source = ToImageSource(OtherFuncs.GetWindowBitmap(intPtr_p));
            WindowPic.Width = WindowPic.Source.Width;
            WindowPic.Height = WindowPic.Source.Height;
            RotateTransform rt = new RotateTransform
            {
                Angle = -(body.GetAngle() * (180 / Math.PI) % 360)
            };
            ContentGrid.RenderTransformOrigin = new Point(0.5, 0.5);
            ContentGrid.RenderTransform = rt;

            //移动
            UserData userData = body.UserData as UserData;
            var output = tumbler.WorldToProcessing(body.GetTransform().Position);
            if (OtherFuncs.IsDraging(userData.intPtr_p))
            {
                body.ApplyLinearImpulseToCenter(new Vector2(OtherFuncs.pointDelta.X, OtherFuncs.pointDelta.Y), true);
            }
            else
            {
                var area = WindowFuncs.GetWindowRectangle(this_intPtr);
                if (OtherFuncs.pointDelta.X > area.Left && OtherFuncs.pointDelta.X < area.Right && OtherFuncs.pointDelta.Y > area.Top && OtherFuncs.pointDelta.Y < area.Bottom)
                {
                    mPoint p1 = OtherFuncs.pointDelta;
                    mPoint centrePoint = new mPoint() { X = area.X + area.Width / 2, Y = area.Y + area.Height / 2 };
                    Rotate.Rotate.RotateAngle(centrePoint, p1, body.GetAngle() * (180 / Math.PI) % 360, out mPoint p3);
                    WindowFuncs.SetWindowPos(intPtr_p, -2, (int)((int)output.X + (p1.X - p3.X)), (int)((int)output.Y + (p1.Y - p3.Y)), 0, 0, 1 | 4 | 20);
                }
                else
                {
                    WindowFuncs.SetWindowPos(intPtr_p, -2, (int)output.X, (int)output.Y, 0, 0, 1 | 4 | 20);
                }
            }

            var newRect = WindowFuncs.GetWindowRectangle(intPtr_p);
            if (newRect.Size != userData.rect.Size && WindowPic.Source != null && this.RenderSize == WindowPic.RenderSize)
            {
                tumbler.SetBodyRectangle(intPtr_p, newRect);
            }

            var pos = tumbler.WorldToProcessing(body.GetTransform().Position);
            WindowFuncs.SetWindowPos(this_intPtr, (int)intPtr_p, (int)pos.X, (int)pos.Y, 0, 0, 1 | 4 | 20);

            //AfterAll
            this.RenderSize = WindowPic.RenderSize;
            this_Rectangle = WindowFuncs.GetWindowRectangle(this_intPtr);

        }

        /*
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
        
        private void WindowDummyInstance_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void WindowPic_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void WindowDummyInstance_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }


        #endregion

        private void WindowDummyInstance_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OnUpdate();
        }

        private void WindowPic_MouseUp(object sender, MouseButtonEventArgs e)
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
        }*/

        #region 穿透模块

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);
        /// <summary>
        /// 将 Bitmap 转换为 ImageSource
        /// 使用过System.Drawing.Bitmap后一定要用DeleteObject释放掉对象，不然内存不释，很快系统内存就消耗光了。
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static ImageSource ToImageSource(System.Drawing.Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            // 记得要进行内存释放。否则会有内存不足的报错。
            if (!DeleteObject(hBitmap))
            {
                throw new System.ComponentModel.Win32Exception();
            }
            return wpfBitmap;
        }

        /// <summary>
        /// 设置点击穿透到后面透明的窗口
        /// </summary>
        public void SetTransparentHitThrough()
        {
            if (_dwmEnabled)
            {
                Win32.User32.SetWindowLongPtr(_hwnd, Win32.GetWindowLongFields.GWL_EXSTYLE,
                    (IntPtr)(int)((long)Win32.User32.GetWindowLongPtr(_hwnd, Win32.GetWindowLongFields.GWL_EXSTYLE) | (long)Win32.ExtendedWindowStyles.WS_EX_TRANSPARENT));
            }
            else
            {
                Background = Brushes.Transparent;
            }
        }

        /// <summary>
        /// 设置点击命中，不会穿透到后面的窗口
        /// </summary>
        public void SetTransparentNotHitThrough()
        {
            if (_dwmEnabled)
            {
                Win32.User32.SetWindowLongPtr(_hwnd, Win32.GetWindowLongFields.GWL_EXSTYLE,
                    (IntPtr)(int)((long)Win32.User32.GetWindowLongPtr(_hwnd, Win32.GetWindowLongFields.GWL_EXSTYLE) & ~(long)Win32.ExtendedWindowStyles.WS_EX_TRANSPARENT));
            }
            else
            {
                Background = BrushCreator.GetOrCreate("#0100000");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct STYLESTRUCT
        {
            public int styleOld;
            public int styleNew;
        }

        private void PerformanceDesktopTransparentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ((HwndSource)PresentationSource.FromVisual(this)).AddHook((IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) =>
            {
                //想要让窗口透明穿透鼠标和触摸等，需要同时设置 WS_EX_LAYERED 和 WS_EX_TRANSPARENT 样式，
                //确保窗口始终有 WS_EX_LAYERED 这个样式，并在开启穿透时设置 WS_EX_TRANSPARENT 样式
                //但是WPF窗口在未设置 AllowsTransparency = true 时，会自动去掉 WS_EX_LAYERED 样式（在 HwndTarget 类中)，
                //如果设置了 AllowsTransparency = true 将使用WPF内置的低性能的透明实现，
                //所以这里通过 Hook 的方式，在不使用WPF内置的透明实现的情况下，强行保证这个样式存在。
                if (msg == (int)Win32.WM.STYLECHANGING && (long)wParam == (long)Win32.GetWindowLongFields.GWL_EXSTYLE)
                {
                    var styleStruct = (STYLESTRUCT)Marshal.PtrToStructure(lParam, typeof(STYLESTRUCT));
                    styleStruct.styleNew |= (int)Win32.ExtendedWindowStyles.WS_EX_LAYERED;
                    Marshal.StructureToPtr(styleStruct, lParam, false);
                    handled = true;
                }
                return IntPtr.Zero;
            });
        }

        /// <summary>
        /// 是否开启 DWM 了，如果开启了，那么才可以使用高性能的桌面透明窗口
        /// </summary>
        private readonly bool _dwmEnabled;
        private readonly IntPtr _hwnd;

        #endregion
    }
}