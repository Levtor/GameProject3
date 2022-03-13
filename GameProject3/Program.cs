using System;

namespace GameProject3
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game3())
                game.Run();
        }
    }
}
