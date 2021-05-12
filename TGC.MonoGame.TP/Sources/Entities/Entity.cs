using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Entities
{
    internal abstract class Entity
    {
        protected abstract Model Model();
        protected abstract Texture2D[] Textures();
        protected abstract Matrix GeneralWorldMatrix();

        internal void Draw()
        {
            Matrix generalWorldMatrix = GeneralWorldMatrix();

            int index = 0;
            ModelMeshCollection meshes = Model().Meshes;
            foreach (var mesh in meshes)
            {
                Matrix worldMatrix = mesh.ParentBone.Transform * generalWorldMatrix;
                TGCGame.effectManager.BasicShader.Parameters["World"].SetValue(worldMatrix);
                TGCGame.effectManager.BasicShader.Parameters["ModelTexture"].SetValue(Textures()[index]);
                mesh.Draw();
                index++;
            }
        }
    }
}