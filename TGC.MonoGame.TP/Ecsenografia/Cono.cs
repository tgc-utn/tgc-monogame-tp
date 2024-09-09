using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Escenografia
{
    public class Cono : Escenografia3D
    {
        public override Matrix getWorldMatrix()
        {
           return world;
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