using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThunderingTanks.Collisions;

namespace ThunderingTanks.Objects.Props
{
    public class Shack : GameObject
    {
        private Texture2D Texture2 { get; set; }

        public BoundingBox ShackBox;
        public override void LoadContent(ContentManager Content, Effect effect)
        {
            Model = Content.Load<Model>(ContentFolder3D + "Snowy_Shack/Little_shack");
            Texture = Content.Load<Texture2D>(ContentFolder3D + "Snowy_Shack/Little_shack_DefaultMaterial_BaseColor");
            Texture2 = Content.Load<Texture2D>(ContentFolder3D + "Snowy_Shack/Little_shack_DefaultMaterial_Metallic");
            Effect = effect;

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }

            ShackBox = CollisionsClass.CreateBoundingBox(Model, Matrix.CreateScale(2.5f), Position);
        }
    }
}
