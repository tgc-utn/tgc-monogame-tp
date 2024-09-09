using System;
using Microsoft.Xna.Framework;

namespace Utils
{
    static class Matematicas
    {
        public static Vector3 clampV(Vector3 valor, Vector3 minimo, Vector3 maximo)
        {
            valor.X = Math.Clamp(valor.X, minimo.X, maximo.X);
            valor.Y = Math.Clamp(valor.Y, minimo.Y, maximo.Y);
            valor.Z = Math.Clamp(valor.Z, minimo.Z, maximo.Z);
            return valor; 
        }
    }
}