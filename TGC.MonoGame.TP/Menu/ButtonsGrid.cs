using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading;
using TGC.MonoGame.TP.Types;

namespace TGC.MonoGame.TP.Menu;

public class ButtonsGrid
{
    private const int VerticalSpacing = 80;

    private GameState _gameState;
    private int _x;
    private int _startY;
    private List<Button> _buttons;
    private List<Rectangle> _buttonsRectangles;

    public ButtonsGrid(GameState gameState, int x, int startY, List<Button> buttons)
    {
        _gameState = gameState;
        _x = x;
        _startY = startY;
        _buttons = buttons;
        _buttonsRectangles = new List<Rectangle>();
    }

    public void LoadContent(ContentManager contentManager)
    {
        for (var buttonIndex = 0; buttonIndex < _buttons.Count; buttonIndex++)
        {
            var button = _buttons[buttonIndex];
            
            button.LoadContent(contentManager);
            
            var buttonY = _startY + buttonIndex * VerticalSpacing;

            var buttonRectangle = button.SetDrawPosition(_x, buttonY);
            _buttonsRectangles.Add(buttonRectangle);
        }
    }

    public void Update(MouseState mouseState)
    {
        for (var index = 0; index < _buttons.Count; index++)
        {
            var button = _buttons[index];
            var buttonRectangle = _buttonsRectangles[index];

            var mousePosition = new Point(mouseState.X, mouseState.Y);
            button.IsHovered = buttonRectangle.Contains(mousePosition);
            
            if (button.IsHovered && mouseState.LeftButton == ButtonState.Pressed) button.Select(_gameState); 
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (var index = 0; index < _buttons.Count; index++)
        {
            var button = _buttons[index];
            button.Draw(spriteBatch);
        }
    }

}
