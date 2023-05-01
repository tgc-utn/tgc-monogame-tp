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
        protected Vector3 PuntoInicial = Vector3.Zero;
        protected Vector3 PuntoFinal;
        public readonly bool EsHorizontal;
        private Effect Efecto = TGCGame.GameContent.E_TextureShader;
        private Matrix World;

        public Pared(Vector3 puntoInicio, Vector3 puntoFinal, bool esHorizontal = false){
            PuntoInicial = puntoInicio;
            PuntoFinal = puntoFinal;
            EsHorizontal = esHorizontal;
            Ubicar(puntoInicio, puntoFinal);
        }

        ///<summary> Me dibujo de derecha a izquierda (Horizontal) o de arriba para abajo (Vertical) </summary>
        public void Ubicar(Vector3 puntoInicial, Vector3 puntoFinal){
            PuntoInicial = puntoInicial;
            PuntoFinal = puntoFinal;

            //Largo = largoAbsoluto;
            Largo = (EsHorizontal)? puntoFinal.Z - puntoInicial.Z : puntoFinal.X - puntoInicial.X;
            
            Matrix Escala       = Matrix.CreateScale(Altura,0,Largo);
            Matrix LevantarQuad = Matrix.CreateRotationZ(MathHelper.PiOver2);
            Matrix Rotacion     = EsHorizontal ? Matrix.CreateRotationY(0) : Matrix.CreateRotationY(MathHelper.PiOver2);
            Matrix Traslacion   = Matrix.CreateTranslation(PuntoInicial.X,-100f,PuntoInicial.Z);

            World = Escala * LevantarQuad * Rotacion * Traslacion ;
        }
        public void Draw(){ 
            Efecto.Parameters["World"].SetValue(World); 
            Efecto.Parameters["Texture"].SetValue(TGCGame.GameContent.T_Concreto);

            TGCGame.GameContent.G_Quad.Draw(Efecto);
        }
        
        public void SetPuntoInicio(Vector3 nuevoPunto) => PuntoInicial = nuevoPunto;
    }
}