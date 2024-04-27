using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common.Shaders;

namespace WarSteel.Common;

public class Renderable
{
    private Model _model { get; }
    private Dictionary<string, Shader> _shaders;

    public Renderable(Model model)
    {
        _model = model;
        _shaders = new Dictionary<string, Shader>();
    }

    public void AddShader(string name, Shader shader)
    {
        _shaders[name] = shader;
        shader.AssociateShaderTo(_model);
    }

    public void Draw(Matrix world, Camera camera)
    {
        foreach (var mesh in _model.Meshes)
        {
            foreach (var shader in _shaders)
            {
                shader.Value.UseCamera(camera);
                shader.Value.ApplyEffects();

                Matrix modelWorld = mesh.ParentBone.Transform * world;
                shader.Value.UseWorld(modelWorld);
            }

            mesh.Draw();
        };
    }
}
