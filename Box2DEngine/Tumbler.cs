using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Forms;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Joints;
using WindowControl;
using System.Runtime.InteropServices;
using Rotate;
using System.Drawing;

namespace Box2DEngine
{
    public class UserData
    {
        public IntPtr intPtr_p;//根窗口的IntPtr
        public IntPtr this_intPtr;//Dummy的IntPtr
        public System.Drawing.Rectangle rect;//保存的的窗口矩形
    }

    public class Tumbler
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        public World World;
        public Body body;
        public float PIXEL_TO_METER = 30;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public int X;
            public int Y;
        };
        public static System.Drawing.Point GetMousePosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new System.Drawing.Point(w32Mouse.X, w32Mouse.Y);
        }

        public Tumbler()
        {
            World = new World
            {
                Gravity = new Vector2(0.0f, -50.0f),
                AllowSleep = true
            };

            var h = Screen.PrimaryScreen.Bounds.Height;//获取含任务栏的屏幕大小
            var w = Screen.PrimaryScreen.Bounds.Width;
            Vector2 offset = new Vector2(0, 0);
            //var w = SystemInformation.WorkingArea.Width;//获取不含任务栏的屏幕大小
            //var h = SystemInformation.WorkingArea.Height;
            var wallDef = new BodyDef();
            var wallBody = World.CreateBody(wallDef);

            EdgeShape wallShape = new EdgeShape();
            wallShape.SetTwoSided(new Vector2(offset.X, offset.Y), new Vector2(w / PIXEL_TO_METER + offset.X, offset.Y));
            wallBody.CreateFixture(wallShape, 0);//下
            wallShape.SetTwoSided(new Vector2(w / PIXEL_TO_METER + offset.X, offset.Y), new Vector2(w / PIXEL_TO_METER + offset.X, h / PIXEL_TO_METER + offset.Y));
            wallBody.CreateFixture(wallShape, 0);//右
            wallShape.SetTwoSided(new Vector2(w / PIXEL_TO_METER + offset.X, h / PIXEL_TO_METER + offset.Y), new Vector2(offset.X, h / PIXEL_TO_METER + offset.Y));
            wallBody.CreateFixture(wallShape, 0);//上
            wallShape.SetTwoSided(new Vector2(offset.X, h / PIXEL_TO_METER + offset.Y), new Vector2(offset.X, offset.Y));
            wallBody.CreateFixture(wallShape, 0);//左
        }

        public void Step()
        {
            World.Step(1 / 60f, 8, 3);
            if (GetConsoleWindow() != IntPtr.Zero)
                Console.Clear();

            /*LinkedList<Body> _bodyList = World.BodyList;
            Body[] _ = new Body[_bodyList.Count];
            _bodyList.CopyTo(_, 0);
            foreach (Body body in _)
            {
                if (body.UserData != null)
                {
                    if (body.UserData.GetType() == typeof(IntPtr))
                    {
                        var output = WorldToProcessing(body.GetTransform().Position);
                        if (OtherFuncs.IsDraging((IntPtr)body.UserData))
                        {
                            //body.SetTransform(new Vector2(WindowFuncs.GetClientRectangle((IntPtr)body.UserData).X / PIXEL_TO_METER, WindowFuncs.GetClientRectangle((IntPtr)body.UserData).Y / PIXEL_TO_METER), body.GetTransform().Rotation.Angle);
                            body.ApplyLinearImpulseToCenter(new Vector2(OtherFuncs.pointDelta.X, OtherFuncs.pointDelta.Y), true);
                            //WindowFuncs.SetWindowPos((IntPtr)body.UserData, -2, (int)output.X, (int)output.Y, 0, 0, 1 | 4);
                        }
                        else
                        {
                            mPoint p1 = ProcessingToWorld(new Vector2(OtherFuncs.pointDelta.X, OtherFuncs.pointDelta.Y));//世界坐标的p1
                            var area = WindowFuncs.GetWindowRectangle((IntPtr)body.UserData);
                            if (OtherFuncs.pointDelta.X > area.Left && OtherFuncs.pointDelta.X < area.Right && OtherFuncs.pointDelta.Y > area.Top && OtherFuncs.pointDelta.Y < area.Bottom)
                            {
                                mPoint p2 = body.GetPosition();//世界坐标的p2
                                Rotate.Rotate.RotateAngle(p1, p2, body.GetAngle() * (180 / Math.PI) % 360, out mPoint p3);
                                //p3 = WorldToProcessing(new Vector2((float)p3.X, (float)p3.Y));
                                WindowFuncs.SetWindowPos((IntPtr)body.UserData, -2, (int)((int)output.X + (p1.X - p3.X)), (int)((int)output.Y + (p1.Y - p3.Y)), 0, 0, 1 | 4);
                            }
                            else
                            {
                                WindowFuncs.SetWindowPos((IntPtr)body.UserData, -2, (int)output.X, (int)output.Y, 0, 0, 1 | 4);
                            }
                        }
                    }
                }
            }*/

            GlobalEvent.Register.UpdateEventAction();
        }

        public void AddBody(IntPtr intPtr, Vector2 targetPos = new Vector2(), object UserData = null)
        {
            Rectangle rect = WindowFuncs.GetWindowRectangle(intPtr);
            BodyDef bodyDef = new BodyDef
            {
                BodyType = BodyType.DynamicBody,
                Position = ProcessingToWorld(targetPos)
            };
            PolygonShape shape = new PolygonShape();
            FixtureDef fixtureDef = new FixtureDef
            {
                Shape = shape
            };
            fixtureDef.Restitution = 0.5f;
            fixtureDef.RestitutionThreshold = 80.0f;
            fixtureDef.Density = 1.0f;
            shape.SetAsBox(rect.Width / 2 / PIXEL_TO_METER, rect.Height / 2 / PIXEL_TO_METER);
            body = World.CreateBody(bodyDef);
            body.CreateFixture(fixtureDef);
            body.BodyType = BodyType.DynamicBody;
            body.UserData = UserData;
            body.SetTransform(new Vector2(rect.X / PIXEL_TO_METER, rect.Y / PIXEL_TO_METER), 0);
        }

        public void AddImpulse(IntPtr target, Vector2 impulse)
        {
            Body body = GetBody(target);
            if (body.UserData != null)
            {
                UserData userData = body.UserData as UserData;
                if (userData.intPtr_p == target)
                {
                    //body.ApplyLinearImpulse(Impulse, null, true);
                    body.ApplyLinearImpulseToCenter(impulse, true);

                }
            }

        }
        public void RotateBody(IntPtr intPtr, float angle)
        {
            Body body = GetBody(intPtr);
            if (body.UserData != null)
            {
                UserData userData = body.UserData as UserData;
                if (userData.intPtr_p == intPtr)
                {
                    body.GetTransform().Rotation.Set((float)(-(((-body.GetAngle()) * 180.0f / Math.PI) + angle) / (Math.PI * 180)));
                }
            }

        }

        public Body GetBody(IntPtr intPtr)
        {
            foreach (Body body in World.BodyList)
            {
                if (body.UserData != null)
                {
                    UserData userData = body.UserData as UserData;
                    if (userData.intPtr_p == intPtr)
                    {
                        return body;
                    }
                }
                else
                    return null;
            }
            return null;
        }
        public Rectangle GetBodyRectangle(IntPtr intPtr)
        {

            Body body = GetBody(intPtr);
            if (body.UserData != null)
            {
                UserData userData = body.UserData as UserData;
                if (userData.intPtr_p == intPtr)
                {
                    return userData.rect;
                }
            }
            return new Rectangle(999999, 999999, 999999, 999999);
        }
        public bool SetBodyRectangle(IntPtr intPtr, Rectangle rect)
        {
            Body body = GetBody(intPtr);
            if (body.UserData != null)
            {
                UserData userData = body.UserData as UserData;
                PolygonShape fixture = body.FixtureList[0].Shape as PolygonShape;
                fixture.SetAsBox(rect.Width / 2 / PIXEL_TO_METER, rect.Height / 2 / PIXEL_TO_METER);
                userData.rect = rect;
                return true;
            }
            return false;
        }

        public Vector2 ProcessingToWorld(Vector2 input)
        {
            return new Vector2(input.X / PIXEL_TO_METER, (-input.Y + Screen.PrimaryScreen.Bounds.Height) / PIXEL_TO_METER);
        }
        public Vector2 WorldToProcessing(Vector2 input)
        {
            return new Vector2(input.X * PIXEL_TO_METER, -(input.Y * PIXEL_TO_METER - Screen.PrimaryScreen.Bounds.Height));
        }
    }
}