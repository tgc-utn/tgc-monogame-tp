using BepuPhysics;
using Microsoft.Xna.Framework;
using MonoGamers.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGamers.PowerUps
{
    internal class RushPowerUp : PowerUp
    {
        public RushPowerUp(Vector3 position) : base(position)
        {
        }

        public override async void Activate(MonoSphere Sphere)
        {
            if (!Activated)
            {
                Activated = true;
                Sphere.rushed = true;
                await Task.Delay(4000);
                Activated = false;
            }
        }
    }
}
