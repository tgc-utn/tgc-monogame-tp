using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{

    abstract class Shader
    {

        private Effect _effect;
        public void AssociateShaderTo(Model model)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = _effect;
                }
            }
        }

        public void UseCamera(Camera camera){
            _effect.Parameters["View"].SetValue(camera.View);
            _effect.Parameters["Projection"].SetValue(camera.Projection);
        }

        public void UseWorld(Matrix world){
            _effect.Parameters["World"].SetValue(world);
        }

        public abstract void ApplyEffects();

        public Effect Effect {
            get {return _effect;}
            set {_effect = value;}}

    }

}