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
        private List<IElementoDinamico> MueblesDinamicos;
        private List<IElemento> Muebles;

        //Ancho y Alto en Cantidad de Baldosas
        public IHabitacion(int ancho, int alto, Vector3 posicionInicial)
        {
            Ancho = ancho;
            Alto = alto;
            PosicionInicial = posicionInicial;

            MueblesDinamicos = new List<IElementoDinamico>();
            Muebles = new List<IElemento>();
            Piso = new Piso(ancho, alto, posicionInicial); // Se carga el default

            Paredes = new List<Pared>();
            Puertas = new List<Puerta>();
        }

        public void AddDinamico( IElementoDinamico elem ){
            elem.newPosicionInicial(PosicionInicial);
            MueblesDinamicos.Add(elem);
        }
        public void AddElemento( IElemento elem ){
            elem.newPosicionInicial(PosicionInicial);
            Muebles.Add(elem);
        }

        public Vector3 getCenter(){
            return Piso.getCenter();
        }

        public void AddPared( Pared pared ){
            Paredes.Add(pared);
        }

        public void AddPuerta( Puerta puerta ){
            Puertas.Add(puerta);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState){
            foreach(var e in MueblesDinamicos){
                e.Update(gameTime, keyboardState);
            }
            return;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Piso.Draw(view, projection);
            //new Alfombra(Matrix.CreateScale(3000f, 0f, 2000f)*Matrix.CreateRotationY(MathHelper.Pi/3)*Matrix.CreateTranslation(new Vector3(-12000f,20f,200f))).Draw();

            foreach(var puerta in Puertas)
                puerta.Draw();
            foreach(var pared in Paredes)
                pared.Draw();
            foreach(var elemento in MueblesDinamicos)
                elemento.Draw(view, projection);
            foreach(var elemento in Muebles)
                elemento.Draw(view, projection);
        }
    }
}