using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;
using WarSteel.Scenes;
namespace WarSteel.Entities.Map;


public class Ground : Entity
{


    public Ground() : base("ground", Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
        AddComponent(new StaticBody(new Transform(Transform, new Vector3(0, -100, 0)), new Collider(new BoxShape(100, 100000000000, 100000000000), new NoAction())));
    }

    public override void Initialize(Scene scene)
    {
        Transform.Position = new Vector3(0, -100f, 0);
        base.Initialize(scene);
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/Ground");
        _renderable = new Renderable(model);
        _renderable.AddShader("color", new PhongShader(0.5f, 0.5f, Color.Gray));
        base.LoadContent();
    }
}