using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    abstract class Entity
    {
        protected abstract Model Model();
        protected abstract Texture2D[] Textures();
        public Matrix World { get; private set; }
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 scale;

        public Entity(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
        
        public void Draw(Effect effect)
        {
            int index = 0;
            ModelMeshCollection meshes = Model().Meshes;
            foreach (var mesh in meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(position);
                effect.Parameters["World"].SetValue(World);
                effect.Parameters["ModelTexture"].SetValue(Textures()[index]);
                mesh.Draw();
                index++;
            }
        }
    }
}