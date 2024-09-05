
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Escenografia.TESTS
{
    class Auto : Escenografia3D
    {
        public Auto(Vector3 posicion)
        {
            this.posicion = posicion;
        }
        public override Matrix getWorldMatrix()
        {
            return Matrix.CreateWorld(posicion, Vector3.Forward, Vector3.Up);
        }
        public override void loadModel(String direccionModelo, String direccionEfecto, ContentManager contentManager)
        {
            base.loadModel(direccionModelo, direccionEfecto, contentManager);
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