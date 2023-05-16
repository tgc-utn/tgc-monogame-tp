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
        }

        public void AddDinamico( IElementoDinamico e ){
            e.SetPosicionInicial(e.GetPosicionInicial()+PosicionInicial);
            ElementosDinamicos.Add(e);
        }
        public void AddElemento( Elemento e ){
            e.SetPosicionInicial(e.GetPosicionInicial()+PosicionInicial);
            Elementos.Add(e);
        }

        public Vector3 GetMiddlePoint() => Piso.GetMiddlePoint();
        public Vector3 GetCenter() => Piso.getCenter();
        public Vector3 GetVerticeInicio() => this.PosicionInicial;
        public Vector3 GetVerticeExtremo() => this.GetCenter()*2;
        public (Vector3 inicio, Vector3 final) GetSegmentoSuperior() =>
                    ( this.GetVerticeInicio(),
                    new Vector3(this.GetVerticeInicio().X, 0f , this.GetVerticeExtremo().Z));
        public (Vector3 inicio, Vector3 final) GetSegmentoInferior() =>
                    (new Vector3(this.GetVerticeExtremo().X, 0f , this.GetVerticeInicio().Z),
                    this.GetVerticeExtremo());
        public (Vector3 inicio, Vector3 final) GetSegmentoDerecha() =>
                    (this.GetVerticeInicio(),
                    new Vector3(this.GetVerticeExtremo().X, 0f , this.GetVerticeInicio().Z));
        public (Vector3 inicio, Vector3 final) GetSegmentoIzquierda() =>
                    (new Vector3(this.GetVerticeInicio().X, 0f , this.GetVerticeExtremo().Z),
                    this.GetVerticeExtremo());

        public int cantidadElementos(){
            // Para debuggear
            return this.Elementos.Count + this.ElementosDinamicos.Count;
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
            foreach(var e in ElementosDinamicos)
                e.Draw();
        }
    }
}