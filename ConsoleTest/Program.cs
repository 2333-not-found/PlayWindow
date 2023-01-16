using System;
using System.Diagnostics;
using System.Linq;

namespace NETCoreTest
{
    static class Program
    {
        public static void Main(string[] args)
        {
            var test = new NormalTest();
            Console.CancelKeyPress += (sender, eventArgs) => { test.Stop(); };
            test.Run();
        }
    }
}