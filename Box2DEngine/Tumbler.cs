using System;
using System.Numerics;
using System.Windows.Forms;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Joints;

namespace Box2DEngine
{
    public class Tumbler
    {
        public World World;
        public Body body;
        public float PIXEL_TO_METER = 10;

        public Tumbler()
        {
            World = new World();

            var bd = new BodyDef
            {
                BodyType = BodyType.DynamicBody,
                Position = new Vector2(0.0f, 10.0f)
            };
            body = World.CreateBody(bd);

            //var h = Screen.PrimaryScreen.Bounds.Height;//获取含任务栏的屏幕大小
            //var w = Screen.PrimaryScreen.Bounds.Width;
            var w = SystemInformation.WorkingArea.Width;//获取不含任务栏的屏幕大小
            var h = SystemInformation.WorkingArea.Height;
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

            var shape = new PolygonShape();
            shape.SetAsBox(0.125f, 0.125f);
            body.CreateFixture(shape, 1.0f);

            /*
            Body ground;
            {
                var bd = new BodyDef();
                ground = World.CreateBody(bd);
            }

            {
                var bd = new BodyDef
                {
                    BodyType = BodyType.DynamicBody,
                    AllowSleep = false,
                    Position = new Vector2(0.0f, 10.0f)
                };
                var body = World.CreateBody(bd);

                var shape = new PolygonShape();
                shape.SetAsBox(0.5f, 10.0f, new Vector2(10.0f, 0.0f), 0.0f);
                body.CreateFixture(shape, 5.0f);
                shape.SetAsBox(0.5f, 10.0f, new Vector2(-10.0f, 0.0f), 0.0f);
                body.CreateFixture(shape, 5.0f);
                shape.SetAsBox(10.0f, 0.5f, new Vector2(0.0f, 10.0f), 0.0f);
                body.CreateFixture(shape, 5.0f);
                shape.SetAsBox(10.0f, 0.5f, new Vector2(0.0f, -10.0f), 0.0f);
                body.CreateFixture(shape, 5.0f);

                var jd = new RevoluteJointDef
                {
                    BodyA = ground,
                    BodyB = body,
                    LocalAnchorA = new Vector2(0.0f, 10.0f),
                    LocalAnchorB = new Vector2(0.0f, 0.0f),
                    ReferenceAngle = 0.0f,
                    MotorSpeed = 0.05f * Settings.Pi,
                    MaxMotorTorque = 1e8f,
                    EnableMotor = true
                };
                _joint = (RevoluteJoint)World.CreateJoint(jd);
            }

            _count = 0;*/

        }

        public void Step()
        {
            World.Step(1 / 60f, 8, 3);
            /*
            if (_count < Count)
            {
                var bd = new BodyDef
                {
                    BodyType = BodyType.DynamicBody,
                    Position = new Vector2(0.0f, 10.0f)
                };
                var body = World.CreateBody(bd);

                var shape = new PolygonShape();
                shape.SetAsBox(0.125f, 0.125f);
                body.CreateFixture(shape, 1.0f);

                ++_count;
            }*/
            Console.WriteLine(body.GetPosition());
        }
    }
}