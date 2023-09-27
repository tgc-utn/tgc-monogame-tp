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
    internal class FastPowerUp : PowerUp 
    {
        
        public FastPowerUp(Vector3 position) : base(position)
        {
        }

        public override async void Activate(MonoSphere Sphere)
        {
            if (!Activated)
            {
                Sphere.SphereSideSpeed *= 4;
                Activated = true;
                await Task.Delay(4000);
                Sphere.SphereSideSpeed /= 4;
                await Task.Delay(4000);
                Activated = false;
            }
        }
    }
}
