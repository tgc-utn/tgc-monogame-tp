using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.Common.Shaders;

class TextureShader : Shader
{

    private Texture2D _texture;


    public TextureShader(Texture2D texture)
    {
        this._texture = texture;
        this.Effect = ContentRepoManager.Instance().GetEffect("BasicTextureShader");
    }

    public override void ApplyEffects(Scene scene, Matrix world)
    {
        Effect.Parameters["Texture"].SetValue(_texture);
        Effect.Parameters["World"].SetValue(world);
        Effect.Parameters["View"].SetValue(scene.GetCamera().View);
        Effect.Parameters["Projection"].SetValue(scene.GetCamera().Projection);
    }

}