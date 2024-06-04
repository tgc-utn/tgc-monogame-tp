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
        AddComponent(new StaticBody(new Collider(new BoxShape(100, 100000, 100000), new NoAction()),Vector3.Up * 50));
    }

    public override void Initialize(Scene scene)
    {
        base.Initialize(scene);
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/Ground");
        Renderable = new Renderable(model,new PhongShader(0.5f, 0.5f, Color.Gray));
        base.LoadContent();
    }
}