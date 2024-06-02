using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WarSteel.Scenes;

public class UIProcessor : ISceneProcessor
{

    private List<UI> _uiElements = new List<UI>();
    private SpriteBatch _spriteBatch;

    public UIProcessor(SpriteBatch spriteBatch)
    {

    }

    /*
    TO
    */
    public void Initialize(Scene scene)
    {


    }

    public void Draw(Scene scene)
    {
        _uiElements.ForEach(ui => ui.Draw(scene/* ,spritebatch */));
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
                {
                    ui.OnClick(scene, position);
                }

            });
        }

    }

    public void CheckDeletions()
    {

        List<UI> copyList = new List<UI>();

        copyList.ForEach(ui =>
        {
            // if (ui.ToDestroy)
            // {
            //     _uiElements.Remove(ui);
            // }
        });

    }


}