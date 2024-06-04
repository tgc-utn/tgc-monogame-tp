using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.Samples.Collisions;

namespace TGC.MonoGame.TP
{
    public class BaseModel
    {
        public Model Model { get; private set; }
        public Effect Effect { get; private set; }

        public float Scale = 1f;
        public Vector3 Position { get; set; }

        public List<Matrix> World = new List<Matrix>();

        public List<BoundingBox> BBox = new List<BoundingBox>();

        public List<List<Texture2D>> MeshPartTextures = new List<List<Texture2D>>();

        protected BaseModel(Model model, Effect effect, float scale, Vector3 pos)
        {
            Initialize(model, effect, scale, pos);
        }

        protected BaseModel(Model model, Effect effect, float scale, List<Vector3> listPos)
        {
            Initialize(model, effect, scale, listPos);
        }

        private void Initialize(Model model, Effect effect, float scale, Vector3 pos)
        {
            Model = model;
            Effect = effect;
            Scale = scale;
            Position = pos;
            World.Add(Matrix.CreateTranslation(Position));

            foreach (var mesh in Model.Meshes)
            {
                var textures = new List<Texture2D>();

                foreach (var meshPart in mesh.MeshParts)
                {
                    var texture = ((BasicEffect)meshPart.Effect)?.Texture;
                    textures.Add(texture);
                    meshPart.Effect = Effect;
                }
                MeshPartTextures.Add(textures);
            }

            BBox.Add(BoundingVolumesExtensions.FromMatrix(Matrix.CreateTranslation(pos)));

        }

        private void Initialize(Model model, Effect effect, float scale, List<Vector3> listPos)
        {
            foreach (var pos in listPos)
            {
                Model = model;
                Effect = effect;
                Scale = scale;
                Position = pos;
                World.Add(Matrix.CreateTranslation(Position));

                BBox.Add(BoundingVolumesExtensions.FromMatrix(Matrix.CreateTranslation(pos)));


            }

            foreach (var mesh in Model.Meshes)
            {
                var textures = new List<Texture2D>();
                foreach (var meshPart in mesh.MeshParts)
                {
                    var texture = ((BasicEffect)meshPart.Effect)?.Texture;
                    textures.Add(texture);
                    meshPart.Effect = Effect;
                }
                MeshPartTextures.Add(textures);
            }

        }
    }
}
