using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Entities
{
    internal abstract class StaticEntity : Entity
    {
        protected Vector3 Position { get; set; }
        protected Quaternion Rotation { get; set; }
        protected abstract Vector3 Scale { get; }

        protected override Matrix GeneralWorldMatrix()
            => Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateTranslation(Position);
    }
}