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
        public Habitacion(int ancho, int alto, Vector3 posicionInicial)
        {
            Ancho = ancho;
            Alto = alto;
            PosicionInicial = posicionInicial;

            ElementosDinamicos = new List<IElementoDinamico>();
            Elementos = new List<IElemento>();
            Piso = new Piso(ancho, alto, posicionInicial);

            Paredes = new List<Pared>();
            Paredes.Add(Pared.Arriba (10, 10, Vector3.Zero));
           /*  Paredes.Add(Pared.Derecha   (ancho, alto, posicionInicial + new Vector3(0, 0, ancho * 5000f)));          
            Paredes.Add(Pared.Izquierda    (ancho, alto, posicionInicial + new Vector3(ancho * 5000f, 0, ancho * 5000f)));
            Paredes.Add(Pared.Abajo     (ancho, alto, posicionInicial + new Vector3(0, 0, ancho * 5000f))); */          

            ParedQueAnda = Pared.Arriba(10, 10, Vector3.Zero);
        }

        public void Load(ContentManager content)
        {
            ParedQueAnda.Load(content);
            Piso.Load(content);

            foreach(var pared in Paredes)
                pared.Load(content);
            foreach(var elemento in ElementosDinamicos)
                elemento.Load(content);
            foreach(var elemento in Elementos)
                elemento.Load(content);  
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Piso.Draw(view, projection);
            //ParedQueAnda.Draw(view, projection);

            foreach(var pared in Paredes)
                pared.Draw(view, projection);

            foreach(var elemento in ElementosDinamicos)
                elemento.Draw(view, projection);
            foreach(var elemento in Elementos)
                elemento.Draw(view, projection);
        }
    }
}