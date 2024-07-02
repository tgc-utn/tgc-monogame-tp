using System;

namespace ThunderingTanks
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TankGame())
                game.Run();
        }
    }
}
