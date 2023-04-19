using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    public class Pared{
        private int Ancho;
        private int Alto;
        private Vector3 PosicionInicial;
        private bool EsHorizontal;
        private Effect Efecto = TGCGame.GameContent.E_BasicShader;
        private Matrix World = Matrix.Identity;

        private Pared(int ancho, int alto, Vector3 posicionInicial,bool esHorizontal){
            Ancho = ancho;
            Alto = alto;
            PosicionInicial = posicionInicial;
            EsHorizontal = esHorizontal;
        }

        public static Pared Izquierda(int ancho, int alto, Vector3 posicionInicial){
            return new Pared(ancho,alto, posicionInicial + new Vector3(0f, 0f, alto*500f) ,false);
        }
        public static Pared Derecha(int ancho, int alto, Vector3 posicionInicial){
            return new Pared(ancho,alto,posicionInicial + new Vector3(0f, 0f, alto*500f) ,false);
        }
        public static Pared Arriba(int ancho, int alto, Vector3 posicionInicial){
            return new Pared(ancho,alto,posicionInicial + new Vector3(ancho*250, 0f, alto*500f) ,true);
        }
        public static Pared Abajo(int ancho, int alto, Vector3 posicionInicial){
            return new Pared(ancho,alto,posicionInicial + new Vector3(ancho*250f, 0f, alto*500f) ,true);
        }

        /*public void Load(ContentManager Content)
        {
            Efecto = Content.Load<Effect>("Effects/BasicShader");

            Efecto.Parameters["DiffuseColor"].SetValue(new Color(117, 115, 162).ToVector3());
            Matrix Rotacion = !EsHorizontal ? Matrix.Identity : Matrix.CreateRotationY(MathHelper.PiOver2);
            Efecto.Parameters["World"].SetValue(
                Matrix.CreateScale(Ancho*500f, 0f, Alto*500f) *
                Matrix.CreateRotationX(MathHelper.PiOver2) *
                Rotacion *
                Matrix.CreateTranslation(PosicionInicial));  
        }*/

        public void Draw(){ 

            Efecto.Parameters["DiffuseColor"].SetValue(new Color(117, 115, 162).ToVector3());
            Matrix Rotacion = !EsHorizontal ? Matrix.Identity : Matrix.CreateRotationY(MathHelper.PiOver2);
            World = Matrix.Identity * Matrix.CreateScale(Ancho*500f, 0f, Alto*500f) * Matrix.CreateRotationX(MathHelper.PiOver2) * Rotacion;  
            Efecto.Parameters["World"].SetValue(World); 
            TGCGame.GameContent.G_Quad.Draw(Efecto);
        }

        public static implicit operator Pared(List<Pared> v)
        {
            throw new NotImplementedException();
        }
    }
}