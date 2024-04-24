using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common.Shaders;

namespace WarSteel.Common;

public class Renderable
{
    private Model _model { get; }
    private Shader _shader { get; }

    public Renderable(Model model)
    {
        _model = model;
    }

    public Renderable(Model model, Shader shader)
    {
        _shader = shader;
        _model = model;
        _shader.AssociateShaderTo(model);
    }

    public void Draw(Matrix world, Camera camera)
    {

        _shader.UseCamera(camera);
        _shader.ApplyEffects();

        foreach (var mesh in _model.Meshes)
        {
            Matrix modelWorld = mesh.ParentBone.Transform * world;
            _shader.UseWorld(modelWorld);
            mesh.Draw();
        }
    }
}
