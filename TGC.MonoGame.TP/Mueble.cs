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
        private Vector3 Position;

        //Que reciba una MATRIZ INICIAL no una posición inicial
        public Mueble(string nombreMueble, float escala, Vector3 posicionInicial, Vector3 rotacion) : base(UbicacionMuebles+nombreMueble+"/"+nombreMueble,posicionInicial, rotacion){
            World *= Matrix.CreateScale(escala);

            //no se si es necesario guardarla
            Position = posicionInicial;
        }
    }
}
