using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThunderingTanks.Objects
{
    public class CasaAbandonada
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public Model CasaModel { get; set; }
        public Matrix[] CasaWorlds { get; set; }
        public Effect Effect { get; set; }

        public CasaAbandonada()
        {
            CasaWorlds = new Matrix[] { };
        }

        public void AgregarCasa(Vector3 Position)
        {
            Matrix escala = Matrix.CreateScale(500f);
            var nuevaCasa = new Matrix[]{
                escala * Matrix.CreateTranslation(Position),
            };
            CasaWorlds = CasaWorlds.Concat(nuevaCasa).ToArray();
        }

        public void LoadContent(ContentManager Content)
        {
            CasaModel = Content.Load<Model>(ContentFolder3D + "casa/house");
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            foreach (var mesh in CasaModel.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);
            Effect.Parameters["DiffuseColor"].SetValue(Color.Azure.ToVector3());
            foreach (var mesh in CasaModel.Meshes)
            {

                for (int i = 0; i < CasaWorlds.Length; i++)
                {
                    Matrix _casaWorld = CasaWorlds[i];
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _casaWorld);
                    mesh.Draw();
                }

            }
        }
    }
}
