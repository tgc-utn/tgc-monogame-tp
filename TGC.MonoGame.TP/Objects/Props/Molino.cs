using System;
using System.Collections.Generic;
using System.Linq;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThunderingTanks.Objects.Props
{
    public class Molino
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";

        public Model MolinoModel { get; set; }
        public Texture2D MolinoTexture { get; set; }
        public Matrix MolinoMatrix { get; set; }
        public Effect MolinoEffect { get; set; }

        public Molino(Matrix molinoMatrix)
        {
            MolinoMatrix = molinoMatrix;
        }

        public void LoadContent(ContentManager Content)
        {
            MolinoModel = Content.Load<Model>(ContentFolder3D + "ModelosVarios/MolinoProp/MolinoProp");
            MolinoTexture = Content.Load<Texture2D>(ContentFolder3D + "ModelosVarios/MolinoProp/T_Windpump_D");
            MolinoEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            foreach (var mesh in MolinoModel.Meshes)
            {
                if (mesh.Name == "SM_WindPump")
                {
                    foreach (var meshPart in mesh.MeshParts)
                    {
                        meshPart.Effect = MolinoEffect;
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            MolinoEffect.Parameters["View"].SetValue(view);
            MolinoEffect.Parameters["Projection"].SetValue(projection);
            MolinoEffect.Parameters["ModelTexture"].SetValue(MolinoTexture);

            Matrix _molinoMatrix = MolinoMatrix;

            foreach (var mesh in MolinoModel.Meshes)
            {
                if (mesh.Name == "SM_WindPump")
                {
                    MolinoEffect.Parameters["ModelTexture"].SetValue(MolinoTexture);
                    MolinoEffect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _molinoMatrix);

                    mesh.Draw();
                }
            }
        }

    }
}
