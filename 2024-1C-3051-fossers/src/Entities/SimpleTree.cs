using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;
using WarSteel.Scenes;
using WarSteel.Utils;

namespace WarSteel.Entities;


class SimpleTree : Entity
{
    public SimpleTree() : base("simple-tree", Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
    }

    public override void Initialize(Scene scene)
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/SimpleTree");
        AddComponent(new StaticBody(new Collider(new BoxShape(1000, 200, 200),new NoAction()),new Vector3(0,500,0)));
        base.Initialize(scene);
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/SimpleTree");
        Renderable = new Renderable(model);
        Renderable.AddShader("color",new PhongShader(0.5f,0.5f,Color.Brown));

        base.LoadContent();
    }
}