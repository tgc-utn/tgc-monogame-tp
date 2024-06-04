using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WarSteel.Scenes;
using WarSteel.UIKit;
using WarSteel.Utils;

namespace WarSteel.Screens.MainMenu;

public class StartScreen : UIScreen
{
    MenuScreens currentScreen = MenuScreens.START;

    public StartScreen() : base("start-screen") { }

    public override void Render(Scene scene)
    {
        GraphicsDeviceManager GraphicsDeviceManager = scene.GraphicsDeviceManager;
        Vector2 screenCenter = Screen.GetScreenCenter(GraphicsDeviceManager);
        int screenWidth = Screen.GetScreenWidth(GraphicsDeviceManager);

        // ui elems
        UI background = new UI(new Vector3(screenCenter.X, screenCenter.Y, 0), screenWidth, screenWidth, new Image("UI/menu-bg"));
        AddUIElem(background);

        switch (currentScreen)
        {
            case MenuScreens.START:
                RenderStart(scene);
                break;
            case MenuScreens.CONTROLS:
                RenderControls(scene);
                break;
            case MenuScreens.EXIT_CONFIRM_MSG:
                RenderExit(scene);
                break;
        }

    }

    private void ChangeCurrentScreen(Scene scene, MenuScreens screen)
    {
        currentScreen = screen;
        Remove();
        Render(scene);
    }


    private void RenderStart(Scene scene)
    {
        GraphicsDeviceManager GraphicsDeviceManager = scene.GraphicsDeviceManager;
        Vector2 screenCenter = Screen.GetScreenCenter(GraphicsDeviceManager);
        int screenWidth = Screen.GetScreenWidth(GraphicsDeviceManager);
        int screenHeight = Screen.GetScreenHeight(GraphicsDeviceManager);

        Vector3 GetBtnPos(int pos)
        {
            int margin = 90;
            return new Vector3(screenCenter.X, screenCenter.Y - 50 + margin * pos, 0);
        }

        UI header = new UI(new Vector3(screenCenter.X, screenCenter.Y - 160, 0), new Header("WARSTEEL"));

        UI startBtn = new UI(GetBtnPos(0), 300, 60, new PrimaryBtn("Start"), new List<UIAction>()
        {
            (scene, ui) => { SceneManager.Instance().SetCurrentScene(ScenesNames.MAIN); },
        });
        UI controlsBtn = new UI(GetBtnPos(1), 300, 60, new SecondaryBtn("Controls"), new List<UIAction>()
        {
            (scene, ui) => {  ChangeCurrentScreen(scene, MenuScreens.CONTROLS); }
        });
        UI exitBtn = new UI(GetBtnPos(2), 300, 60, new SecondaryBtn("Exit"), new List<UIAction>(){
            (scene, ui) => {  ChangeCurrentScreen(scene, MenuScreens.EXIT_CONFIRM_MSG); }
        });


        AddUIElem(header);
        AddUIElem(startBtn);
        AddUIElem(controlsBtn);
        AddUIElem(exitBtn);
    }



    private void RenderControls(Scene scene)
    {
        GraphicsDeviceManager GraphicsDeviceManager = scene.GraphicsDeviceManager;
        Vector2 screenCenter = Screen.GetScreenCenter(GraphicsDeviceManager);
        Vector3 GetControlPos(int pos, Vector2 screenCenter)
        {
            int margin = 50;
            return new Vector3(screenCenter.X, screenCenter.Y - 2 * margin + margin * pos, 0);
        }

        UI background = new UI(new Vector3(screenCenter.X, screenCenter.Y, 0), 500, 500, new Image("UI/primary-btn"));
        UI header = new UI(new Vector3(screenCenter.X, 100, 0), new Header("Tank Controls"));
        UI w = new UI(GetControlPos(0, screenCenter), new Paragraph("W - Move tank forward."));
        UI s = new UI(GetControlPos(1, screenCenter), new Paragraph("S - Move tank backwards."));
        UI a = new UI(GetControlPos(2, screenCenter), new Paragraph("A - Rotate tank to the left."));
        UI d = new UI(GetControlPos(3, screenCenter), new Paragraph("D - Rotate tank to the right."));
        UI lmb = new UI(GetControlPos(4, screenCenter), new Paragraph("LMB - Shoot projectile."));
        UI backBtn = new UI(new Vector3(screenCenter.X, Screen.GetScreenHeight(scene.GraphicsDeviceManager) - 50, 0), 300, 60, new SecondaryBtn("Back"), new List<UIAction>()
        {
            (scene, ui) => { ChangeCurrentScreen(scene, MenuScreens.START); },
        });

        AddUIElem(background);
        AddUIElem(header);
        AddUIElem(w);
        AddUIElem(s);
        AddUIElem(a);
        AddUIElem(d);
        AddUIElem(lmb);
        AddUIElem(backBtn);
    }

    private void RenderExit(Scene scene)
    {
        GraphicsDeviceManager GraphicsDeviceManager = scene.GraphicsDeviceManager;
        Vector2 screenCenter = Screen.GetScreenCenter(GraphicsDeviceManager);

        UI confirmMessage = new UI(new Vector3(screenCenter.X, screenCenter.Y - 100, 0), new Paragraph("Are you sure you want to exit?"));

        UI yesBtn = new UI(new Vector3(screenCenter.X - 100, screenCenter.Y, 0), 100, 60, new SecondaryBtn("Yes"), new List<UIAction>()
        {
            (scene, ui) => { Environment.Exit(0); },
        });

        UI noBtn = new UI(new Vector3(screenCenter.X + 100, screenCenter.Y, 0), 100, 60, new PrimaryBtn("No"), new List<UIAction>()
        {
            (scene, ui) => { ChangeCurrentScreen(scene, MenuScreens.START); },
        });


        AddUIElem(confirmMessage);
        AddUIElem(yesBtn);
        AddUIElem(noBtn);
    }
}