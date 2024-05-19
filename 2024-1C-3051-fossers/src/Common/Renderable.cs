using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common.Shaders;
using WarSteel.Scenes;

namespace WarSteel.Common;

public class Renderable
{
    protected Model _model { get; }
    protected Dictionary<string, Shader> _shaders;

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

    public virtual void Draw(Matrix world, Scene scene)
    {
        foreach (var mesh in _model.Meshes)
        {
            foreach (var shader in _shaders)
            {
                shader.Value.UseCamera(scene.GetCamera());
                shader.Value.ApplyEffects(scene);

                Matrix modelWorld = mesh.ParentBone.Transform * world;
                shader.Value.UseWorld(modelWorld);
            }

            foreach (Effect effect in mesh.Effects)
            {
                if (effect is BasicEffect basicEffect)
                {
                    basicEffect.World = mesh.ParentBone.Transform * world;
                    basicEffect.View = scene.GetCamera().View;
                    basicEffect.Projection = scene.GetCamera().Projection;
                }
            }

            mesh.Draw();
        };
    }
}
