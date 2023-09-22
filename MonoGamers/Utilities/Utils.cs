using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using NumericVector3 = System.Numerics.Vector3;
using MonoGamers.Checkpoints;

namespace MonoGamers.Utilities
{
    public class Utils
    {
        public static NumericVector3 ToNumericVector3(Vector3 v)
        {
            return new NumericVector3(v.X, v.Y, v.Z);
        }
    }
}
