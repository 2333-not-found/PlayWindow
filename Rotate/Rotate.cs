using System;
using System.Drawing;
using System.Numerics;

namespace Rotate
{
    public struct Cicular
    {
        public double X;
        public double Y;
        public double R;
    }

    public struct mPoint
    {
        public double X;
        public double Y;
        public static implicit operator mPoint(Point mPoint)
        {
            mPoint point = new mPoint();
            point.X = mPoint.X;
            point.Y = mPoint.Y;
            return point;
        }
        public static implicit operator mPoint(Vector2 mPoint)
        {
            mPoint point = new mPoint();
            point.X = mPoint.X;
            point.Y = mPoint.Y;
            return point;
        }
    }

    public class Rotate
    {
        /// <summary>
        /// 求经过三点的圆
        /// </summary>
        /// <param name="px1">坐标1</param>
        /// <param name="px2">坐标2</param>
        /// <param name="px3">坐标3</param>
        /// <param name="C">结果圆</param>
        public static void CalculateCicular(mPoint px1, mPoint px2, mPoint px3, out Cicular C)
        {
            double x1, y1, x2, y2, x3, y3;
            double a, b, c, g, e, f;
            x1 = px1.X;
            y1 = px1.Y;
            x2 = px2.X;
            y2 = px2.Y;
            x3 = px3.X;
            y3 = px3.Y;
            e = 2 * (x2 - x1);
            f = 2 * (y2 - y1);
            g = x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1;
            a = 2 * (x3 - x2);
            b = 2 * (y3 - y2);
            c = x3 * x3 - x2 * x2 + y3 * y3 - y2 * y2;
            C.X = (g * b - c * f) / (e * b - a * f);
            C.Y = (a * g - c * e) / (a * f - b * e);
            C.R = Math.Sqrt((C.X - x1) * (C.X - x1) + (C.Y - y1) * (C.Y - y1));

        }

        /// <summary>
        /// 计算坐标1以坐标2为中心旋转后坐标
        /// </summary>
        /// <param name="centrePoint">中心点</param>
        /// <param name="p1">坐标</param>
        /// <param name="Angle">旋转角度</param>
        /// <param name="p2">旋转后坐标</param>
        /// <returns>运行状态</returns>
        public static string RotateAngle(mPoint centrePoint, mPoint p1, double Angle, out mPoint p2)
        {
            try
            {
                double Rad = 0;
                Rad = Angle * Math.Acos(-1) / 180;
                p2.X = (p1.X - centrePoint.X) * Math.Cos(Rad) - (p1.Y - centrePoint.Y) * Math.Sin(Rad) + centrePoint.X;
                p2.Y = (p1.Y - centrePoint.Y) * Math.Cos(Rad) + (p1.X - centrePoint.X) * Math.Sin(Rad) + centrePoint.Y;
                return "OK";
            }
            catch (Exception ex)
            {
                p2.X = 999999;
                p2.Y = 999999;
                return ex.Message;
            }
        }

        /// <summary>
        /// 计算矩形绕中心任意角度旋转后所占区域矩形宽高
        /// </summary>
        /// <param name="width">原矩形的宽</param>
        /// <param name="height">原矩形高</param>
        /// <param name="angle">顺时针旋转角度</param>
        /// <returns></returns>
        public static Rectangle GetRotateRectangle(double width, double height, float angle)
        {
            double radian = angle * Math.PI / 180;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //只需要考虑到第四象限和第三象限的情况取大值(中间用绝对值就可以包括第一和第二象限)
            int newWidth = (int)(Math.Max(Math.Abs(width * cos - height * sin), Math.Abs(width * cos + height * sin)));
            int newHeight = (int)(Math.Max(Math.Abs(width * sin - height * cos), Math.Abs(width * sin + height * cos)));
            return new Rectangle(0, 0, newWidth, newHeight);
        }
    }
}
