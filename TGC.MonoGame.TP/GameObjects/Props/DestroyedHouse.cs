using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThunderingTanks.Objects.Props
{
    public class DestroyedHouse : GameObject
    {

        public Matrix[] CasaWorlds { get; set; }

        public DestroyedHouse()
        {
            CasaWorlds = System.Array.Empty<Matrix>();

            MaxBox = new Vector3(100f, 1500f, 2000f);
            MinBox = new Vector3(-1900f, 700f, 300f);
        }

        public override void LoadContent(ContentManager Content, Effect effect)
        {
            Model = Content.Load<Model>(ContentFolder3D + "casa/house");

            Texture = Content.Load<Texture2D>(ContentFolderTextures + "casaAbandonada/Medieval_Brick_Texture_by_goodtextures");

            Effect = effect;

            foreach (var mesh in Model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }

            WorldMatrix = Matrix.CreateScale(500f) * Matrix.CreateTranslation(Position);

            BoundingBox = new BoundingBox(Position + MinBox, Position + MaxBox);
        }
    }
}