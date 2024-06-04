
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Screens;
using WarSteel.Screens.MainMenu;

namespace WarSteel.Scenes.Main;

public class MenuScene : Scene
{

    public MenuScene(GraphicsDeviceManager Graphics, SpriteBatch SpriteBatch) : base(Graphics, SpriteBatch)
    {
    }

    public override void Initialize()
    {
        UIProcessor uiProcessor = new UIProcessor();
        AddSceneProcessor(uiProcessor);
        UIScreen screen = new StartScreen();
        uiProcessor.AddScreen(screen);
        screen.Render(this);

        base.Initialize();
    }


    public override void Draw()
    {
        base.Draw();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}