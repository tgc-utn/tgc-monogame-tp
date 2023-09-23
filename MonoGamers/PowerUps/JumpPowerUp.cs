using Microsoft.Xna.Framework;
using MonoGamers.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGamers.PowerUps
{
    internal class JumpPowerUp : PowerUp
    {
        public JumpPowerUp(Vector3 position) : base(position)
        {
        }

        public override void Activate(MonoSphere sphere)
        {
            sphere.SphereJumpSpeed *= 4;
        }
    }
}
