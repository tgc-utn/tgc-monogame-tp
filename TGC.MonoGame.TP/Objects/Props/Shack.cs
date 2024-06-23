using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThunderingTanks.Objects.Props
{
    public class WaterTank : GameProp
    { 
        private Texture2D Texture2 {  get; set; }

        public void LoadContent(ContentManager Content, Effect effect)
        {
            Model = Content.Load<Model>(ContentFolder3D + "Snowy_Shack/Little_shack");
            Texture = Content.Load<Texture2D>(ContentFolder3D + "Snowy_Shack/Little_shack_DefaultMaterial_BaseColor");
            Texture2 = Content.Load<Texture2D>(ContentFolder3D + "Snowy_Shack/Little_shack_DefaultMaterial_Metallic");
            Effect = effect;

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach(ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
        }
    }
}
