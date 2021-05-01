using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Sources.Entities
{
    class XWing : Entity
    {
        public XWing(Vector3 position, Quaternion rotation) : base(ModelManager.XWing, position, rotation, Vector3.One) { }
    }
}