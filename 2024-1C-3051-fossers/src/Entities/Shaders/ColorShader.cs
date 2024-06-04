using Microsoft.Xna.Framework;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.Common.Shaders;

class ColorShader : Shader
{
    private Color _color;

    public ColorShader(Color color)
    {
        _color = color;
        Effect = ContentRepoManager.Instance().GetEffect("BasicShader");
    }
    public override void ApplyEffects(Scene scene, Matrix world)
    {
        Effect.Parameters["World"].SetValue(world);
        Effect.Parameters["View"].SetValue(scene.GetCamera().View);
        Effect.Parameters["Projection"].SetValue(scene.GetCamera().Projection);
        Effect.Parameters["DiffuseColor"].SetValue(_color.ToVector3());
    }
}
