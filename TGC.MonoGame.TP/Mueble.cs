using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class Mueble : IElemento
    {
        private const string UbicacionMuebles = "Models/Muebles/";
        private float Scale = 1f;
        private Vector3 Position;

        public Mueble(string nombreMueble, float escala, Vector3 posicionInicial) : base(UbicacionMuebles+nombreMueble+"/"+nombreMueble,posicionInicial){
            World *= Matrix.CreateScale(escala);

            //no se si es necesario guardarla
            Position = posicionInicial;
        }
    }
}
