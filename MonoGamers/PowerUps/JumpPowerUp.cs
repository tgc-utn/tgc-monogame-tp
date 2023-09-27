using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using MonoGamers.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonoGamers.PowerUps
{
    internal class JumpPowerUp : PowerUp
    {
        public JumpPowerUp(Vector3 position) : base(position)
        {
        }

        public override async void Activate(MonoSphere Sphere)
        {
            if (!Activated)
            {
                Sphere.SphereJumpSpeed *= 1.005f;
                Activated = true;
                await Task.Delay(4000);
                Sphere.SphereJumpSpeed /= 1.005f;
                await Task.Delay(4000);
                Activated = false;
            }
        }
    }
}
