using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;
using WarSteel.Scenes;
using WarSteel.Utils;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace WarSteel.Entities;

public class Tank : Entity
{
    float acc = 0;
    RigidBody rb;
    public Tank(string name) : base(name, Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
        rb = new RigidBody(Transform, 20, Matrix.Identity, new List<Func<RigidBody, OVector3>>() { }, new Collider(Transform, new Vector3(200, 200, 200)));
        AddComponent(rb);
        AddComponent(new LightComponent(Color.White, new Vector3(2000, 0, 0)));
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
        Movement(gameTime);
        base.Update(gameTime, scene);
    }

    public void Movement(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            rb.AddForce(new OVector3(Vector3.Zero, Transform.GetWorld().Forward * 20000));
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            rb.AddForce(new OVector3(Vector3.Zero, Transform.GetWorld().Backward * 20000));
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            // rb.AddForce(new OVector3(Vector3.Zero, new Vector3() * 20000));
            Transform.Rotate(new Vector3(0, acc, 0));

        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            Transform.Rotate(new Vector3(0, acc, 0));
        }
    }


}