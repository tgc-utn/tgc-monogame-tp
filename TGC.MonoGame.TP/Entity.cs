using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    class Entity
    {
        private readonly Model model;
        public Matrix World { get; private set; }
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 scale;

        public Entity(Model model, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.model = model;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
        
        public void Draw(Effect effect)
        {
            foreach (var mesh in model.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(position);
                effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
        }
    }
}