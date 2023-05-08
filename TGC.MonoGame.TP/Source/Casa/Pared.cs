using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    public class Pared{
        private Matrix WorldCerrada;
        private Matrix WorldLateral1, WorldLateral2;
        private float Largo;
        private float LargoPuerta = TGCGame.S_METRO * 2f;
        public const float Grosor = TGCGame.S_METRO * 0.1f;
        public const float Altura = TGCGame.S_METRO * 2f;
        protected Vector3 PuntoInicial = Vector3.Zero;
        protected Vector3 PuntoFinal;
        public readonly bool EsHorizontal;
        private bool ConPuerta = false;
        private Effect Efecto = TGCGame.GameContent.E_TextureShader;
        private Matrix AutoProyectado;
        

        public Pared(Vector3 puntoInicio, Vector3 puntoFinal, bool esHorizontal = false){
            PuntoInicial = puntoInicio;
            PuntoFinal = puntoFinal;
            EsHorizontal = esHorizontal;
            Ubicar(puntoInicio, puntoFinal);
        }

        ///<summary> Pared Completamente Cerrada. Me dibujo de derecha a izquierda (Horizontal) o de arriba para abajo (Vertical) </summary>
        public void Ubicar(Vector3 puntoInicial, Vector3 puntoFinal){
            PuntoInicial = puntoInicial;
            PuntoFinal = puntoFinal;

            //Largo = largoAbsoluto;
            Largo = (EsHorizontal)? puntoFinal.Z - puntoInicial.Z : puntoFinal.X - puntoInicial.X - Grosor;
            
            Matrix Escala       = Matrix.CreateScale(Altura,Grosor,Largo);
            Matrix LevantarQuad = Matrix.CreateRotationZ(MathHelper.PiOver2);
            Matrix Rotacion     = EsHorizontal ? Matrix.CreateRotationY(0) : Matrix.CreateRotationY(MathHelper.PiOver2);
            Matrix Traslacion   = Matrix.CreateTranslation(PuntoInicial.X,-100f,PuntoInicial.Z);

            WorldCerrada = Escala * LevantarQuad * Rotacion * Traslacion ;
        }
        /// <summary> La distancia es un valor entre 0 y 1. Siendo 0 el principio y 1 el final</summary>
        public void AddPuerta(float distanciaPonderada){
            // La idea es generalizar esto con una lista de distancias ponderadas y una lista de matrices de mundo 
            // por segmento de pared

            ConPuerta = true;

            Matrix Escala       = Matrix.CreateScale(Altura,Grosor,Largo*distanciaPonderada);
            Matrix LevantarQuad = Matrix.CreateRotationZ(MathHelper.PiOver2);
            Matrix Rotacion     = EsHorizontal ? Matrix.CreateRotationY(0) : Matrix.CreateRotationY(MathHelper.PiOver2);
            Matrix Traslacion   = Matrix.CreateTranslation(PuntoInicial.X,-100f,PuntoInicial.Z);

            WorldLateral1 = Escala * LevantarQuad * Rotacion * Traslacion ;
            
            Escala       = Matrix.CreateScale(Altura,Grosor,Largo*(1 - distanciaPonderada) - LargoPuerta);
            Traslacion   = (EsHorizontal)? 
                            Matrix.CreateTranslation(PuntoInicial.X,-100f,PuntoInicial.Z + distanciaPonderada * Largo + LargoPuerta) :
                            Matrix.CreateTranslation(PuntoInicial.X +distanciaPonderada * Largo + LargoPuerta,-100f,PuntoInicial.Z) ;
            
            WorldLateral2 = Escala * LevantarQuad * Rotacion * Traslacion ;

        }
        public void Draw(){ 
            Draw(TGCGame.GameContent.T_MarmolNegro);
        }
        public void Draw(Texture2D textura){ 
            Efecto.Parameters["Texture"]?.SetValue(textura);

            // Generalizar para tener mas puertas
            if(ConPuerta){
                Efecto.Parameters["World"].SetValue(WorldLateral1); 
                TGCGame.GameContent.G_Cubo.Draw(Efecto);
                Efecto.Parameters["World"].SetValue(WorldLateral2); 
                TGCGame.GameContent.G_Cubo.Draw(Efecto);
            }else{
                Efecto.Parameters["World"].SetValue(WorldCerrada); 
                TGCGame.GameContent.G_Cubo.Draw(Efecto);
            }            
        }

        public void SetProjectedAuto(Matrix projectedAuto){
            AutoProyectado = projectedAuto;
        }

        public void SetEffect(Effect nuevoEfecto){
            Efecto = nuevoEfecto;
        }
        
        public void SetPuntoInicio(Vector3 nuevoPunto) => PuntoInicial = nuevoPunto;
    }
}