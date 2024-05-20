using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.Entities;

public class Bullet : Entity
{
    public float Damage;
    private Vector3 _direction;
    private float _force;
    // future attr when implementing bullets explosions
    //// public float ExplosionArea;

    public Bullet(string name, float damage, Vector3 Pos, Vector3 direction, float force) : base(name, Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
        DynamicBody r = new DynamicBody(Transform, new BoxCollider(new List<string>() { "bullet" }, new Dictionary<string, object>(), new List<ColliderListener>(), 1, 1, 1), 10);
        AddComponent(r);
        r.ApplyForce(direction * force);

        _direction = direction;
        _force = force;
        Damage = damage;
        Transform.Pos = Pos;
    }

    public override void Initialize(Scene scene)
    {
        LoadContent();
        base.Initialize(scene);
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Tanks/Bullet");
        _renderable = new Renderable(model);

        base.LoadContent();
    }

    public override void Update(GameTime gameTime, Scene scene)
    {
        base.Update(gameTime, scene);
    }
}