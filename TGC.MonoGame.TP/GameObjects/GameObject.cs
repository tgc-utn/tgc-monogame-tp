using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThunderingTanks.Cameras;
using ThunderingTanks.Collisions;
using ThunderingTanks.Geometries;
using ThunderingTanks.Gizmos;

namespace ThunderingTanks.Objects
{
    public class GameObject
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";
        public const string ContentFolderMusic = "Music/";

        public Model Model { get; set; }
        public Texture2D Texture { get; set; }

        public Effect Effect { get; set; }

        public Matrix WorldMatrix { get; set; }

        public Vector3 Position { get; set; }
        public Vector3 LastPosition { get; set; }

        public BoundingBox BoundingBox { get; set; }
        public Vector3 MaxBox;
        public Vector3 MinBox;

        public GameObject()
        {
            WorldMatrix = Matrix.Identity;
        }

        public virtual void Initialize() { }    

        public virtual void Update(GameTime gameTime) { }

        public virtual void LoadContent(ContentManager Content, Effect effect)
        {
            Effect = effect;
        }

        public virtual void Draw(Matrix view, Matrix projection)
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
