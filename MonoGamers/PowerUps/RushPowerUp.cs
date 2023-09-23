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

        public override void Activate(MonoSphere sphere)
        {
            throw new NotImplementedException();
        }
    }
}
