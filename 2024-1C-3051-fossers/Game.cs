using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WarSteel.Managers;

namespace WarSteel;

public class Game : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager Graphics { get; }
    private SpriteBatch spriteBatch;

    public Game()
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        EntitiesManager.SetUpInstance();
        ContentRepoManager.SetUpInstance(Content);
        SceneManager.SetUpInstance(ScenesNames.MAIN, Graphics);
        SceneManager.Instance().CurrentScene().Initialize();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        SceneManager.Instance().CurrentScene().LoadContent();

        base.LoadContent();
    }

    protected override void Draw(GameTime gameTime)
    {
        Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
        SceneManager.Instance().CurrentScene().Draw();
        base.Draw(gameTime);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        SceneManager.Instance().CurrentScene().Update(gameTime);

        base.Update(gameTime);
    }

    protected override void UnloadContent()
    {
        SceneManager.Instance().CurrentScene().Unload();
        Content.Unload();
        base.UnloadContent();
    }
}