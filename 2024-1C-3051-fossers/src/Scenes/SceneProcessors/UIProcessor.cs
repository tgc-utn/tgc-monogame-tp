using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarSteel.Scenes;
using WarSteel.UIKit;

public class UIProcessor : ISceneProcessor
{

    // these are used to prevent double clicks when new ui is rapidly added
    private TimeSpan _clickCooldown = TimeSpan.FromSeconds(0.1);
    private DateTime _lastClickTime = DateTime.MinValue;

    private List<UIScreen> _screens = new List<UIScreen>();

    public UIProcessor()
    {

    }

    public void Initialize(Scene scene)
    {

    }

    public void AddScreen(UIScreen screen)
    {
        _screens.Add(screen);
    }

    public void RemoveScreen(UIScreen screen)
    {
        _screens.Remove(screen);
    }

    public void Draw(Scene scene)
    {
        scene.SpriteBatch.Begin();
        _screens.ForEach(screen => screen.UIElems.ForEach(ui => ui.Draw(scene)));
        scene.SpriteBatch.End();
    }

    public void Update(Scene scene, GameTime gameTime)
    {
        CheckClick(scene, gameTime);
        CheckDeletions();
    }

    private void CheckClick(Scene scene, GameTime gameTime)
    {
        MouseState state = Mouse.GetState();
        Vector2 position = new Vector2(state.X, state.Y);

        bool clicked = state.LeftButton == ButtonState.Pressed;

        List<UIScreen> copyList = new List<UIScreen>(_screens);
        if (clicked && (DateTime.Now - _lastClickTime) >= _clickCooldown)
        {
            _lastClickTime = DateTime.Now;
            copyList.ForEach(screen =>
            {
                List<UI> copyList = new List<UI>(screen.UIElems);
                copyList.ForEach(ui =>
                {
                    if (ui.IsBeingClicked(position))
                        ui.OnClick(scene);
                });
            }
            );
        }
    }

    public void CheckDeletions()
    {
        _screens.ForEach(screen =>
        {
            List<UI> copyList = new List<UI>(screen.UIElems);
            screen.UIElems.RemoveAll(ui => ui.toDestroy);
        });
    }
}