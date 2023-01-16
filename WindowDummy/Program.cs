using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Global;

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
            Global.Register.UpdateEvent += WindowDummyCenter.UpdateEvent;
        }
    }

    public class WindowDummyCenter
    {
        public List<WindowDummyInstance> windowDummyInstances = new List<WindowDummyInstance>(3);

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
        }
    }
}
