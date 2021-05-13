using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.ConcreteEntities;

namespace TGC.MonoGame.TP
{
    internal class DeathStar
    {
        internal const float trenchScale = 15f;
        internal const float trenchSize = 28.2857f * trenchScale;
        private const int radius = 20;

        internal void Create()
        {
            for (int x = -radius; x < radius; x++)
                for (int z = -radius; z < radius; z++)
                    if (z != 0)
                        new TrenchPlain().Instantiate(new Vector3(x * trenchSize, -100f, z * trenchSize));

            for (int x = -radius; x < radius; x++)
                new Trench().Instantiate(new Vector3(x * trenchSize, -100f, 0));
        }
    }
}