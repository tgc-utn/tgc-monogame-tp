using Microsoft.Xna.Framework.Graphics;
using WarSteel.Managers;

namespace WarSteel.Common.Shaders;

class TextureShader : Shader
{

    private Texture2D _texture;


    public TextureShader(Texture2D texture)
    {
        this._texture = texture;
        this.Effect = ContentRepoManager.Instance().GetEffect("BasicTextureShader");
    }

    public override void ApplyEffects()
    {
        this.Effect.Parameters["Texture"].SetValue(_texture);
    }

}