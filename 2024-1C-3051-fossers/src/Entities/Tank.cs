using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;

namespace WarSteel.Entities;

public class Tank : Entity
{
    public Tank(string name) : base(name, Array.Empty<string>(), new Transform())
    {

    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Tanks/Panzer/Panzer");
        Shader texture = new TextureShader(ContentRepoManager.Instance().GetTexture("Tanks/T90/textures_mod/hullA"));
        _renderable = new Renderable(model, texture);

        base.LoadContent();
    }

    public override void Draw(Camera camera)
    {
        base.Draw(camera);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void OnDestroy()
    {
        base.Initialize();
    }

}