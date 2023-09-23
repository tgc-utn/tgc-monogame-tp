using BepuPhysics;
using Microsoft.Xna.Framework;
using MonoGamers.Collisions;
using MonoGamers.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGamers.PowerUps
{
    internal abstract class PowerUp
    {
        public Vector3 Position { get; set; }
        public BoundingBox BoundingBox { get; set; }
        protected PowerUp(Vector3 position)
        {
            Position = position;
            var world = Matrix.CreateScale(500f, 500f, 500f) * Matrix.CreateTranslation(position);
            BoundingBox = BoundingVolumesExtensions.FromMatrix(world);
        }
        public bool IsWithinBounds(Vector3 position)
        {
            var BoundingSphere = new BoundingSphere(position, 5f);
            return BoundingBox.Intersects(BoundingSphere);
        }

        public void ActivateIfBounding(BodyReference bodyReference, MonoSphere sphere)
        {
            if (IsWithinBounds(bodyReference.Pose.Position)) Activate(sphere);
        }
        public abstract void Activate(MonoSphere sphere);

    }
}
