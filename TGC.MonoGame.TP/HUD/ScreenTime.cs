using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Utils;


namespace TGC.MonoGame.TP.HUD;

public class ScreenTime : ScreenText
{
    private float _currentGameTime;
    private float _timeLimit;

    internal override Vector2 LogoLocation() => new Vector2(ScreenWidth/2f - Logo.Width/3.5f, ScreenHeight/10f);
    internal override Vector2 TextLocation() => new Vector2(ScreenWidth/2f, ScreenHeight/10f);
    internal override string TextToDraw() => TimeToString(_currentGameTime);

    public ScreenTime(GraphicsDevice graphicsDevice, float timeLimit) : base(graphicsDevice)
    {
        _timeLimit = timeLimit;
        _currentGameTime = _timeLimit;
        LogoScale = 0.15f;
        TextScale = 2f;
    }

    public override void LoadContent(ContentManager content)
    {
        Font = content.Load<SpriteFont>($"{ContentFolder.Fonts}/Stencil16");
        Logo = content.Load<Texture2D>($"{ContentFolder.Images}/time");
    }
    
    public void Update(GameTime gameTime)
    {
        _currentGameTime -= (float) gameTime.ElapsedGameTime.TotalSeconds;
    }

    public string TimeToString(float time)
    {
        var minutes = (int) time / 60;
        var seconds = (int) time % 60f;

        var secondsString = seconds.ToString("0");

        if (seconds < 10)
            secondsString = "0" + secondsString;
        
        return minutes.ToString("0") + ":" + secondsString;
    }
    
    public void Reset()
    {
        _currentGameTime = _timeLimit;
    }
}