using System;

namespace Chinchulines
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new ChinchuGame())
                game.Run();
        }
    }
}
