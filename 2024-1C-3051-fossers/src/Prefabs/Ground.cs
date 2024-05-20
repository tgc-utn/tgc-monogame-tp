using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;
using WarSteel.Scenes;
using WarSteel.Utils;


namespace WarSteel.Entities.Map;

public class Ground : Entity
{
    public Ground() : base("ground", Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
        AddComponent(new StaticBody(new Transform(Transform,new Vector3(0,-200,0)), 
        new BoxCollider(
            new List<string>(){"ground"},
            new Dictionary<string, object>(),
            new List<ColliderListener>(),
            200, 9000, 9000)
            ));
    }

    public override void Initialize(Scene scene)
    {
        Transform.Pos = new Vector3(0, -100f, 0);
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/Ground");
        _renderable = new Renderable(model);
        _renderable.AddShader("color", new PhongShader(0.5f, 0.5f, Color.Gray));
        base.LoadContent();
    }
}