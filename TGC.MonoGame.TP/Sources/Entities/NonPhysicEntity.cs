using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Entities
{
    internal abstract class NonPhysicEntity : Entity
    {
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 scale;

        internal NonPhysicEntity(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        protected override Matrix GeneralWorldMatrix()
            => Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(position);
    }
}