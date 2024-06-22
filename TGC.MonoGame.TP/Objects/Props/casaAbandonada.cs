using System;
using System.Collections.Generic;
using System.Linq;
using BepuPhysics.Constraints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThunderingTanks.Collisions;

namespace ThunderingTanks.Objects.Props
{
    public class CasaAbandonada
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";

        public Model CasaModel { get; set; }

        public Vector3 Position { get; set; }

        private Texture2D TexturaCasa { get; set; }
        public Matrix[] CasaWorlds { get; set; }
        public Effect Effect { get; set; }

        public BoundingBox CasaBox { get; set; }
        private Vector3 MaxBox = new Vector3(100f, 1500f, 2000f);
        private Vector3 MinBox = new Vector3(-1900f, 700f, 300f);

        public Matrix CasaWorld { get; set; }

        public CasaAbandonada()
        {
            CasaWorlds = new Matrix[] { };
        }

        public void LoadContent(ContentManager Content)
        {
            CasaModel = Content.Load<Model>(ContentFolder3D + "casa/house");

            TexturaCasa = Content.Load<Texture2D>(ContentFolderTextures + "casaAbandonada/Medieval_Brick_Texture_by_goodtextures");
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            foreach (var mesh in CasaModel.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
            CasaWorld = Matrix.CreateScale(500f) * Matrix.CreateTranslation(Position);

            CasaBox = new BoundingBox(Position + MinBox, Position + MaxBox);
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);
            foreach (var mesh in CasaModel.Meshes)
            {
                Matrix _casaWorld = CasaWorld;
                Effect.Parameters["ModelTexture"].SetValue(TexturaCasa);

                Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _casaWorld);
                mesh.Draw();
            }
        }
    }
}