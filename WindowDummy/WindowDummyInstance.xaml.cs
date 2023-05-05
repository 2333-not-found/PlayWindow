using Box2DSharp.Dynamics;
using Rotate;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using WindowControl;
using Box2DEngine;
using System.Windows.Forms;

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
        public System.Drawing.Bitmap srcImage = null;

        public Tumbler tumbler;
        public Body body;
        public Box2DSharp.Dynamics.Joints.MouseJoint MouseJoint;
        public Body GroundBody;
        public Vector2 MouseWorld;

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
        public static void LoadWindow()
        {
            WindowDummyInstance windowDummyInstance = new WindowDummyInstance();
            windowDummyInstance.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //第零步：初始化参数
            UpdateTimer.Tick += UpdateEvent;
            intPtr_p = WindowFuncs.GetRoot(intPtr);
            this.Title = intPtr_p.ToString();
            this_intPtr = GetWindowHwndSource(this);
            var rect = WindowFuncs.GetWindowRectangle(intPtr_p);
            GroundBody = tumbler.World.CreateBody(new BodyDef());
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

            UpdateTimer.Start();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            WindowFuncs.ChangeOpacity(intPtr_p, 255);
        }

        #region MouseControl

        private class MouseQueryCallback : IQueryCallback
        {
            public Fixture QueryFixture;

            public Vector2 Point;

            public void Reset(in Vector2 point)
            {
                QueryFixture = null;
                Point = point;
            }

            /// <inheritdoc />
            public bool QueryCallback(Fixture fixture1)
            {
                var body = fixture1.Body;
                if (body.BodyType == BodyType.DynamicBody)
                {
                    var inside = fixture1.TestPoint(Point);
                    if (inside)
                    {
                        QueryFixture = fixture1;

                        // We are done, terminate the query.
                        return false;
                    }
                }

                // Continue the query.
                return true;
            }
        }

        private readonly MouseQueryCallback _callback = new MouseQueryCallback();

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (body != null)
            {
                if (OtherFuncs.IsDraging(intPtr_p) && MouseJoint == null)//MouseDown
                {
                    var p = tumbler.ConvertScreenToWorld(new Vector2(e.Location.X, e.Location.Y));
                    MouseWorld = p;

                    // Make a small box.
                    var aabb = new Box2DSharp.Collision.AABB();
                    var d = new Vector2(0.001f, 0.001f);
                    aabb.LowerBound = p - d;
                    aabb.UpperBound = p + d;

                    // Query the world for overlapping shapes.
                    _callback.Reset(p);
                    tumbler.World.QueryAABB(_callback, aabb);
                    if (_callback.QueryFixture != null)
                    {
                        float frequencyHz = 5.0f;
                        float dampingRatio = 0.7f;

                        var body = _callback.QueryFixture.Body;
                        var jd = new Box2DSharp.Dynamics.Joints.MouseJointDef
                        {
                            BodyA = GroundBody,
                            BodyB = body,
                            Target = p,
                            MaxForce = 1000.0f * body.Mass
                        };
                        Box2DSharp.Dynamics.Joints.JointUtils.LinearStiffness(out jd.Stiffness, out jd.Damping, frequencyHz, dampingRatio, jd.BodyA, jd.BodyB);
                        MouseJoint = (Box2DSharp.Dynamics.Joints.MouseJoint)tumbler.World.CreateJoint(jd);
                        body.IsAwake = true;
                    }
                }
            }
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (body != null)
            {
                if (MouseJoint != null)
                {
                    MouseWorld = tumbler.ConvertScreenToWorld(new Vector2(e.Location.X, e.Location.Y));
                    MouseJoint?.SetTarget(tumbler.ConvertScreenToWorld(new Vector2(e.X, e.Y)));
                }
            }
        }

        public void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (body != null)
            {
                MouseWorld = tumbler.ConvertScreenToWorld(new Vector2(e.Location.X, e.Location.Y));

                if (MouseJoint != null)
                {
                    tumbler.World.DestroyJoint(MouseJoint);
                    MouseJoint = null;
                }
            }
        }

        #endregion

        private async void UpdateEvent(object sender, EventArgs e)
        {
            await AnUpdate();
        }
        public async Task AnUpdate()
        {
            await
                   this.Dispatcher.InvokeAsync(new Action(() =>
                   {
                       //填充图像
                       WindowPic.Source = ToImageSource(OtherFuncs.GetWindowBitmap(intPtr_p));
                       if (WindowPic.Source == null)
                       {
                           tumbler.World.DestroyBody(body);
                           tumbler.World.DestroyBody(GroundBody);
                           this.Close();
                           return;
                       }
                       if (body == null)
                       {
                           body = tumbler.GetBody(intPtr_p);
                           return;
                       }

                       //移动部分
                       UserData userData = body.UserData as UserData;//body的UserData
                       var bodyPos = tumbler.ConvertWorldToScreen(body.GetTransform().Position);//body的位置
                                                                                                //替代Reflection功能
                       if (!OtherFuncs.IsDraging(intPtr_p))
                       {
                           var area = WindowFuncs.GetWindowRectangle(this_intPtr);
                           if (OtherFuncs.pointDelta.X > area.Left && OtherFuncs.pointDelta.X < area.Right && OtherFuncs.pointDelta.Y > area.Top && OtherFuncs.pointDelta.Y < area.Bottom)
                           {/*
                            mPoint p1 = OtherFuncs.pointDelta;
                            mPoint centrePoint = new mPoint() { X = area.X + area.Width / 2, Y = area.Y + area.Height / 2 };
                            Rotate.Rotate.RotateAngle(centrePoint, p1, body.GetAngle() * (180 / Math.PI) % 360, out mPoint p3);
                            WindowFuncs.SetWindowPos(intPtr_p, -2, (int)((int)bodyPos.X + (p1.X - p3.X)), (int)((int)bodyPos.Y + (p1.Y - p3.Y)), 0, 0, 1 | 4 | 20);
                            */
                               var mousept = Rotate.Rotate.GetRotateRectangle(OtherFuncs.pointDelta.X, OtherFuncs.pointDelta.Y, (float)(body.GetAngle() * (180 / Math.PI) % 360));
                               WindowFuncs.SetWindowPos(intPtr_p, -2, (int)bodyPos.X + mousept.X, (int)bodyPos.Y + mousept.Y, 0, 0, 1 | 4 | 20);
                           }
                           else
                           {
                               WindowFuncs.SetWindowPos(intPtr_p, -2, (int)bodyPos.X, (int)bodyPos.Y, 0, 0, 1 | 4 | 20);
                           }
                       }
                       //修改窗口及body大小
                       var newRect = WindowFuncs.GetWindowRectangle(intPtr_p);
                       if (newRect.Size != userData.rect.Size && WindowPic.Source != null && this.RenderSize == WindowPic.RenderSize)
                       {
                           tumbler.SetBodyRectangle(intPtr_p, newRect);
                       }
                       //移动此窗口
                       var pos = tumbler.ConvertWorldToScreen(body.GetTransform().Position);
                       WindowFuncs.SetWindowPos(this_intPtr, (int)intPtr_p, (int)pos.X, (int)pos.Y, 0, 0, 1 | 4 | 20);

                       //更新视图
                       RotateTransform rt = new RotateTransform
                       {
                           Angle = body.GetAngle() * (180 / Math.PI) % 360
                       };
                       //WindowPic.RenderTransformOrigin = new Point(0.5, 0.5);
                       //WindowPic.RenderTransform = rt;
                       WindowPic.LayoutTransform = rt;

                       System.Drawing.Rectangle realRect = Rotate.Rotate.GetRotateRectangle(WindowPic.Source.Width, WindowPic.Source.Height, (float)(body.GetAngle() * (180 / Math.PI) % 360));
                       this.Width = realRect.Width;
                       this.Height = realRect.Height;
                       WindowPic.Width = WindowPic.Source.Width;
                       WindowPic.Height = WindowPic.Source.Height;

                       try
                       {
                           rt = WindowPic.RenderTransform as RotateTransform;
                           if (rt.Angle == body.GetAngle() * (180 / Math.PI) % 360)
                           {
                               return;
                           }
                           else
                           {
                               rt = new RotateTransform
                               {
                                   Angle = body.GetAngle() * (180 / Math.PI) % 360
                               };
                               //WindowPic.RenderTransformOrigin = new Point(0.5, 0.5);
                               //WindowPic.RenderTransform = rt;
                               WindowPic.LayoutTransform = rt;
                           }
                       }
                       catch { }
                       {
                           rt = new RotateTransform
                           {
                               Angle = body.GetAngle() * (180 / Math.PI) % 360
                           };
                           //WindowPic.RenderTransformOrigin = new Point(0.5, 0.5);
                           //WindowPic.RenderTransform = rt;
                           WindowPic.LayoutTransform = rt;
                       }
                       //Console.WriteLine(body.GetAngle() * (180 / Math.PI) % 360);
                   }));
        }

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
            if (bitmap != null)
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
            return null;
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