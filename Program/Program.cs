using System.Runtime.InteropServices;
using System.Threading;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Game game = new Game(854, 480, "Title");
                game.Start();
            } catch (Exception ex)
            {
                Console.WriteLine("ERROR STARTING GAME" + ex.ToString());
            }
        }
    }
}