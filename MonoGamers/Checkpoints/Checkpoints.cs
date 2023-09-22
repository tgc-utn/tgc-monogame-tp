using Microsoft.Xna.Framework;
using MonoGamers.Collisions;

namespace MonoGamers.Checkpoints
{
    public class Checkpoint 
    {
        public Vector3 Position {get;set;}
        public BoundingBox BoundingBox {get;set;}
        public Checkpoint (Vector3 position)
        {
            this.Position = position;
            var world = Matrix.CreateScale(500f,500f,500f) * Matrix.CreateTranslation(position);
            this.BoundingBox = BoundingVolumesExtensions.FromMatrix(world);
        }
        public bool IsWithinBounds(Vector3 position)
        {
            var BoundingSphere = new BoundingSphere(position, 5f);
            return BoundingBox.Intersects(BoundingSphere);
        }
    }
}