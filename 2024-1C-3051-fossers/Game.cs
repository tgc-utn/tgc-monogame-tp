using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WarSteel.Managers;
using WarSteel.Scenes.Main;

namespace WarSteel;

public class Game : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager Graphics { get; }
    private SpriteBatch SpriteBatch;

    private SceneManager SceneManager;

    public Game()
    {
        Graphics = new GraphicsDeviceManager(this);
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
        // Graphics.IsFullScreen = true;
        Window.AllowUserResizing = true;
        Graphics.PreferredBackBufferWidth = 1280;
        Graphics.PreferredBackBufferHeight = 720;
    }

    protected override void Initialize()
    {
        ContentRepoManager.SetUpInstance(Content);
        SceneManager = new SceneManager();

        SceneManager.AddScene(ScenesNames.MAIN, new MainScene(Graphics, SpriteBatch));
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
        var originalRasterizerState = GraphicsDevice.RasterizerState;
        var rasterizerState = new RasterizerState();
        rasterizerState.CullMode = CullMode.None;
        Graphics.GraphicsDevice.RasterizerState = rasterizerState;

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