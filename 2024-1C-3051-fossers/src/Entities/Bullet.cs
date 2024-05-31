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

        public DeleteOnImpact(Entity self){
            _self = self;
        }

        public void ExecuteAction(Collision col)
        {
            if (col.Entity?.Name != "player-bullet" && col.Entity?.Name != "player")
            {
              _self.Destroy();
            }

        }

    }

    public Bullet(string name, float damage, Vector3 Pos, Vector3 direction, float force) : base(name, Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
        _direction = direction;
        _force = force;
        Damage = damage;
        Transform.Position = Pos;
        AddComponent(new DynamicBody(Transform, new Collider(new BoxShape(50,50,50),new DeleteOnImpact(this)), 5));
    }

    public override void Initialize(Scene scene)
    {
        GetComponent<DynamicBody>().ApplyForce(_direction * _force);
        base.Initialize(scene);
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Tanks/Bullet");
        _renderable = new Renderable(model);
        _renderable.AddShader("color", new ColorShader(Color.Red));

        base.LoadContent();
    }

    public override void Update(GameTime gameTime, Scene scene)
    {
        base.Update(gameTime, scene);
    }
}