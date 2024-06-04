using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Entities;
using WarSteel.Managers;
using WarSteel.Scenes;



public class SkyBox : Entity
{
    public SkyBox() : base("skybox", Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {

    }


    public override void Initialize(Scene scene)
    {
        Model model = ContentRepoManager.Instance().GetModel("SkyBox/cube");
        TextureCube skyboxTexture = ContentRepoManager.Instance().GetTextureCube("sunset");
        Renderable = new Renderable(model,new SkyBoxShader(skyboxTexture));
        Transform.Dimensions = new(1000, 1000, 1000);
        base.Initialize(scene);
    }
}