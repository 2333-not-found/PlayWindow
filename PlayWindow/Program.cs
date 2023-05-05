using System;
using System.Reflection;
using System.Windows.Forms;

namespace PlayWindow
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public class BuildDate
    {
        public static Version Version()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }

        public static DateTime Date()
        {
            Version version = Version();
            DateTime startDate = new DateTime(2000, 1, 1, 0, 0, 0);
            TimeSpan span = new TimeSpan(version.Build, 0, 0, version.Revision * 2);
            DateTime buildDate = startDate.Add(span);
            return buildDate;
        }

        public static string ToString(string format = null)
        {
            DateTime date = Date();
            return date.ToString(format);
        }
    }
}