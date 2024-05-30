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
        public BoundingBox BBox { get; set; }

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
                    var texture = ((BasicEffect)meshPart.Effect).Texture;
                    textures.Add(texture);
                    meshPart.Effect = Effect;
                }
                MeshPartTextures.Add(textures);
            }

            var modelBoundingBox = BoundingVolumesExtensions.CreateAABBFrom(Model);
            BBox = new BoundingBox(modelBoundingBox.Min + Position, modelBoundingBox.Max + Position);
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


                var modelBoundingBox = BoundingVolumesExtensions.CreateAABBFrom(Model);
                BBox = new BoundingBox(modelBoundingBox.Min + Position, modelBoundingBox.Max + Position);

            }

            foreach (var mesh in Model.Meshes)
            {

                var textures = new List<Texture2D>();
                foreach (var meshPart in mesh.MeshParts)
                {
                    if ((meshPart.Effect is BasicEffect) == false)
                        continue;

                    var texture = ((BasicEffect)meshPart.Effect)?.Texture;
                    textures.Add(texture);
                    meshPart.Effect = Effect;
                }
                MeshPartTextures.Add(textures);
            }

        }
    }
}
