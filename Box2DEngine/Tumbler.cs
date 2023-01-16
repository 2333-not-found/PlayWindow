using System;
using System.Numerics;
using System.Windows.Forms;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Joints;
using WindowControl;
using GlobalEvent;

namespace Box2DEngine
{
    public class Tumbler
    {
        public World World;
        public Body body;
        public float PIXEL_TO_METER = 30;

        public Tumbler()
        {
            World = new World();
            World.Gravity = new Vector2(0.0f, -50.0f);

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
            Console.Clear();
            if (body != null)
                Console.WriteLine(body.GetPosition() + " " + body.IsAwake);

            var _ = World.BodyList;
            foreach(var body in _)
            {
                if (body.UserData != null)
                {
                    Console.WriteLine("\n" + body.UserData.GetType());
                    if (body.UserData.GetType() == typeof(IntPtr))
                    {
                        WorldToProcessing(body.GetTransform().Position, out var output);
                        WindowFuncs.SetWindowPos((IntPtr)body.UserData, -2, (int)output.X, (int)output.Y, 0, 0, 1 | 4);
                    }
                }
            }

            GlobalEvent.Register.Update();
        }

        public void AddBody(IntPtr intPtr)
        {
            var rect = WindowFuncs.GetWindowRectangle(intPtr);
            var bodyDef = new BodyDef
            {
                BodyType = BodyType.DynamicBody,
                Position = new Vector2(10.0f, 10.0f)
            };
            var shape = new PolygonShape();
            var fixtureDef = new FixtureDef
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
            foreach (var body in World.BodyList)
            {
                if (body.UserData != null)
                {
                    if (body.UserData.GetType() == typeof(IntPtr))
                    {
                        if((IntPtr)body.UserData == target)
                        {
                            //body.ApplyLinearImpulse(Impulse, null, true);
                            body.ApplyLinearImpulseToCenter(impulse, true);
                        }
                    }
                }
            }
        }

        public void WorldToProcessing(in Vector2 input,out Vector2 output)
        {
            output = new Vector2(input.X * PIXEL_TO_METER, Screen.PrimaryScreen.Bounds.Height - input.Y * PIXEL_TO_METER);
        }
    }
}