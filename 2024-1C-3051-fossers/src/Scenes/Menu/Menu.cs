
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Managers;
using WarSteel.UIKit;
using WarSteel.Utils;


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
        Vector2 screenCenter = Screen.GetScreenCenter(GraphicsDeviceManager);
        int screenWidth = Screen.GetScreenWidth(GraphicsDeviceManager);
        int screenHeight = Screen.GetScreenHeight(GraphicsDeviceManager);

        Func<int, Vector3> GetButtonPos = (int pos) =>
        {
            Vector2 screenCenter = Screen.GetScreenCenter(GraphicsDeviceManager);
            int margin = 90;
            return new Vector3(screenCenter.X, screenCenter.Y - 50 + margin * pos, 0);
        };

        // ui elems
        UI background = new UI(new Vector3(screenCenter.X, screenCenter.Y, 0), screenWidth, screenWidth, new Image("UI/menu-bg"));
        UI header = new UI(new Vector3(screenCenter.X, screenCenter.Y - 160, 0), new Header("WARSTEEL"));
        UI startBtn = new UI(GetButtonPos(0), 300, 60, new PrimaryBtn("Start"), new List<UIAction>()
        {
            (scene, ui) => { SceneManager.Instance().SetCurrentScene(ScenesNames.MAIN); },
        });
        UI controlsBtn = new UI(GetButtonPos(1), 300, 60, new SecondaryBtn("Controls"), new List<UIAction>()
        {
        });
        UI exitBtn = new UI(GetButtonPos(2), 300, 60, new SecondaryBtn("Exit"), new List<UIAction>());

        // add elems
        uiProcessor.AddUi(background);
        uiProcessor.AddUi(header);
        uiProcessor.AddUi(startBtn);
        uiProcessor.AddUi(controlsBtn);
        uiProcessor.AddUi(exitBtn);

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