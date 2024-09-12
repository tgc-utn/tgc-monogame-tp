using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Data;

namespace Escenografia
{
    abstract class Auto : Escenografia3D
    {
        //ralacionadas con movimiento
        protected float velocidad;
        protected float aceleracion;
        protected float velocidadGiro;
        //protected float peso;
        protected Vector3 direccion;

        //para limitar el movimiento de objetos
        //esto es una constante
        static protected Vector3 esquinaInferiorEsc = new Vector3(1f,0f,1f) * -10000f;
        static protected Vector3 esquinaSuperiorEsc = new Vector3(1f, 0f, 1f) * 10000f;
        //vector unitario

        //esto lo implementan los hijos de la clase
        abstract public void mover(float deltaTime);
    }
    class AutoJugador : Auto
    {
        public AutoJugador(Vector3 posicion, Vector3 direccion, float aceleracion, float velocidadGiro)
        {
            this.direccion = direccion;
            this.posicion = posicion;
            this.aceleracion = aceleracion;
            this.velocidadGiro = velocidadGiro;
        }
        public override Matrix getWorldMatrix()
        {
            return Matrix.CreateFromYawPitchRoll(rotacionY, rotacionX, rotacionZ) * Matrix.CreateTranslation(posicion);
        }

        public override void loadModel(string direcionModelo, string direccionEfecto, ContentManager contManager)
        {
            base.loadModel(direcionModelo, direccionEfecto, contManager);
            foreach ( ModelMesh mesh in modelo.Meshes )
            {
                foreach ( ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = efecto;
                }
            }
        }
        public override void mover(float deltaTime)
        {
            if ( Keyboard.GetState().IsKeyDown(Keys.S))
            {
                velocidad -= aceleracion * deltaTime;
            }
            else if ( Keyboard.GetState().IsKeyDown(Keys.W))
            {
                velocidad += aceleracion * deltaTime;
            }
            //la velocidad siempre se reducira por algun facot, en este caso por 4%
            else 
            {
                velocidad *= 0.96f;
            }
            //los elvis operators/ ifinlines / ternaris. Estan solo para que el auto se mueva como un auto de verdad
            if ( Keyboard.GetState().IsKeyDown(Keys.A))
            {
                rotacionY += (velocidad >= 0 ? velocidadGiro : -velocidadGiro) * deltaTime;
            }
            if ( Keyboard.GetState().IsKeyDown(Keys.D))
            {
                rotacionY += (velocidad >= 0 ? -velocidadGiro : velocidadGiro) * deltaTime;
            }
            
            
            posicion += Vector3.Transform(direccion, Matrix.CreateFromYawPitchRoll(
                rotacionY, rotacionX, rotacionZ) ) * velocidad * deltaTime;
            posicion = Utils.Matematicas.clampV(posicion, esquinaInferiorEsc, esquinaSuperiorEsc);
        }
    }

    class AutoNPC : Auto
    {
        public Color color;
        public AutoNPC(Vector3 posicion)
        {
            this.posicion = posicion; 
        }
        public AutoNPC(Vector3 posicion, float rotacionX, float rotacionY, float rotacionZ)
        {
            this.posicion = posicion;
            this.rotacionX = rotacionX;
            this.rotacionY = rotacionY;
            this.rotacionZ = rotacionZ;
        }
        public AutoNPC(Vector3 posicion, float rotacionX, float rotacionY, float rotacionZ, Color color)
        {
            this.posicion = posicion;
            this.rotacionX = rotacionX;
            this.rotacionY = rotacionY;
            this.rotacionZ = rotacionZ;
            this.color = color;
        }
        public override Matrix getWorldMatrix()
        {
            return Matrix.CreateFromYawPitchRoll(rotacionY,rotacionX,rotacionZ) * Matrix.CreateTranslation(this.posicion);
        }

        public override void mover(float deltaTime)
        {
            throw new System.NotImplementedException();
        }
        public override void loadModel(string direcionModelo, string direccionEfecto, ContentManager contManager)
        {
            base.loadModel(direcionModelo, direccionEfecto, contManager);
            foreach ( ModelMesh mesh in modelo.Meshes )
            {
                foreach ( ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = efecto;
                }
            }
        }
    }
}