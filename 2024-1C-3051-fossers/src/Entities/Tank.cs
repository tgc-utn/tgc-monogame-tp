using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;
using WarSteel.Utils;

namespace WarSteel.Entities;

public class Tank : Entity
{
    public Tank(string name) : base(name, Array.Empty<string>(), new Transform(),new Component[]{new RigidBody(
        Vector3.Zero,Vector3.Zero,1,Matrix.Identity
    )})
    {

    }

    public override void LoadContent(Camera camera)
    {
        Model model = ContentRepoManager.Instance().GetModel("Tanks/Panzer/Panzer");
        Shader texture = new TextureShader(ContentRepoManager.Instance().GetTexture("Tanks/T90/textures_mod/hullA"));
        _renderable = new Renderable(model);
        _renderable.AddShader("texture", texture);

        base.LoadContent(camera);
    }
}