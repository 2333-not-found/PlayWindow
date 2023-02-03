using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlobalEvent;
using Box2DEngine;

namespace WindowDummy
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            GlobalEvent.Register.UpdateEvent += WindowDummyCenter.UpdateEvent;
        }
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

        public static void UpdateEvent()
        {

        }

        public void NewDummy(IntPtr intPtr)
        {
            WindowDummyInstance windowDummyInstance = new WindowDummyInstance
            {
                intPtr = intPtr
            };
            windowDummyInstance.Show();
            windowDummyInstances.Add(windowDummyInstance);
            engine._tumbler.AddBody(intPtr);
        }
    }
}
