using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public abstract class IHabitacion
    {
        private const float S_METRO = TGCGame.S_METRO;
        public readonly int Ancho;
        public readonly int Largo;
        private Vector3 PosicionInicial;
        internal Piso Piso;
        private Pared ParedAbajo {get;set;}
        private Pared ParedArriba {get;set;}
        private Pared ParedDerecha {get;set;}
        private Pared ParedIzquierda {get;set;}
        private List<Pared> Paredes;
        private List<IElementoDinamico> ElementosDinamicos;
        internal List<Elemento> Elementos;

        //Ancho y Alto en Cantidad de Baldosas
        public IHabitacion(int metrosAncho, int metrosLargo, Vector3 posicionInicial)
        {
            Ancho = metrosAncho;
            Largo = metrosLargo;
            PosicionInicial = posicionInicial;

            ElementosDinamicos = new List<IElementoDinamico>();
            Elementos = new List<Elemento>();

            Piso = new Piso(metrosAncho, metrosLargo, posicionInicial);

            Paredes = new List<Pared>();
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
        public Vector3 getVerticeExtremo() => this.getCenter()*2;

        public int cantidadElementos(){
            // Para debuggear
            return this.Elementos.Count + this.ElementosDinamicos.Count;
        }

        public void SetParedArriba(Pared p){
            p.Ubicar(PosicionInicial, Ancho);

            ParedArriba = p;
            Paredes.Add(p);
        }
        public void SetParedDerecha(Pared p){

            p.Ubicar(PosicionInicial, Largo);

            ParedDerecha = p;
            Paredes.Add(p);
        }
        public void SetParedAbajo(Pared p){
            var posicionArranque = PosicionInicial + new Vector3(this.Largo,0f,0f);

            p.Ubicar(posicionArranque, Ancho);

            ParedAbajo = p;
            Paredes.Add(p);
        }
        public void SetParedIzquierda(Pared p){
            var posicionArranque = PosicionInicial + new Vector3(0f,0f,this.Ancho);

            p.Ubicar(posicionArranque, Largo);
            
            ParedIzquierda = p;
            Paredes.Add(p);
        }
        public void Update(GameTime gameTime, KeyboardState keyboardState){
            foreach(var e in ElementosDinamicos){
                e.Update(gameTime, keyboardState);
            }
            return;
        }

        public void Draw()
        {
            Piso.Draw();

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