using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarSteel.Managers;

namespace WarSteel;

public class Game : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager Graphics { get; }

    public Game()
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
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
        GraphicsDevice.Clear(Color.Black);
        SceneManager.Instance().CurrentScene().Draw();

        base.Draw(gameTime);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        SceneManager.Instance().CurrentScene().Update();

        base.Update(gameTime);
    }

    protected override void UnloadContent()
    {
        SceneManager.Instance().CurrentScene().Unload();
        Content.Unload();
        base.UnloadContent();
    }
}