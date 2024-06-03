using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarSteel.Scenes;
using WarSteel.UIKit;

public class UIProcessor : ISceneProcessor
{

    private List<UI> _uiElements = new List<UI>();

    public UIProcessor()
    {

    }

    public void Initialize(Scene scene)
    {

    }

    public void AddUi(UI ui)
    {
        _uiElements.Add(ui);
    }

    public void RemoveUi(UI ui)
    {
        ui.toDestroy = true;
    }

    public void Draw(Scene scene)
    {
        scene.GetSpriteBatch().Begin();
        _uiElements.ForEach(ui => ui.Draw(scene));
        scene.GetSpriteBatch().End();
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

        if (clicked)
        {
            _uiElements.ForEach(ui =>
            {
                if (ui.IsBeingClicked(position))
                    ui.OnClick(scene);
            });
        }
    }

    public void CheckDeletions()
    {
        List<UI> copyList = new List<UI>(_uiElements);

        copyList.ForEach(ui =>
        {
            if (ui.toDestroy)
                _uiElements.Remove(ui);
        });
    }
}