using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Collisions;

namespace TGC.MonoGame.TP
{
    public class Pared{
        // private Matrix WorldCerrada;
        // private Matrix WorldLateral1, WorldLateral2;
        private List<Matrix> Worlds = new List<Matrix>();
        private List<float> DistanciasPuertas = new List<float>();
        private float Largo;
        private float LargoPuerta = TGCGame.S_METRO * 2f;
        public const float Grosor = TGCGame.S_METRO * 0.1f;
        public const float Altura = TGCGame.S_METRO * 2f;
        protected Vector3 PuntoInicial = Vector3.Zero;
        protected Vector3 PuntoFinal;
        public readonly bool EsHorizontal;
        // private bool ConPuerta = false;
        private Effect Efecto = TGCGame.GameContent.E_TextureShader;
        private Matrix AutoProyectado;
        private StaticHandle Handle;
        

        public Pared(Vector3 puntoInicio, Vector3 puntoFinal, bool esHorizontal = false){
            PuntoInicial = puntoInicio;
            PuntoFinal = puntoFinal;
            EsHorizontal = esHorizontal;
            Ubicar(puntoInicio, puntoFinal);
 
            var esteNumerito = (PuntoInicial.X+PuntoFinal.X)*0.5f;
            var otroNumerito = (PuntoInicial.Z+PuntoFinal.Z)*0.5f;

            Box boxito = (!EsHorizontal)? new Box(esteNumerito*2, Altura, Grosor) 
                                        : new Box(Grosor, Altura, otroNumerito*2);

            Handle = TGCGame.Simulation.Statics.Add(
                                            new StaticDescription(
                                                new Vector3(esteNumerito, Altura*0.5f, otroNumerito).ToBepu(),
                                                TGCGame.Simulation.Shapes.Add(boxito)));
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
            Matrix Traslacion   = Matrix.CreateTranslation(PuntoInicial.X,-0,PuntoInicial.Z);

            var worldCerrada = Escala * LevantarQuad * Rotacion * Traslacion ;
            Worlds.Add(worldCerrada);
        }
        /// <summary> La distancia es un valor entre 0 y 1. Siendo 0 el principio y 1 el final</summary>
        public void AddPuerta(float distanciaPonderada){
            this.DistanciasPuertas.Add(distanciaPonderada);
            this.DistanciasPuertas.Sort();
            Matrix LevantarQuad = Matrix.CreateRotationZ(MathHelper.PiOver2);
            Matrix Rotacion     = EsHorizontal ? Matrix.CreateRotationY(0) : Matrix.CreateRotationY(MathHelper.PiOver2);
            this.Worlds = new List<Matrix>();
            
            
            Matrix Escala;
            Matrix Traslacion;
            Matrix WorldAux;
            int i;
            float largoParedRestante = Largo;
            float ponderacionRestante = 1f;
            float corrimiento = 0f;

            // si agregan 2 puertas muy pegadas solo cambiar el corrimiento, no crear un mundo nuevo
            for(i = 0 ; i < DistanciasPuertas.Count ; i++){
                var largoParedActual = largoParedRestante*this.DistanciasPuertas[i] * ponderacionRestante;

                Escala       = Matrix.CreateScale(Altura,Grosor,largoParedActual);
                Traslacion   = (EsHorizontal)? 
                            Matrix.CreateTranslation(PuntoInicial.X,-0,PuntoInicial.Z + corrimiento ) :
                            Matrix.CreateTranslation(PuntoInicial.X + corrimiento ,-0,PuntoInicial.Z) ;

                WorldAux = Escala * LevantarQuad * Rotacion * Traslacion ;
                this.Worlds.Add(WorldAux);
                

                ponderacionRestante = ((largoParedRestante-LargoPuerta)/Largo);
                DistanciasPuertas.ForEach(d => d -= (largoParedActual/Largo) );
                corrimiento +=(largoParedActual + LargoPuerta);
                largoParedRestante  -= (largoParedActual + LargoPuerta);
            }

            // dibuja la última
            Escala       = Matrix.CreateScale(Altura,Grosor,largoParedRestante);
            Traslacion   = (EsHorizontal)? 
                            Matrix.CreateTranslation(PuntoInicial.X,-0,PuntoInicial.Z + corrimiento ) :
                            Matrix.CreateTranslation(PuntoInicial.X + corrimiento,-0,PuntoInicial.Z) ;
            
            WorldAux = Escala * LevantarQuad * Rotacion * Traslacion ;
                            
            this.Worlds.Add(WorldAux);
                     
            

        }
        public void Draw(){ 
            Draw(TGCGame.GameContent.T_MarmolNegro);
        }
        public void Draw(Texture2D textura){ 
            
            var body = TGCGame.Simulation.Statics.GetStaticReference(Handle);
            var aabb = body.BoundingBox;

            TGCGame.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Black);

            Efecto.Parameters["Texture"]?.SetValue(textura);

            foreach( var w in Worlds){
                Efecto.Parameters["World"].SetValue(w); 
                TGCGame.GameContent.G_Cubo.Draw(Efecto);
            }

            // Generalizar para tener mas puertas
            // if(ConPuerta){
            //     Efecto.Parameters["World"].SetValue(WorldLateral1); 
            //     TGCGame.GameContent.G_Cubo.Draw(Efecto);
            //     Efecto.Parameters["World"].SetValue(WorldLateral2); 
            //     TGCGame.GameContent.G_Cubo.Draw(Efecto);
            // }else{
            //     Efecto.Parameters["World"].SetValue(WorldCerrada); 
            //     TGCGame.GameContent.G_Cubo.Draw(Efecto);
            // }            
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