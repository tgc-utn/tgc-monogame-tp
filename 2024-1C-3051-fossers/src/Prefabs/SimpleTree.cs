using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Managers;

namespace WarSteel.Entities;


class SimpleTree : Entity
{
    class TreeCollider : Collider
    {
        public TreeCollider() : base(new BoxCollider(200, 200, 200)) { }
    }

    public SimpleTree() : base("simple-tree", Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
        AddComponent(new StaticBody(new Transform(Transform, new Vector3(0, -200, 0)), new TreeCollider()));
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/SimpleTree");
        _renderable = new Renderable(model);

        base.LoadContent();
    }
}