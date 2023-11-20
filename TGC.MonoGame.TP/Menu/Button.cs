using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Types;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Menu;

public class Button
{
    public int Width;
    public int Height;

    private SpriteFont Font;
    
    private string _text;
    public bool IsHovered;
    private readonly GameStatus _gameStatus;

    private Texture2D _normalImg;
    private Texture2D _hoveredImg;

    private Rectangle _rectangle; 

    public Button(string text, GameStatus gameStatus)
    {
        _text = text;
        _gameStatus = gameStatus;
    }

    public void LoadContent(ContentManager contentManager)
    {
        Font = contentManager.Load<SpriteFont>($"{ContentFolder.Fonts}/Arial16");
        _normalImg = contentManager.Load<Texture2D>($"{ContentFolder.Images}/button");
        _hoveredImg = contentManager.Load<Texture2D>($"{ContentFolder.Images}/hoverbutton");
        Width = _normalImg.Width/2;
        Height = _normalImg.Height/2;
    }

    public Rectangle SetDrawPosition(int x, int y)
    {
        _rectangle = new Rectangle(x-Width/2, y, Width, Height);
        return _rectangle;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(IsHovered ? _hoveredImg : _normalImg, _rectangle, Color.White);
        
        var textSize = Font.MeasureString(_text);
        spriteBatch.DrawString(Font, _text, new Vector2(_rectangle.X + _rectangle.Width/2f - textSize.X/2, 
                                                                _rectangle.Y + _rectangle.Height/2f - textSize.Y/2), Color.Black);
        spriteBatch.End();
    }

    public void Select(GameState gameState)
    {
        gameState.Set(_gameStatus);
    }
}
