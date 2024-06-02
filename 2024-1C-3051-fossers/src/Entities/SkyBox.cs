using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Entities;
using WarSteel.Managers;
using WarSteel.Scenes;

class SkyBoxRenderable : Renderable
{
    public SkyBoxRenderable(Model model) : base(model)
    {

    }

    public override void Draw(Transform transform, Scene scene)
    {

    }
}

public class SkyBox : Entity
{
    public SkyBox() : base("skybox", Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {

    }


    public override void Initialize(Scene scene)
    {
        Model model = ContentRepoManager.Instance().GetModel("SkyBox/cube");
        TextureCube skyboxTexture = ContentRepoManager.Instance().GetTextureCube("sunset");
        Renderable = new Renderable(model);
        this.Transform.Dimensions = new(1000, 1000, 1000);
        Renderable.AddShader("skybox", new SkyBoxShader(skyboxTexture));
        base.Initialize(scene);
    }
}