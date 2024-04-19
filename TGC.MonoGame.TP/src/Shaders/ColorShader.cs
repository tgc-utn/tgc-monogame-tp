using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{

    class ColorShader : Shader
    {

        private Color _color;

        public ColorShader(Color color){
            this._color = color;
            this.Effect = ContentRepository.GetInstance().GetEffect("BasicShader");
        }

        public override void ApplyEffects()
        {
            Effect.Parameters["DiffuseColor"].SetValue(_color.ToVector3());
        }
    }
}
