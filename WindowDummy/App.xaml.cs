using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Box2DEngine;
using WindowControl;

namespace WindowDummy
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
    }

    public class WindowDummyCenter
    {
        public List<WindowDummyInstance> windowDummyInstances = new List<WindowDummyInstance>(3);
        public MouseHook.MouseHook mh = null;//钩子实例
        public NormalTest engine = new NormalTest();

        public void StartEngine()
        {
            Thread thread = new Thread(new ThreadStart(EngineThread))
            {
                Name = "EngineThread"
            };
            thread.Start();
        }

        private void EngineThread()
        {
            engine.Run();
        }

        public Task RunNewWindowAsync(IntPtr intPtr, MouseHook.MouseHook mh)
        {
            //检查IntPtr是否有效
            if (OtherFuncs.GetWindowBitmap(WindowFuncs.GetRoot(intPtr)) == null)
            {
                MessageBox.Show("无效的IntPtr!");
                return null;
            }

            TaskCompletionSource<object> tc = new TaskCompletionSource<object>();
            // 新线程
            Thread t = new Thread(() =>
            {
                WindowDummyInstance windowDummyInstance = new WindowDummyInstance
                {
                    intPtr = intPtr,
                    tumbler = engine._tumbler
                };
                mh.MouseDownEvent += windowDummyInstance.OnMouseDown;
                mh.MouseUpEvent += windowDummyInstance.OnMouseUp;
                mh.MouseMoveEvent += windowDummyInstance.OnMouseMove;

                windowDummyInstances.Add(windowDummyInstance);
                windowDummyInstance.Closed += (d, k) =>
                {
                    // 当窗口关闭后马上结束消息循环
                    System.Windows.Threading.Dispatcher.ExitAllFrames();
                    windowDummyInstances.Remove(windowDummyInstance);
                };
                windowDummyInstance.Show();
                // Run 方法必须调用，否则窗口一打开就会关闭
                // 因为没有启动消息循环
                System.Windows.Threading.Dispatcher.Run();
                // 这句话是必须的，设置Task的运算结果
                // 但由于此处不需要结果，故用null
                tc.SetResult(null);
            })
            {
                IsBackground = true,
                Name = intPtr.ToString()
            };
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            // 新线程启动后，将Task实例返回以便支持 await 操作符
            return tc.Task;
        }
        
        /*public bool NewDummy(IntPtr intPtr, MouseHook.MouseHook mh)
        {
            //检查IntPtr是否有效
            if (OtherFuncs.GetWindowBitmap(WindowFuncs.GetRoot(intPtr)) == null)
            {
                MessageBox.Show("无效的IntPtr!");
                return false;
            }

            Thread thread = new Thread(() =>
            {
                WindowDummyInstance windowDummyInstance = new WindowDummyInstance
                {
                    intPtr = intPtr,
                    tumbler = engine._tumbler
                };
                mh.MouseDownEvent += windowDummyInstance.OnMouseDown;
                mh.MouseUpEvent += windowDummyInstance.OnMouseUp;
                mh.MouseMoveEvent += windowDummyInstance.OnMouseMove;
                windowDummyInstance.Show();
                windowDummyInstances.Add(windowDummyInstance);
            })
            {
                IsBackground = true,
                Name = intPtr.ToString()
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return true;
        }*/
    }
}