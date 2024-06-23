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
    public class GameProp
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";

        public Model Model { get; set; }
        public Texture2D Texture { get; set; }
        public Matrix WorldMatrix { get;  set; }
        public Effect Effect { get; set; }
        public Vector3 Position { get; set; }
        public BoundingBox BoundingBox { get; set; }
        public Vector3 MaxBox;
        public Vector3 MinBox;

        public GameProp()
        {
            WorldMatrix = Matrix.Identity;
        }

        public void Load(Model model, Texture2D texture, Effect effect)
        {
            Model = model;
            Texture = texture;
            Effect = effect;
        }

         public virtual void Draw( Matrix view, Matrix projection)
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

                Effect.Parameters["ModelTexture"].SetValue(Texture);
                Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * WorldMatrix);

                mesh.Draw();

            }
        }

        public void SpawnPosition(Vector3 position)
        {
            WorldMatrix = Matrix.CreateTranslation(position);
            Position = position;
        }
    }
}
