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

        Model model = ContentRepoManager.Instance().GetModel("Map/Ground");
        float width = ModelUtils.GetWidth(model);
        float height = ModelUtils.GetHeight(model);
        AddComponent(new RigidBody(Transform, new BoxCollider(Transform, new Vector3(-width, -height, -height), new Vector3(width, height, height)), 20, Matrix.Identity, true));

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