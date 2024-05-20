using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThunderingTanks.Objects
{
   public class Roca
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public Model RocaModel { get; set; }
        public Matrix[] RocaWorlds { get; set; }
        public Effect Effect { get; set; }

        public Roca()
        {
            RocaWorlds = new Matrix[] { };
        }

        public void AgregarRoca(Vector3 Position)
        {
            Matrix escala = Matrix.CreateScale(2.5f);
            var nuevaRoca = new Matrix[]{
                escala * Matrix.CreateTranslation(Position),
            };
            RocaWorlds = RocaWorlds.Concat(nuevaRoca).ToArray();
        }

        public void LoadContent(ContentManager Content)
        {
            RocaModel = Content.Load<Model>(ContentFolder3D + "nature/rock/Rock_1");
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            foreach (var mesh in RocaModel.Meshes)
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
            Effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
            foreach (var mesh in RocaModel.Meshes)
            {

                for (int i = 0; i < RocaWorlds.Length; i++)
                {
                    Matrix _cartelWorld = RocaWorlds[i];
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _cartelWorld);
                    mesh.Draw();
                }

            }
        }
    }
}
