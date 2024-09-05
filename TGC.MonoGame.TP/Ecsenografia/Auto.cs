using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Escenografia
{
    abstract class Auto : Escenografia3D
    {
        protected float velocidad;
        protected float celeracion;
        protected float peso;

        //esto lo implementan los hijos de la clase
        abstract public void mover(float deltaTime);
    }
    class AutoJugador : Auto
    {
        public AutoJugador(Vector3 posicion, float velocidad)
        {
            this.posicion = posicion;
            this.velocidad = velocidad;
        }
        public override Matrix getWorldMatrix()
        {
            return Matrix.CreateFromYawPitchRoll(rotacionX, rotacionY, rotacionZ) * Matrix.CreateTranslation(posicion);
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
            if ( Keyboard.GetState().IsKeyDown(Keys.A))
            {
                posicion += Vector3.Left * velocidad * deltaTime;
            }
            if ( Keyboard.GetState().IsKeyDown(Keys.S))
            {
                posicion += Vector3.Backward * velocidad * deltaTime;
            }
            if ( Keyboard.GetState().IsKeyDown(Keys.D))
            {
                posicion += Vector3.Right * velocidad * deltaTime;
            }
            if ( Keyboard.GetState().IsKeyDown(Keys.W))
            {
                posicion += Vector3.Forward * velocidad * deltaTime;
            }
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