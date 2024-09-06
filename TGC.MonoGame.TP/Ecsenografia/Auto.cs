using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Escenografia
{
    abstract class Auto : Escenografia3D
    {
        protected float velocidad;
        protected float aceleracion;
        protected float velocidadGiro;
        protected float peso;
        //vector unitario
        protected Vector3 direccion;

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
            //la velocidad siempre se reducira por algun facot, en este caso por 10%
            else 
            {
                velocidad *= 0.9f;
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
        }
    }

    class AutoNPC : Auto
    {
        public AutoNPC(Vector3 posicion)
        {
            this.posicion = posicion;
        }
        public override Matrix getWorldMatrix()
        {
            return Matrix.CreateTranslation(posicion);
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