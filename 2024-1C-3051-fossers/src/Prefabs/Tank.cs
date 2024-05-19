using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;
using WarSteel.Scenes;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace WarSteel.Entities;

class TankRenderable : Renderable
{
    private Matrix[] boneTransforms;

    public TankRenderable(Model model) : base(model) { }

    public override void Draw(Matrix world, Scene scene)
    {
        // Look up combined bone matrices for the entire model.

        // Draw the model.
        foreach (var mesh in _model.Meshes)
        {

            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.World = boneTransforms[mesh.ParentBone.Index];
                effect.View = scene.GetCamera().View;
                effect.Projection = scene.GetCamera().Projection;

                effect.EnableDefaultLighting();
            }

            mesh.Draw();
        }
    }
}

public class Tank : Entity
{
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
        base.Update(gameTime, scene);
    }
}