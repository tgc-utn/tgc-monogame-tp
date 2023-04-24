using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    public class Pared{
        private float Ancho;
        private float Altura = 1000f;
        private static float Escala = 1000f;
        private Vector3 PosicionInicial;
        private bool EsHorizontal;
        private Effect Efecto = TGCGame.GameContent.E_BasicShader;
        private Matrix World;

        private Pared(float ancho, Vector3 posicionInicial,bool esHorizontal = false){
            Ancho = ancho;
            PosicionInicial = posicionInicial;
            EsHorizontal = esHorizontal;

            Matrix Traslacion = Matrix.CreateTranslation(PosicionInicial);
            Matrix Rotacion = !EsHorizontal ? Matrix.CreateRotationY(MathHelper.PiOver2) : Matrix.Identity;
            World = Matrix.Identity * Matrix.CreateScale(Altura, 0f, Ancho*Escala) * Matrix.CreateRotationZ(MathHelper.PiOver2) * Rotacion * Traslacion;            
        }

        public static Pared Izquierda(float ancho, float alto, Vector3 posicionInicial){
            return new Pared(ancho, posicionInicial + new Vector3(0f, 0f, ancho * Escala));
            
        }
        public static Pared Derecha(float ancho, float alto, Vector3 posicionInicial){
            return new Pared(ancho, posicionInicial);
        }
        public static Pared Arriba(float ancho, float alto, Vector3 posicionInicial){
            return new Pared(ancho, posicionInicial, true);
        }
        public static Pared Abajo(float ancho, float alto, Vector3 posicionInicial){
            return new Pared(ancho, posicionInicial + new Vector3(alto * Escala - 0f, 0f, 0f) ,true);
        }
        public void Draw(){ 
            Efecto.Parameters["World"].SetValue(World); 
            Efecto.Parameters["DiffuseColor"].SetValue(Color.DarkGray.ToVector3());

            TGCGame.GameContent.G_Quad.Draw(Efecto);
        }
    }
}