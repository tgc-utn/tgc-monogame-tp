using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class Habitacion
    {
        private int Ancho;
        private int Alto;
        private Vector3 PosicionInicial;
        private Piso Piso;
        private Pared[] Paredes;
        private List<IElementoDinamico> ElementosDinamicos;
        private List<IElemento> Elementos;

        //Ancho y Alto en Cantidad de Baldosas
        public Habitacion(int Ancho, int Alto, Vector3 PosicionInicial)
        {
            this.Ancho = Ancho;
            this.Alto = Alto;
            this.PosicionInicial = PosicionInicial;

            Piso = new Piso(Ancho, Alto, PosicionInicial);
            
        }
    }
}