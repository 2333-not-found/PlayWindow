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

namespace Box2DEngine
{
    public class Tumbler
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        public List<IntPtr> windowDummyInstancesIntPtr;

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
            World = new World();
            World.Gravity = new Vector2(0.0f, -50.0f);
            World.AllowSleep = true;

            var h = Screen.PrimaryScreen.Bounds.Height;//获取含任务栏的屏幕大小
            var w = Screen.PrimaryScreen.Bounds.Width;
            //var w = SystemInformation.WorkingArea.Width;//获取不含任务栏的屏幕大小
            //var h = SystemInformation.WorkingArea.Height;
            var wallDef = new BodyDef();
            var wallBody = World.CreateBody(wallDef);

            float wallLineOffset = 0;
            EdgeShape wallShape = new EdgeShape();
            wallShape.SetTwoSided(new Vector2(0, 0 + wallLineOffset), new Vector2(w / PIXEL_TO_METER, 0 + wallLineOffset));
            wallBody.CreateFixture(wallShape, 0);//下
            wallShape.SetTwoSided(new Vector2(0, h / PIXEL_TO_METER - wallLineOffset), new Vector2(w / PIXEL_TO_METER, h / PIXEL_TO_METER - wallLineOffset));
            wallBody.CreateFixture(wallShape, 0);//上
            wallShape.SetTwoSided(new Vector2(0 + wallLineOffset, h / PIXEL_TO_METER), new Vector2(0 + wallLineOffset, 0));
            wallBody.CreateFixture(wallShape, 0);//左
            wallShape.SetTwoSided(new Vector2(w / PIXEL_TO_METER - wallLineOffset, h / PIXEL_TO_METER), new Vector2(w / PIXEL_TO_METER - wallLineOffset, 0));
            wallBody.CreateFixture(wallShape, 0);//右
        }

        public void Step()
        {
            World.Step(1 / 60f, 8, 3);
            if (GetConsoleWindow() != IntPtr.Zero)
                Console.Clear();

            LinkedList<Body> _bodyList = World.BodyList;
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
                            mPoint p1 = ProcessingToWorld(new Vector2(OtherFuncs.pointDelta.X, OtherFuncs.pointDelta.Y));
                            var area = WindowFuncs.GetWindowRectangle((IntPtr)body.UserData);
                            if (OtherFuncs.pointDelta.X > area.Left && OtherFuncs.pointDelta.X < area.Right && OtherFuncs.pointDelta.Y > area.Top && OtherFuncs.pointDelta.Y < area.Bottom)
                            {
                                Console.WriteLine("True!!1");
                                mPoint p2 = WorldToProcessing(new Vector2(body.GetPosition().X, body.GetPosition().Y));
                                Rotate.Rotate.RotateAngle(p1, p2, body.GetAngle() * (180 / Math.PI) % 360, out mPoint p3);
                                p3 = WorldToProcessing(new Vector2((float)p3.X, (float)p3.X));
                                WindowFuncs.SetWindowPos((IntPtr)body.UserData, -2, (int)((int)output.X + (p1.X - p3.X)), (int)((int)output.Y + (p1.Y - p3.Y)), 0, 0, 1 | 4);
                            }
                            else
                            {
                                WindowFuncs.SetWindowPos((IntPtr)body.UserData, -2, (int)output.X, (int)output.Y, 0, 0, 1 | 4);

                                Console.WriteLine("False!!1");
                            }
                        }
                    }
                }
            }

            GlobalEvent.Register.UpdateEventAction();
        }

        public void AddBody(IntPtr intPtr)
        {
            System.Drawing.Rectangle rect = WindowFuncs.GetWindowRectangle(intPtr);
            BodyDef bodyDef = new BodyDef
            {
                BodyType = BodyType.DynamicBody,
                Position = new Vector2(10.0f, 10.0f)
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
            body.UserData = intPtr;
            body.SetTransform(new Vector2(rect.X / PIXEL_TO_METER, rect.Y / PIXEL_TO_METER), 0);
        }

        public void AddImpulse(IntPtr target, Vector2 impulse)
        {
            foreach (Body body in World.BodyList)
            {
                if (body.UserData != null)
                {
                    if (body.UserData.GetType() == typeof(IntPtr))
                    {
                        if ((IntPtr)body.UserData == target)
                        {
                            //body.ApplyLinearImpulse(Impulse, null, true);
                            body.ApplyLinearImpulseToCenter(impulse, true);
                        }
                    }
                }
            }
        }
        public void RotateBody(IntPtr intPtr, float angle)
        {
            foreach (Body body in World.BodyList)
            {
                if (body.UserData != null)
                {
                    if ((IntPtr)body.UserData == intPtr)
                    {
                        body.GetTransform().Rotation.Set((float)(-(((-body.GetAngle()) * 180.0f / Math.PI) + angle) / (Math.PI * 180)));
                    }
                }
            }
        }

        public Body GetBody(IntPtr intPtr)
        {
            foreach (Body body in World.BodyList)
            {
                if (body.UserData != null)
                {
                    if ((IntPtr)body.UserData == intPtr)
                    {
                        return body;
                    }
                }
                else
                    return null;
            }
            return null;
        }

        public Vector2 WorldToProcessing(Vector2 input)
        {
            return new Vector2(input.X * PIXEL_TO_METER, Screen.PrimaryScreen.Bounds.Height - (input.Y * PIXEL_TO_METER));
        }
        public Vector2 ProcessingToWorld(Vector2 input)
        {
            return new Vector2(input.X / PIXEL_TO_METER, (Screen.PrimaryScreen.Bounds.Height - input.Y) / PIXEL_TO_METER);
        }
    }
}