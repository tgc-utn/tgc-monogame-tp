
using Microsoft.Xna.Framework;
using WarSteel.Entities;
using WarSteel.Common;
using System;
using WarSteel.Managers;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Entities.Map;

namespace WarSteel.Scenes.Main;

public class MainScene : Scene
{
    public MainScene(GraphicsDeviceManager Graphics) : base(Graphics)
    {
    }

    public override void Initialize()
    {
        entities.Add(new Tank("player"));
        entities.Add(new Ground());
        camera = new Camera(new Vector3(1000, 1000, 1000), Graphics.GraphicsDevice.Viewport.AspectRatio);

        base.Initialize();
    }

    public override void LoadContent()
    {
        base.LoadContent();
    }

    public override void Draw()
    {
        base.Draw();
    }

    public override void Update(GameTime gameTime)
    {
        camera.Follow(entities.GetByName("player"));

        base.Update(gameTime);
    }
}