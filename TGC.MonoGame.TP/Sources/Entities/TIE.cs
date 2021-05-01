using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Sources.Entities
{
    class TIE : Entity
    {
        public TIE(Vector3 position, Quaternion rotation) : base(ModelManager.TIE, position, rotation, Vector3.One / 100f) { }
    }
}