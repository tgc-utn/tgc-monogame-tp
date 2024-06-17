using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThunderingTanks.Objects.Props
{
    internal class Grass : GameProp
    {
        private Texture2D GrassApha {  get; set; }
        private Texture2D GrassColor { get; set; }
        private Texture2D GrassNormal { get; set; }
        private Texture2D GrassSmoothness { get; set; }

        public void Load(Model model, Texture2D grassApha, Texture2D grassColor, Texture2D grassNormal, Texture2D grassSmoothness, Effect effect)
        {
            Model = model;
            GrassApha = grassApha;
            GrassColor = grassColor;
            GrassNormal = grassNormal;
            GrassSmoothness = grassSmoothness;
            Effect = effect;
        }

        public void Draw(List<Vector3> positions, Matrix view, Matrix projection)
        {
            foreach (var position in positions)
            {

                foreach (var mesh in Model.Meshes)
                {

                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = Effect;
                    }

                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.Parameters["View"].SetValue(view);
                        effect.Parameters["Projection"].SetValue(projection);
                    }

                    Effect.Parameters["ModelTexture"].SetValue(GrassApha);
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * Matrix.CreateTranslation(position));

                    mesh.Draw();
                }
            }
        }
    }
}
