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

        public bool Activated;
        protected PowerUp(Vector3 position)
        {
            Activated = false;
            Position = position;
            var world = Matrix.CreateScale(100f, 100f, 100f) * Matrix.CreateTranslation(position);
            BoundingBox = BoundingVolumesExtensions.FromMatrix(world);
        }
        public bool IsWithinBounds(Vector3 position)
        {
            var BoundingSphere = new BoundingSphere(position, 5f);
            return BoundingBox.Intersects(BoundingSphere);
        }

        public void ActivateIfBounding(Simulation Simulation, MonoSphere sphere)
        {
            BodyReference SphereBody = Simulation.Bodies.GetBodyReference(sphere.SphereHandle);
            if (IsWithinBounds(SphereBody.Pose.Position)) Activate(sphere);
        }
        public abstract void Activate(MonoSphere Sphere);

    }
}
