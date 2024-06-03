using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Scenes;

namespace WarSteel.Common.Shaders;

public abstract class Shader
{
    public Effect Effect { get; set; }

    public void UseCamera(Camera camera)
    {
        Effect.Parameters["View"].SetValue(camera.View);
        Effect.Parameters["Projection"].SetValue(camera.Projection);
    }

    public void UseWorld(Matrix world)
    {
        Effect.Parameters["World"].SetValue(world);
    }

    public abstract void ApplyEffects(Transform transform,Scene scene);
}