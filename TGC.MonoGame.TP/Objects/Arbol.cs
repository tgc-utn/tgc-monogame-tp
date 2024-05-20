using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThunderingTanks.Objects
{
    public class Arbol
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public Model ArbolModel { get; set; }
        public Matrix[] ArbolWorlds { get; set; }
        public Effect Effect { get; set; }

        public Arbol()
        {
            ArbolWorlds = new Matrix[] { };
        }

        public void AgregarArbol(Vector3 Position)
        {
            Matrix escala = Matrix.CreateScale(2.5f);
            var nuevaRoca = new Matrix[]{
                escala * Matrix.CreateTranslation(Position),
            };
            ArbolWorlds = ArbolWorlds.Concat(nuevaRoca).ToArray();
        }

        public void LoadContent(ContentManager Content)
        {
            ArbolModel = Content.Load<Model>(ContentFolder3D + "nature/tree/Southern Magnolia-CORONA");
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            foreach (var mesh in ArbolModel.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            Effect.Parameters["View"].SetValue(view); //Cambio View por Eso
            Effect.Parameters["Projection"].SetValue(projection);
            Effect.Parameters["DiffuseColor"].SetValue(Color.Brown.ToVector3());
            foreach (var mesh in ArbolModel.Meshes)
            {

                for (int i = 0; i < ArbolWorlds.Length; i++)
                {
                    Matrix _arbolWorld = ArbolWorlds[i];
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _arbolWorld);
                    mesh.Draw();
                }

            }
        }
    }
}
