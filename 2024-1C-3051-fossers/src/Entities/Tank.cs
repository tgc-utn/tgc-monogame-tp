using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;
using WarSteel.Scenes;
using WarSteel.Scenes.Main;
using WarSteel.Utils;

namespace WarSteel.Entities;

public class TankRenderable : Renderable
{

    private static readonly string TurretBone = "Turret";
    private static readonly string Cannonbone = "Cannon";
    private Transform _turretTransform;
    private Transform _cannonTransform;

    public TankRenderable(Model model, Transform turretTransform, Transform cannonTransform) : base(model)
    {
        _turretTransform = turretTransform;
        _cannonTransform = cannonTransform;
    }

    public override Matrix GetMatrix(ModelMesh mesh, Transform transform)
    {

        if (mesh.Name is string t && t == TurretBone)
        {
            return _turretTransform.LocalToWorldMatrix(mesh.ParentBone.Transform);
        }

        if (mesh.Name is string c && c == Cannonbone)
        {
            return _cannonTransform.LocalToWorldMatrix(mesh.ParentBone.Transform);
        }

        return base.GetMatrix(mesh, transform);
    }

}

public class Tank : Entity
{
    private Transform _turretTransform;
    private Transform _cannonTransform;

    public Tank(string name) : base(name, Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {

    }

    public override void Initialize(Scene scene)
    {
        _turretTransform = new Transform(Transform, Vector3.Zero);
        _cannonTransform = new Transform(_turretTransform, Vector3.Zero);
        AddComponent(new DynamicBody(new Collider(new BoxShape(200, 325, 450), new NoAction()), new Vector3(0, 100, 0), 200, 0.9f, 2f));
        AddComponent(new PlayerControls(_cannonTransform));
        AddComponent(new TurretController(_turretTransform, scene.GetCamera(), 3f));
        AddComponent(new CannonController(_cannonTransform, scene.GetCamera(), 3f));
        base.Initialize(scene);
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Tanks/Panzer/Panzer");
        Shader texture = new PhongShader(0.2f, 0.5f, Color.Blue);

        Renderable = new TankRenderable(model, _turretTransform, _cannonTransform);
        Renderable.AddShader("phong", texture);

        base.LoadContent();
    }

    public override void Update(GameTime gameTime, Scene scene)
    {
        base.Update(gameTime, scene);
    }

    public override void Draw(Scene scene)
    {
        base.Draw(scene);
    }
}
