using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.ConcreteEntities;

namespace TGC.MonoGame.TP
{
    internal class DeathStar
    {
        internal void Create()
        {
            new Trench().Instantiate(new Vector3(150f, 0f, 0f));
            new Trench2().Instantiate(new Vector3(200f, 0f, 0f));
        }
    }
}