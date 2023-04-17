using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class Mueble : IElemento
    {
        private const string UbicacionMuebles = "Models/Muebles/";
        private Vector3 Position;

        public Mueble(string nombreMueble, float escala, Vector3 posicionInicial, Vector3 rotacion) : base(UbicacionMuebles+nombreMueble+"/"+nombreMueble,posicionInicial, rotacion){
            World *= Matrix.CreateScale(escala);

            //no se si es necesario guardarla
            Position = posicionInicial;
        }
    }
}
