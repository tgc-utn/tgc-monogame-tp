using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;

namespace WarSteel.Entities;


class Bush : Entity
{
    public Bush() : base("bush", Array.Empty<string>(), new Transform())
    {
    }

    public override void LoadContent(Camera camera)
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/Bush");
        _renderable = new Renderable(model);
        _renderable.AddShader("color", new ColorShader(Color.Green));

        base.LoadContent(camera);
    }
}