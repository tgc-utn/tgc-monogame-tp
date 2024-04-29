using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WarSteel.Managers;
using WarSteel.Scenes;
using WarSteel.Scenes.Main;

namespace WarSteel;

public class Game : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager Graphics { get; }

    private SceneManager SceneManager;

    public Game()
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        ContentRepoManager.SetUpInstance(Content);
        SceneManager = new SceneManager();

        SceneManager.AddScene(ScenesNames.MAIN, new MainScene(Graphics));
        SceneManager.SetCurrentScene(ScenesNames.MAIN);
        
        
        SceneManager.CurrentScene().Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        SceneManager.CurrentScene().LoadContent();
        base.LoadContent();
    }

    protected override void Draw(GameTime gameTime)
    {
        Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
        SceneManager.CurrentScene().Draw();
        base.Draw(gameTime);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        SceneManager.CurrentScene().Update(gameTime);

        base.Update(gameTime);
    }

    protected override void UnloadContent()
    {
        SceneManager.CurrentScene().Unload();
        Content.Unload();
        base.UnloadContent();
    }
}