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

        public override void Activate(MonoSphere sphere)
        {
            sphere.SphereSideSpeed *= 4;
        }
    }
}
