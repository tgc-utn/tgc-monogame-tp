using System;
using Microsoft.Xna.Framework;
namespace TGC.MonoGame.InsaneGames.Maps
{
    class SpawnableSpace 
    {
        //(Size, Center)
        private (Vector3, Vector3)[] Areas;
        private Random Random;
        public SpawnableSpace((Vector3, Vector3)[] areas)
        {
            Areas = areas;
            Random = new Random();
        }

        public Vector3 GetSpawnPoint(bool floorLevel)
        {
            var area = Areas[Random.Next(0, Areas.Length)];
            float x, y, z;
            x = (float) Random.NextDouble() * area.Item1.X - area.Item1.X / 2 + area.Item2.X;
            y = floorLevel ? area.Item2.Y - area.Item1.Y / 2 : 
                (float) Random.NextDouble() * area.Item1.Y - area.Item1.Y / 2 + area.Item2.Y;
            z = (float) Random.NextDouble() * area.Item1.Z - area.Item1.Z / 2 + area.Item2.Z;
            return new Vector3(x, y, z);
        }
    }
}