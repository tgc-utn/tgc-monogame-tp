using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Sources.Entities
{
    class Trench2 : Entity
    {
        public Trench2(Vector3 position, Quaternion rotation) : base(ModelManager.Trench2, position, rotation, Vector3.One / 100f) { }
    }
}