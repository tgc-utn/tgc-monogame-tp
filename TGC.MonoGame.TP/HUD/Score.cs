using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.HUD;

public class Score : ScreenText
{
    private float _score;
    
    public Score(GraphicsDevice graphicsDevice) : base(graphicsDevice)
    {
        _score = 0;
        LogoScale = 0.12f;
        TextScale = 1.75f;
    }

    internal override Vector2 LogoLocation() => new Vector2(10f , 10f);
    internal override Vector2 TextLocation() => new Vector2(10f + Logo.Width / 5f, 15f);

    internal override string TextToDraw() => _score.ToString("0");
    
    public override void LoadContent(ContentManager content)
    { 
        Font = content.Load<SpriteFont>($"{ContentFolder.Fonts}/Stencil16");
        Logo = content.Load<Texture2D>($"{ContentFolder.Images}/score");
    }
    
    public void Update(float newScore)
    {
        _score = newScore;
    }

    public void Reset()
    {
        _score = 0;
    }
    
    
}