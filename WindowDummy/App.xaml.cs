using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Box2DEngine;
using Box2DSharp;
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

        public NormalTest engine = new NormalTest();

        public void StartEngine()
        {
            Thread thread = new Thread(new ThreadStart(EngineThread));
            thread.Name = "EngineThread";
            thread.Start();
        }

        public void EngineThread()
        {
            engine.Run();
        }

        public void NewDummy(IntPtr intPtr)
        {
            //检查IntPtr是否有效
            if (OtherFuncs.GetWindowBitmap(WindowFuncs.GetRoot(intPtr)) == null)
            {
                MessageBox.Show("无效的IntPtr!");
                return;
            }
            WindowDummyInstance windowDummyInstance = new WindowDummyInstance();
            windowDummyInstance.intPtr = intPtr;
            windowDummyInstance.tumbler = engine._tumbler;
            windowDummyInstance.Start();
            windowDummyInstance.Show();
            windowDummyInstances.Add(windowDummyInstance);
            engine._tumbler.AddBody(intPtr);
        }
    }
}
