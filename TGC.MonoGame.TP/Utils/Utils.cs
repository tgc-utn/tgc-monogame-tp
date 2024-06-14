using Microsoft.Xna.Framework;
using System;
using System.Linq;
using TGC.MonoGame.Samples.Collisions;
using NumericVector3 = System.Numerics.Vector3;

namespace TGC.MonoGame.TP.Utils
{

    public class Utils
    {
        public static NumericVector3 ToNumericVector3(Vector3 v)
        {
            return new NumericVector3(v.X, v.Y, v.Z);
        }

        public static float Max(float value1, float value2)
        {
            return value1 > value2 ? value1 : value2;
        }

    }
}