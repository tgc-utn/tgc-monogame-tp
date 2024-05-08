using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;

namespace WarSteel.Entities;


class SimpleTree : Entity
{
    public SimpleTree() : base("simple-tree", Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/SimpleTree");
        _renderable = new Renderable(model);
        _renderable.AddShader("color", new ColorShader(Color.Black));

        base.LoadContent();
    }
}