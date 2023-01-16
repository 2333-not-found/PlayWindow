using System;

namespace Global
{
    public class Register
    {
        public static event Action UpdateEvent;//声明一个事件 发布消息

        public static void Update()
        {
            UpdateEvent?.Invoke();
        }
    }

    public struct WindowInfo
    {
        public mPoint pos;
        public double angle;
        public IntPtr intPtr;
    }

    public struct mPoint
    {
        public double Y;
        public double X;
    }
}