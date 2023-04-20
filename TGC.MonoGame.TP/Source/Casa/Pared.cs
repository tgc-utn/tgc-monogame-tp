using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    public class Pared{
        private int Ancho;
        private int Alto;
        private float Altura = 500f;
        private static float Escala = 1000f;
        private Vector3 PosicionInicial;
        private bool EsHorizontal;
        private Effect Efecto = TGCGame.GameContent.E_BasicShader;
        private Matrix World;

        private Pared(int ancho, int alto, Vector3 posicionInicial,bool esHorizontal){
            Ancho = ancho;
            Alto = alto;
            PosicionInicial = posicionInicial;
            EsHorizontal = esHorizontal;

            Matrix Traslacion = Matrix.CreateTranslation(PosicionInicial);
            Matrix Rotacion = !EsHorizontal ? Matrix.CreateRotationY(MathHelper.PiOver2) : Matrix.Identity;
            World = Matrix.Identity * Matrix.CreateScale(Altura, 0f, Alto*Escala) * Matrix.CreateRotationZ(MathHelper.PiOver2) * Rotacion * Traslacion;            
        }

        public static Pared Izquierda(int ancho, int alto, Vector3 posicionInicial){
            return new Pared(ancho,alto, posicionInicial + new Vector3(0f, 0f, 0f + 0f) ,false);
        }
        public static Pared Derecha(int ancho, int alto, Vector3 posicionInicial){
            return new Pared(ancho,alto,posicionInicial + new Vector3(0f, 0f, alto * Escala - 0f) ,false);
        }
        public static Pared Arriba(int ancho, int alto, Vector3 posicionInicial){
            return new Pared(ancho,alto,posicionInicial + new Vector3(0f + 0f, 0f, 0f) ,true);
        }
        public static Pared Abajo(int ancho, int alto, Vector3 posicionInicial){
            return new Pared(ancho,alto,posicionInicial + new Vector3(ancho * Escala - 0f, 0f, 0f) ,true);
        }
        public void Draw(){ 
  
            Efecto.Parameters["World"].SetValue(World); 
            Efecto.Parameters["DiffuseColor"].SetValue(new Color(117, 115, 162).ToVector3());

            TGCGame.GameContent.G_Quad.Draw(Efecto);
        }
    }
}