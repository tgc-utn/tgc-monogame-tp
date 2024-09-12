using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Escenografia;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using TGC.MonoGame.TP;
using Microsoft.Xna.Framework.Content;

namespace Escenografia
{
    public class Palmera : Escenografia3D
    {
        float scale;

        public Palmera(GraphicsDevice graphicsDevice, Vector3 posicion)
        {
            this.posicion = posicion;
        }
        public override Matrix getWorldMatrix()
        {
            return Matrix.CreateScale(scale) * Matrix.CreateTranslation(posicion);
        }
        public void SetScale(float scale)
        {
            this.scale = scale;
        }
        public void SetPosition(Vector3 unaPosicion)
        {
            posicion = unaPosicion;
        }

        public override void loadModel(string direcionModelo, string direccionEfecto, ContentManager contManager)
        {
            base.loadModel(direcionModelo, direccionEfecto, contManager);
            foreach (ModelMesh mesh in modelo.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = efecto;
                }
            }
        }
    }
}