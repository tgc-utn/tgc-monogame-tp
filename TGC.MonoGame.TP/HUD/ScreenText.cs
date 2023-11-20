using System.Data.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.HUD;

public abstract class ScreenText
{
    internal int ScreenWidth;
    internal int ScreenHeight;

    internal Texture2D Logo;
    internal SpriteFont Font;
    
    internal float LogoScale;
    internal float TextScale;

    internal abstract Vector2 LogoLocation();
    internal abstract Vector2 TextLocation();
    
    internal abstract string TextToDraw();

    private SpriteBatch _spriteBatch;
    
    public ScreenText(GraphicsDevice graphicsDevice)
    {
        _spriteBatch = new SpriteBatch(graphicsDevice);
        ScreenWidth = graphicsDevice.Viewport.Width;
        ScreenHeight = graphicsDevice.Viewport.Height;
    }
    
    public abstract void LoadContent(ContentManager content);
    
    public void Draw()
    {
        var fixPosition = new Vector2(1, -1);
        _spriteBatch.Begin();

        //Logo
        _spriteBatch.Draw(Logo, LogoLocation(), null, Color.White, 0f, Vector2.Zero, LogoScale, SpriteEffects.None, 0);

        //Texto
        var textSize = Font.MeasureString(TextToDraw());
        _spriteBatch.DrawString(Font, TextToDraw(), TextLocation() - (textSize / 2) * fixPosition, Color.Black, 0f, Vector2.Zero, TextScale,
            SpriteEffects.None, 0);

        _spriteBatch.End();
    }
}