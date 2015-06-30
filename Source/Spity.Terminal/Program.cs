using System;
using System.Threading;

namespace Spity.Terminal
{
    internal class Program
    {
        private static void Main()
        {
            Run();
        }

        private static bool ProcessSpecialKey(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                case ConsoleKey.Escape:
                case ConsoleKey.Spacebar:
                    return true;
            }
            return false;
        }

        private static void Run()
        {
            using (var serviceBuilder = new ServiceBuilder())
            {
                serviceBuilder.BuildApplication();
                SpinWait.SpinUntil(() => ProcessSpecialKey(Console.ReadKey()));
            }
        }
    }
}
