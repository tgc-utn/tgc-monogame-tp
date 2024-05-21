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
    public TankRenderable(Model model) : base(model) { }

    public override void Draw(Matrix world, Scene scene)
    {
        Matrix view = scene.GetCamera().View;
        Matrix projection = scene.GetCamera().Projection;

        var modelMeshesBaseTransforms = new Matrix[_model.Bones.Count];
        _model.CopyAbsoluteBoneTransformsTo(modelMeshesBaseTransforms);

        foreach (ModelMesh mesh in _model.Meshes)
        {
            foreach (var shader in _shaders)
            {
                shader.Value.UseCamera(scene.GetCamera());
                shader.Value.ApplyEffects(scene);
                shader.Value.UseWorld(world);
            }

            foreach (Effect effect in mesh.Effects)
            {
                var relativeTransform = modelMeshesBaseTransforms[mesh.ParentBone.Index];
                effect.Parameters["World"].SetValue(relativeTransform * world);
                effect.Parameters["View"].SetValue(view);
                effect.Parameters["Projection"].SetValue(projection);
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
        _renderable = new TankRenderable(model);
        _renderable.AddShader("phong", texture);

        base.LoadContent();
    }

    public override void Update(GameTime gameTime, Scene scene)
    {
        base.Update(gameTime, scene);
    }
}

