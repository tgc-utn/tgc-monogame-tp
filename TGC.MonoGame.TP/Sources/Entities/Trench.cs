using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Sources.Entities
{
    class Trench : Entity
    {
        public Trench(Vector3 position, Quaternion rotation) : base(ModelManager.Trench, position, rotation, Vector3.One / 100f) { }
    }
}