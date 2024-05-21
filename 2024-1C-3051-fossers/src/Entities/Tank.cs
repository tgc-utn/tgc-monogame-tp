using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.Entities;

public class TankRenderable : Renderable
{
    private Matrix[] boneTransforms;
    private ModelBone turretBone;
    private Matrix turretTransform;
    public ModelBone cannonBone;
    public Matrix cannonTransform;

    public Transform CannonTransform
    {
        get => new(cannonTransform);
    }

    public TankRenderable(Model model) : base(model)
    {
        boneTransforms = new Matrix[model.Bones.Count];
        turretBone = model.Bones["Turret"];
        turretTransform = turretBone.Transform;
        cannonBone = model.Bones["Cannon"];
        cannonTransform = cannonBone.Transform;
    }

    public override void Draw(Matrix world, Scene scene)
    {
        Matrix view = scene.GetCamera().View;
        Matrix projection = scene.GetCamera().Projection;
        MouseController mouse = scene.GetCamera().GetComponent<MouseController>();

        _model.CopyAbsoluteBoneTransformsTo(boneTransforms);

        Matrix turretRotation = Matrix.CreateRotationY(TurretRotation(mouse));
        Matrix cannonRotation = Matrix.CreateRotationX(CannonRotation(mouse));


        turretBone.Transform = turretRotation * turretTransform;
        cannonBone.Transform = cannonRotation * cannonTransform;

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
                var relativeTransform = boneTransforms[mesh.ParentBone.Index];
                effect.Parameters["World"].SetValue(relativeTransform * world);
                effect.Parameters["View"].SetValue(view);
                effect.Parameters["Projection"].SetValue(projection);
            }

            mesh.Draw();
        }
    }


    public float TurretRotation(MouseController mouse)
    {
        return -mouse.Yaw - MathHelper.PiOver2;
    }

    public float CannonRotation(MouseController mouse)
    {
        float pitchThreshold = MathHelper.ToRadians(60f);
        float pitch = 0f;
        if (mouse.Pitch > pitchThreshold)
        {
            pitch = mouse.Pitch - pitchThreshold;
        }
        return -pitch;

    }
}

public class Tank : Entity
{
    public TankRenderable Renderable;

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
        Renderable = new TankRenderable(model);
        _renderable = Renderable;
        _renderable.AddShader("phong", texture);

        base.LoadContent();
    }

    public override void Update(GameTime gameTime, Scene scene)
    {
        base.Update(gameTime, scene);
    }
}

