using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;
using WarSteel.Scenes;
using WarSteel.Utils;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace WarSteel.Entities;

public class Tank : Entity
{
    public Tank(string name) : base(name, Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
        Model model = ContentRepoManager.Instance().GetModel("Tanks/Panzer/Panzer");
        float width = ModelUtils.GetWidth(model);
        float height = ModelUtils.GetHeight(model);
        AddComponent(new RigidBody(Transform,new BoxCollider(Transform,new Vector3(-width,-height,-height), new Vector3(width,height,height)),20,Matrix.Identity, false));
        AddComponent(new LightComponent(Color.White,new Vector3(2000,0,0)));
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
}