using Microsoft.Xna.Framework;
using WarSteel.Managers;

namespace WarSteel.Common.Shaders;

class ColorShader : Shader
{
    private Color _color;

    public ColorShader(Color color)
    {
        _color = color;
        Effect = ContentRepoManager.Instance().GetEffect("BasicShader");
    }

    public override void ApplyEffects()
    {
        Effect.Parameters["DiffuseColor"].SetValue(_color.ToVector3());
    }
}
