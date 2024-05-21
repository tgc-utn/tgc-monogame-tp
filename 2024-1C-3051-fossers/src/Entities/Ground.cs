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
    class GroundCollider : Collider
    {
        public GroundCollider() : base(new BoxCollider(200, 9000, 9000)) { }
    }

    public Ground() : base("ground", Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
        AddComponent(new StaticBody(new Transform(Transform, new Vector3(0, -200, 0)), new GroundCollider()));
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