using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
        /// 计算坐标以圆心旋转后坐标
        /// </summary>
        /// <param name="p1">坐标</param>
        /// <param name="Angle">旋转角度</param>
        /// <param name="p2">圆心</param>
        /// <param name="p3">旋转后坐标</param>
        /// <returns>运行状态</returns>
        public static string RotateAngle(mPoint p1, mPoint p2, double Angle, out mPoint p3)
        {
            try
            {
                double Rad = 0;
                Rad = Angle * Math.Acos(-1) / 180;
                p3.X = (p2.X - p1.X) * Math.Cos(Rad) - (p2.Y - p1.Y) * Math.Sin(Rad) + p1.X;
                p3.Y = (p2.Y - p1.Y) * Math.Cos(Rad) + (p2.X - p1.X) * Math.Sin(Rad) + p1.Y;
                return "OK";
            }
            catch (Exception ex)
            {
                p3.X = 999999;
                p3.Y = 999999;
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
        public static Rectangle GetRotateRectangle(int width, int height, float angle)
        {
            double radian = angle * Math.PI / 180; ;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //只需要考虑到第四象限和第三象限的情况取大值(中间用绝对值就可以包括第一和第二象限)
            int newWidth = (int)(Math.Max(Math.Abs(width * cos - height * sin), Math.Abs(width * cos + height * sin)));
            int newHeight = (int)(Math.Max(Math.Abs(width * sin - height * cos), Math.Abs(width * sin + height * cos)));
            return new Rectangle(0, 0, newWidth, newHeight);
        }
        /// <summary>
        /// 获取原图像绕中心任意角度旋转后的图像
        /// </summary>
        /// <param name="rawImg"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Image GetRotateImage(Image srcImage, int angle)
        {
            angle = angle % 360;
            //原图的宽和高
            int srcWidth = srcImage.Width;
            int srcHeight = srcImage.Height;
            //图像旋转之后所占区域宽和高
            Rectangle rotateRec = GetRotateRectangle(srcWidth, srcHeight, angle);
            int rotateWidth = rotateRec.Width;
            int rotateHeight = rotateRec.Height;
            //目标位图
            Bitmap destImage = null;
            Graphics graphics = null;
            try
            {
                //定义画布，宽高为图像旋转后的宽高
                destImage = new Bitmap(rotateWidth, rotateHeight);
                //graphics根据destImage创建，因此其原点此时在destImage左上角
                graphics = Graphics.FromImage(destImage);
                //要让graphics围绕某矩形中心点旋转N度，分三步
                //第一步，将graphics坐标原点移到矩形中心点,假设其中点坐标（x,y）
                //第二步，graphics旋转相应的角度(沿当前原点)
                //第三步，移回（-x,-y）
                //获取画布中心点
                Point centerPoint = new Point(rotateWidth / 2, rotateHeight / 2);
                //将graphics坐标原点移到中心点
                graphics.TranslateTransform(centerPoint.X, centerPoint.Y);
                //graphics旋转相应的角度(绕当前原点)
                graphics.RotateTransform(angle);
                //恢复graphics在水平和垂直方向的平移(沿当前原点)
                graphics.TranslateTransform(-centerPoint.X, -centerPoint.Y);
                //此时已经完成了graphics的旋转

                //计算:如果要将源图像画到画布上且中心与画布中心重合，需要的偏移量
                Point Offset = new Point((rotateWidth - srcWidth) / 2, (rotateHeight - srcHeight) / 2);
                //将源图片画到rect里（rotateRec的中心）
                graphics.DrawImage(srcImage, new Rectangle(Offset.X, Offset.Y, srcWidth, srcHeight));
                //重至绘图的所有变换
                graphics.ResetTransform();
                graphics.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (graphics != null)
                    graphics.Dispose();
            }
            return destImage;
        }

        public static Bitmap KiRotate(Bitmap bmp, float angle, Color bkColor)
        {
            int w = bmp.Width + 2;
            int h = bmp.Height + 2;

            PixelFormat pf;

            if (bkColor == Color.Transparent)
            {
                pf = PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }

            Bitmap tmp = new Bitmap(w, h, pf);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);

            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
            g = Graphics.FromImage(dst);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tmp, 0, 0);
            g.Dispose();
            tmp.Dispose();
            return dst;
        }
    }
}
