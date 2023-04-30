using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public abstract class IHabitacion
    {
        private int Ancho;
        private int Alto;
        private Vector3 PosicionInicial;
        internal Piso Piso;
        private List<Pared> Paredes;
        private List<Puerta> Puertas;
        private List<IElementoDinamico> ElementosDinamicos;
        internal List<Elemento> Elementos;

        //Ancho y Alto en Cantidad de Baldosas
        public IHabitacion(int ancho, int alto, Vector3 posicionInicial)
        {
            Ancho = ancho;
            Alto = alto;
            PosicionInicial = posicionInicial;

            ElementosDinamicos = new List<IElementoDinamico>();
            Elementos = new List<Elemento>();

            Piso = new Piso(ancho, alto, posicionInicial); // default

            Paredes = new List<Pared>();
            Puertas = new List<Puerta>();
        }

        public void AddDinamico( IElementoDinamico e ){
            e.SetPosicionInicial(e.GetPosicionInicial()+PosicionInicial);
            Elementos.Add(e);
        }
        public void AddElemento( Elemento e ){
            e.SetPosicionInicial(e.GetPosicionInicial()+PosicionInicial);
            Elementos.Add(e);
        }

        public Vector3 getCenter(){
            return Piso.getCenter();
        }
        public int cantidadElementos(){
            return this.Elementos.Count + this.ElementosDinamicos.Count;
        }

        public void AddPared( Pared pared ){
            Paredes.Add(pared);
        }

        public void AddPuerta( Puerta puerta ){
            Puertas.Add(puerta);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState){
            foreach(var e in ElementosDinamicos){
                e.Update(gameTime, keyboardState);
            }
            return;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Piso.Draw(view, projection);

            foreach(var puerta in Puertas)
                puerta.Draw();
            foreach(var pared in Paredes)
                pared.Draw();

            this.DrawDinamicos();
            this.DrawElementos();
        }
        public virtual void DrawDinamicos(){
            foreach(var e in ElementosDinamicos)
                e.Draw();
        }
        public virtual void DrawElementos(){
            foreach(var e in Elementos)
                e.Draw();
        }
    }
}