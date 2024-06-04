using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Scenes;

namespace WarSteel.Common.Shaders;

public abstract class Shader
{
    public Effect Effect { get; set; }

    public void AssociateTo(Model model)
    {
        foreach (var modelMesh in model.Meshes)
            foreach (var meshPart in modelMesh.MeshParts)
                meshPart.Effect = Effect;
    }
    public abstract void ApplyEffects(Scene scene, Matrix world);
}
