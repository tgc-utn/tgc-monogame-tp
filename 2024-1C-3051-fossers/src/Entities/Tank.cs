using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;
using WarSteel.Scenes;

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
    class TankCollider : Collider
    {
        public TankCollider() : base(new BoxCollider(200, 200, 200)) { }

        public override void OnCollide(Collision other)
        {

        }
    }

    public Tank(string name) : base(name, Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
        AddComponent(new DynamicBody(Transform, new TankCollider(), 5));
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

