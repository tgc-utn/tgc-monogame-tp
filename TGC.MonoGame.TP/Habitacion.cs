using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP
{
    public class Habitacion
    {
        private int Ancho;
        private int Alto;
        private Vector3 PosicionInicial;
        private Piso Piso;
        private List<Pared> Paredes;
        private List<IElementoDinamico> ElementosDinamicos;
        private List<IElemento> Elementos;

        private Pared ParedQueAnda;

        //Ancho y Alto en Cantidad de Baldosas
        public Habitacion(int Ancho, int Alto, Vector3 PosicionInicial)
        {
            this.Ancho = Ancho;
            this.Alto = Alto;
            this.PosicionInicial = PosicionInicial;

            ElementosDinamicos = new List<IElementoDinamico>();
            Elementos = new List<IElemento>();
            Piso = new Piso(Ancho, Alto, PosicionInicial);

            Paredes = new List<Pared>();
            //Paredes.Add(new Pared(Ancho, Alto, PosicionInicial, true));
            //Paredes.Add(new Pared(Ancho, Alto, PosicionInicial + new Vector3(Ancho * 500f, 0, 0), true));
            Paredes.Add(new Pared(Ancho, Alto, PosicionInicial + new Vector3(Ancho * 5000f, 0, Ancho * 5000f), false));
            Paredes.Add(new Pared(Ancho, Alto, PosicionInicial + new Vector3(0, 0, Ancho * 5000f), false));          

            ParedQueAnda = new Pared(10, 10, Vector3.Zero, false);

        }

        public void Load(ContentManager Content)
        {
            Piso.Load(Content);
            ParedQueAnda.Load(Content);

            foreach(var pared in Paredes)
                pared.Load(Content);
            foreach(var elemento in ElementosDinamicos)
                elemento.Load(Content);
            foreach(var elemento in Elementos)
                elemento.Load(Content);  
        }

        public void Draw(Matrix View, Matrix Projection)
        {
            Piso.Draw(View, Projection);
            ParedQueAnda.Draw(View, Projection);

            foreach(var pared in Paredes)
                pared.Draw(View, Projection);

            foreach(var elemento in ElementosDinamicos)
                elemento.Draw(View, Projection);
            foreach(var elemento in Elementos)
                elemento.Draw(View, Projection);
        }
    }
}