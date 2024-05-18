using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Scenes;

namespace WarSteel.Common.Shaders;

public abstract class Shader
{
    public Effect Effect { get; set; }

    public void AssociateShaderTo(Model model)
    {
        foreach (var mesh in model.Meshes)
        {
            foreach (var meshPart in mesh.MeshParts)
            {
                meshPart.Effect = Effect;
            }
        }
    }

    public void UseCamera(Camera camera)
    {
        Effect.Parameters["View"].SetValue(camera.View);
        Effect.Parameters["Projection"].SetValue(camera.Projection);
    }

    public void UseWorld(Matrix world)
    {
        Effect.Parameters["World"].SetValue(world);
    }

    public abstract void ApplyEffects(Scene scene);
}
