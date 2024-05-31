using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using BepuPhysics.Collidables;
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
    }

    public virtual void Draw(Transform transform, Scene scene)
    {
        foreach (var mesh in _model.Meshes)
        {
            foreach (var shader in _shaders)
            {
                shader.Value.UseCamera(scene.GetCamera());
                shader.Value.ApplyEffects(scene);

                Matrix modelWorld = GetMatrix(mesh, transform);
                shader.Value.UseWorld(modelWorld);

                foreach (var part in mesh.MeshParts)
                {
                    part.Effect = shader.Value.Effect;
                }

                mesh.Draw();
            }
        };
    }

    public virtual Matrix GetMatrix(ModelMesh mesh, Transform transform){
        return transform.LocalToWorldMatrix(mesh.ParentBone.Transform);
    }


}
