using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;

namespace WarSteel.Entities;


class Bush : Entity
{
    public Bush() : base("bush", Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/Bush");
        Renderable = new Renderable(model);

        base.LoadContent();
    }
}