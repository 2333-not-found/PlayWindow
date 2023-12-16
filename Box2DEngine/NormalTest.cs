using System;
using System.Text;
using System.Threading;
using Box2DSharp.Dynamics;
using Box2DEngine.Framework;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Box2DEngine
{
    public class NormalTest
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        public FixedUpdate FixedUpdate;
        public Tumbler _tumbler;
        private readonly CancellationTokenSource _stopToken = new CancellationTokenSource();

        public void Run()
        {
            TestClass.Test();

            if (GetConsoleWindow() != IntPtr.Zero)
                Console.Clear();
            Console.WriteLine("显示区域大小：" + Screen.PrimaryScreen.Bounds.Width + "x" + Screen.PrimaryScreen.Bounds.Height);

            _tumbler = new Tumbler();
            FixedUpdate = new FixedUpdate { UpdateCallback = Step };
            while (!_stopToken.IsCancellationRequested)
            {
                FixedUpdate.Tick();
            }
        }

        public Profile MaxProfile;
        public Profile TotalProfile;
        public bool Pause;
        public bool SingleStep;

        private readonly StringBuilder _sb = new StringBuilder();

        public void Stop()
        {
            _stopToken.Cancel();
        }

        private long _lastOutput;

        private void Step()
        {
            if (GetConsoleWindow() != IntPtr.Zero && Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.P:
                        Pause = !Pause;
                        break;

                    case ConsoleKey.O:
                        SingleStep = true;
                        break;
                }
            }

            if (!Pause)
            {
                _tumbler.Step();
            }
            else
            {
                if (SingleStep)
                {
                    SingleStep = false;
                    _tumbler.Step();
                }
            }

            var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var p = _tumbler.World.Profile;

            // Track maximum profile times
            MaxProfile.Step = Math.Max(MaxProfile.Step, p.Step);
            MaxProfile.Collide = Math.Max(MaxProfile.Collide, p.Collide);
            MaxProfile.Solve = Math.Max(MaxProfile.Solve, p.Solve);
            MaxProfile.SolveInit = Math.Max(MaxProfile.SolveInit, p.SolveInit);
            MaxProfile.SolveVelocity = Math.Max(MaxProfile.SolveVelocity, p.SolveVelocity);
            MaxProfile.SolvePosition = Math.Max(MaxProfile.SolvePosition, p.SolvePosition);
            MaxProfile.SolveTOI = Math.Max(MaxProfile.SolveTOI, p.SolveTOI);
            MaxProfile.Broadphase = Math.Max(MaxProfile.Broadphase, p.Broadphase);

            TotalProfile.Step += p.Step;
            TotalProfile.Collide += p.Collide;
            TotalProfile.Solve += p.Solve;
            TotalProfile.SolveInit += p.SolveInit;
            TotalProfile.SolveVelocity += p.SolveVelocity;
            TotalProfile.SolvePosition += p.SolvePosition;
            TotalProfile.SolveTOI += p.SolveTOI;
            TotalProfile.Broadphase += p.Broadphase;

            var aveProfile = new Profile();
            if (FixedUpdate.UpdateTime.FrameCount > 0)
            {
                var scale = 1.0f / FixedUpdate.UpdateTime.FrameCount;
                aveProfile.Step = scale * TotalProfile.Step;
                aveProfile.Collide = scale * TotalProfile.Collide;
                aveProfile.Solve = scale * TotalProfile.Solve;
                aveProfile.SolveInit = scale * TotalProfile.SolveInit;
                aveProfile.SolveVelocity = scale * TotalProfile.SolveVelocity;
                aveProfile.SolvePosition = scale * TotalProfile.SolvePosition;
                aveProfile.SolveTOI = scale * TotalProfile.SolveTOI;
                aveProfile.Broadphase = scale * TotalProfile.Broadphase;
            }

            if (now > _lastOutput)
            {
                _lastOutput = now + 500;
                _sb.AppendLine(Pause ? "****PAUSED****".PadRight(120) : "****RUNNING****".PadRight(120));
                _sb.AppendLine($"FPS {FixedUpdate.UpdateTime.FramePerSecond}, ms {FixedUpdate.UpdateTime.Elapsed.TotalMilliseconds}".PadRight(120));
                _sb.AppendLine($"step [ave] (max) = {p.Step} [{aveProfile.Step}] ({MaxProfile.Step})".PadRight(120));
                _sb.AppendLine($"collide [ave] (max) = {p.Collide} [{aveProfile.Collide}] ({MaxProfile.Collide})".PadRight(120));
                _sb.AppendLine($"solve [ave] (max) = {p.Solve} [{aveProfile.Solve}] ({MaxProfile.Solve})".PadRight(120));
                _sb.AppendLine($"solve init [ave] (max) = {p.SolveInit} [{aveProfile.SolveInit}] ({MaxProfile.SolveInit})".PadRight(120));
                _sb.AppendLine($"solve velocity [ave] (max) = {p.SolveVelocity} [{aveProfile.SolveVelocity}] ({MaxProfile.SolveVelocity})".PadRight(120));
                _sb.AppendLine($"solve position [ave] (max) = {p.SolvePosition} [{aveProfile.SolvePosition}] ({MaxProfile.SolvePosition})".PadRight(120));
                _sb.AppendLine($"solveTOI [ave] (max) = {p.SolveTOI} [{aveProfile.SolveTOI}] ({MaxProfile.SolveTOI})".PadRight(120));
                _sb.AppendLine($"broad-phase [ave] (max) = {p.Broadphase} [{aveProfile.Broadphase}] ({MaxProfile.Broadphase})".PadRight(120));

                //Console.WriteLine(_sb.ToString());
                _sb.Clear();
            }
        }
    }

    public class TestClass
    {
        [DllImport(@"Box2DWrapper.dll", EntryPoint = "Add")]
        extern static int Add(int a, int b);
        [DllImport(@"Box2DWrapper.dll", EntryPoint = "WriteString")]
        extern unsafe static void WriteString(char* c);
        [DllImport(@"Box2DWrapper.dll", EntryPoint = "AddInt")]
        extern unsafe static void AddInt(int* i);
        [DllImport(@"Box2DWrapper.dll", EntryPoint = "AddIntArray")]
        extern unsafe static void AddIntArray(int* firstElement, int arraylength);
        [DllImport(@"Box2DWrapper.dll", EntryPoint = "GetArrayFromCPP")]
        extern unsafe static int* GetArrayFromCPP();



        //定义一个委托，返回值为空，存在一个整型参数
        public delegate void CSCallback(int tick);
        //定义一个用于回调的方法，与前面定义的委托的原型一样
        //该方法会被C++所调用
        static void CSCallbackFunction(int tick)
        {
            Console.WriteLine(tick.ToString());

        }
        //定义一个委托类型的实例，
        //在主程序中该委托实例将指向前面定义的CSCallbackFunction方法
        static CSCallback callback;


        //这里使用CSCallback委托类型来兼容C++里的CPPCallback函数指针
        [DllImport(@"Box2DWrapper.dll", EntryPoint = "SetCallback")]
        extern static void SetCallback(CSCallback callback);

        [StructLayout(LayoutKind.Sequential)]
        struct Vector3
        {
            public float X, Y, Z;
        }

        [DllImport(@"Box2DWrapper.dll", EntryPoint = "SendStructFromCSToCPP")]
        extern static void SendStructFromCSToCPP(Vector3 vector);

        public static void Test()
        {
            int c = Add(1, 2);
            Console.WriteLine(c);
            //Console.WriteLine(Environment.CurrentDirectory + Environment.NewLine);
            //因为使用指针，因为要声明非安全域
            unsafe
            {
                //在传递字符串时，将字符所在的内存固化，
                //并取出字符数组的指针
                fixed (char* p = &("hello".ToCharArray()[0]))
                {
                    //调用方法
                    WriteString(p);
                }

            }
            unsafe
            {
                // 调用C++中的AddInt方法
                int i = 10;

                AddInt(&i);
                Console.WriteLine(i);

                //调用C++中的AddIntArray方法将C#中的数据传递到C++中，并在C++中输出
                int[] CSArray = new int[10];
                for (int iArr = 0; iArr < 10; iArr++)
                {
                    CSArray[iArr] = iArr;
                }
                fixed (int* pCSArray = &CSArray[0])
                {
                    AddIntArray(pCSArray, 10);
                }
                //调用C++中的GetArrayFromCPP方法获取一个C++中建立的数组
                int* pArrayPointer = null;
                pArrayPointer = GetArrayFromCPP();
                for (int iArr = 0; iArr < 10; iArr++)
                {
                    Console.WriteLine(*pArrayPointer);
                    pArrayPointer++;
                }
            }




            //让委托指向将被回调的方法
            callback = CSCallbackFunction;
            //将委托传递给C++
            SetCallback(callback);

            //建立一个Vector3的实例
            Vector3 vector = new Vector3() { X = 10, Y = 20, Z = 30 };
            //将vector传递给C++并在C++中输出
            SendStructFromCSToCPP(vector);


            Console.Read();
        }
    }
}