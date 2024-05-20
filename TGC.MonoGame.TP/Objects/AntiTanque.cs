using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThunderingTanks.Objects
{
    public class AntiTanque
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public Model AntitanqueModel { get; set; }

        private Texture2D TexturaAntitanque { get; set; }
        public Matrix[] AntitanqueWorlds { get; set; }
        public Effect Effect { get; set; }

        public AntiTanque()
        {
            AntitanqueWorlds = new Matrix[] { };
        }

        public void AgregarAntitanque(Vector3 Position)
        {
            Matrix posicionAntitanque = Matrix.CreateTranslation(Position) * Matrix.Identity;
            var nuevoAntitanque = new Matrix[]{
                posicionAntitanque * Matrix.CreateScale(500),
            };
            AntitanqueWorlds = AntitanqueWorlds.Concat(nuevoAntitanque).ToArray();
        }

        public void LoadContent(ContentManager Content)
        {
            AntitanqueModel = Content.Load<Model>(ContentFolder3D + "assets militares/rsg_military_antitank_hedgehog_01");

            TexturaAntitanque = Content.Load<Texture2D>(ContentFolder3D + "assets militares/Textures/UE/T_rsg_military_sandbox_01_BC");
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            foreach (var mesh in AntitanqueModel.Meshes)
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
            Effect.Parameters["ModelTexture"].SetValue(TexturaAntitanque);

            //Effect.Parameters["DiffuseColor"].SetValue(Color.Black.ToVector3());
            foreach (var mesh in AntitanqueModel.Meshes)
            {

                for (int i = 0; i < AntitanqueWorlds.Length; i++)
                {
                    Matrix _antitanqueWorld = AntitanqueWorlds[i];
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _antitanqueWorld);
                    mesh.Draw();
                }

            }
        }
    }
}
