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
        RigidBody r = new RigidBody(new Transform(Transform, Vector3.Down * 200), float.MaxValue, Matrix.Identity,new BoxCollider(new Transform(), new System.Numerics.Vector3(10000,200,10000)), true);
        AddComponent(r); 
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