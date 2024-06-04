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

    class DeleteOnImpact : CollisionAction
    {

        private Entity _self;

        public DeleteOnImpact(Entity self)
        {
            _self = self;
        }

        public void ExecuteAction(Collision col)
        {

        }

    }

    public Bullet(string name, float damage, Vector3 Pos, Vector3 direction, float force) : base(name, Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
        _direction = direction;
        _force = force;
        Damage = damage;
        Transform.Position = Pos;

    }

    public override void Initialize(Scene scene)
    {
        AddComponent(new DynamicBody(new Collider(new SphereShape(10), new DeleteOnImpact(this)),Vector3.Zero, 5, 0, 0));
        GetComponent<DynamicBody>().ApplyForce(_direction * _force);
        base.Initialize(scene);
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Tanks/Bullet");
        Renderable = new Renderable(model,new PhongShader(0.2f,0.9f,Color.Red));


        base.LoadContent();
    }

    public override void Update(GameTime gameTime, Scene scene)
    {
        base.Update(gameTime, scene);
    }
}