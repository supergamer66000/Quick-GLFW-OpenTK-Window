using System.Runtime.InteropServices;
using System.Threading;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creates a Console
            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool AllocConsole();

            AllocConsole();

            Game game = new Game(854, 480, "Title");
            game.Start();
        }
    }
}