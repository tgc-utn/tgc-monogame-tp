using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.Entities;


class SimpleTree : Entity
{
    class TreeCollider : Collider
    {
        public TreeCollider() : base(new BoxCollider(50, 1000, 50)) { }
    }

    public SimpleTree() : base("simple-tree", Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
    }

    public override void Initialize(Scene scene)
    {
        AddComponent(new StaticBody(Transform, new TreeCollider()));
        base.Initialize(scene);
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/SimpleTree");
        _renderable = new Renderable(model);

        base.LoadContent();
    }
}