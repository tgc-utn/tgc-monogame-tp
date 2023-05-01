using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    public class Pared{
        private const float S_METRO = TGCGame.S_METRO;
        private float Largo;
        private float Altura = S_METRO*2;
        private Vector3 PosicionInicial = Vector3.Zero;
        private bool EsHorizontal;
        private Effect Efecto = TGCGame.GameContent.E_TextureShader;
        private Matrix World;

        public Pared(bool esHorizontal = false){
            EsHorizontal = esHorizontal;
            World = Matrix.Identity;            
        }

        public Pared Abierta(){
            return this;
        }
        public Pared Cerrada(){
            return this;
        }

        ///<summary> Me dibujo de derecha a izquierda (Horizontal) o de arriba para abajo (Vertical) </summary>
        public void Ubicar(Vector3 posicionInicial, float largo){
            PosicionInicial = posicionInicial;
            
            Largo = largo * S_METRO;

            Matrix Escala       =  EsHorizontal ? Matrix.CreateScale(Altura,0,Largo)  : Matrix.CreateScale(Altura,0,Largo);
            Matrix LevantarQuad = Matrix.CreateRotationZ(MathHelper.PiOver2);
            Matrix Rotacion     =  EsHorizontal ? Matrix.Identity                : Matrix.CreateRotationY(MathHelper.PiOver2);
            Matrix Traslacion   = Matrix.CreateTranslation(PosicionInicial.X * S_METRO,-100f,PosicionInicial.Z * S_METRO);

            World = Escala * LevantarQuad * Rotacion * Traslacion ;
        }

        public void Draw(){ 
            Efecto.Parameters["World"].SetValue(World); 
            Efecto.Parameters["Texture"].SetValue(TGCGame.GameContent.T_Concreto);

            TGCGame.GameContent.G_Quad.Draw(Efecto);
        }
    }
}