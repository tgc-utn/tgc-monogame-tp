using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.Entities;

public class Tank : Entity
{
    public Tank(string name) : base(name, Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {

        // AddComponent(new DynamicBody(Transform, new BoxCollider(new List<string>(){"tank"},new Dictionary<string, object>(){}, new List<ColliderListener>(){new Logger()},200, 200, 200), 5));
        // AddComponent(new PlayerControls());

    }

    public override void Initialize(Scene scene)
    {
        base.Initialize(scene);
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Tanks/Panzer/Panzer");

        Shader texture = new PhongShader(0.2f, 0.5f, Color.Gray);
        _renderable = new Renderable(model);
        _renderable.AddShader("phong", texture);
        base.LoadContent();
    }

    public override void Update(GameTime gameTime, Scene scene)
    {
        base.Update(gameTime, scene);
    }
}

