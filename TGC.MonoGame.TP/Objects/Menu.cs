using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;



namespace ThunderingTanks.Objects
{
    public class Menu
    {
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderModels = "Models/";
        public const string ContentFolderTextures = "Textures/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderFonts = "Fonts/";

        private float ScreenWidth { get; set; }
        private float ScreenHeight { get; set; }

        public float MasterSound { get; set; }
        private Song backgroundSound { get; set; }

        private SpriteFont _font;
        private SpriteFont WarIsOver;

        private Texture2D _cursorTexture;

        private bool _playing = true;

        #region BOTONS

        private Rectangle _playButton;
        private Rectangle _exitButton;
        private Rectangle _soundOnButton;
        private Rectangle _soundOffButton;
        private Texture2D RectangleButtonHover { get; set; }
        private Texture2D RectangleButton { get; set; }
        private Texture2D PlayButton { get; set; }
        private Texture2D PlayButtonHover { get; set; }
        private Texture2D SoundOnButton { get; set; }
        private Texture2D SoundOnButtonHover { get; set; }
        private Texture2D SoundOffButton { get; set; }
        private Texture2D SoundOffButtonHover { get; set; }
        private Texture2D RectangleButtonNormal { get; set; }
        private Texture2D PlayButtonNormal { get; set; }
        private Texture2D SoundOnButtonNormal { get; set; }
        private Texture2D SoundOffButtonNormal { get; set; }

        #endregion

        private bool SoundIsOn = true;

        public Menu(ContentManager contentManager)
        {

            ScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            ScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            int buttonWidth = 400;
            int buttonHeight = 200;
            int buttonSpacing = 20;

            int buttonY = (int) (ScreenHeight - 2 * buttonHeight - buttonSpacing) / 2;           
            int buttonX = (int) ScreenWidth - buttonWidth - 300;

            _playButton = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
            _exitButton = new Rectangle(buttonX, buttonY + buttonHeight + buttonSpacing, buttonWidth, buttonHeight);

            _soundOnButton = new Rectangle(50, (int)(ScreenHeight - 300), 100, 100);
            _soundOffButton = new Rectangle(50, (int)(ScreenHeight - 300), 100, 100);

        }

        public void LoadContent(ContentManager Content)
        {

            backgroundSound = Content.Load<Song>(ContentFolderMusic + "TankGameBackgroundSound");
            _cursorTexture = Content.Load<Texture2D>(ContentFolderTextures + "proyectilMouse");
            _font = Content.Load<SpriteFont>(ContentFolderFonts + "arial");
            WarIsOver = Content.Load<SpriteFont>(ContentFolderFonts + "warisover/WarIsOver");

            RectangleButtonNormal = Content.Load<Texture2D>(ContentFolderTextures + "Menu/Default@4x");
            PlayButtonNormal = Content.Load<Texture2D>(ContentFolderTextures + "Menu/PlayButton");
            SoundOnButtonNormal = Content.Load<Texture2D>(ContentFolderTextures + "Menu/SoundOn");
            SoundOffButtonNormal = Content.Load<Texture2D>(ContentFolderTextures + "Menu/SoundOff");

            RectangleButtonHover = Content.Load<Texture2D>(ContentFolderTextures + "Menu/RectangleButtonHover");
            PlayButtonHover = Content.Load<Texture2D>(ContentFolderTextures + "Menu/PlayButtonHover");
            SoundOnButtonHover = Content.Load<Texture2D>(ContentFolderTextures + "Menu/SoundOnHover");
            SoundOffButtonHover = Content.Load<Texture2D>(ContentFolderTextures + "Menu/SoundOffHover");

        }

        public void Update(ref bool juegoIniciado, GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);

            MediaPlayer.Volume = MasterSound;

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_playButton.Contains(mousePosition))
                {
                    juegoIniciado = true;
                    MediaPlayer.Stop();
                }
                else if (_exitButton.Contains(mousePosition))
                {
                    Environment.Exit(0); 
                }
                if (SoundIsOn && _soundOffButton.Contains(mousePosition))
                {
                    if (MediaPlayer.State == MediaState.Playing)
                        MediaPlayer.Pause();

                    SoundIsOn = false;
                }
                else if (!SoundIsOn && _soundOnButton.Contains(mousePosition))
                {
                    if (MediaPlayer.State == MediaState.Paused)
                        MediaPlayer.Resume();

                    SoundIsOn = true;
                }
            }

            if (_playButton.Contains(mousePosition))
            {
                PlayButton = PlayButtonHover;
            }
            else
            {
                PlayButton = PlayButtonNormal;
            }
            if (_exitButton.Contains(mousePosition))
            {
                RectangleButton = RectangleButtonHover;
            }
            else
            {
                RectangleButton = RectangleButtonNormal;
            }
            if (_soundOnButton.Contains(mousePosition))
            {
                SoundOnButton = SoundOnButtonHover;
            }
            else
            {
                SoundOnButton = SoundOnButtonNormal;
            }
            if (_soundOffButton.Contains(mousePosition))
            {
                SoundOffButton = SoundOffButtonHover;
            }
            else
            {
                SoundOffButton = SoundOffButtonNormal;
            }
            if (_playing)
            {
                MediaPlayer.Play(backgroundSound);
                _playing = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            if (SoundIsOn)
            {
                spriteBatch.Draw(SoundOnButton, _soundOnButton, Color.Gray);
            }
            else
            {
                spriteBatch.Draw(SoundOffButton, _soundOffButton, Color.Gray);
            }

            spriteBatch.Draw(PlayButton, _playButton, Color.Gray);
            spriteBatch.Draw(RectangleButton, _exitButton, Color.Gray);

            spriteBatch.DrawString(WarIsOver, "THUNDERING TANKS", new Vector2(450,  200), Color.SandyBrown);
            spriteBatch.DrawString(WarIsOver, "VOLUMEN = " + MasterSound.ToString("P"), new Vector2(50, ScreenHeight - 200), Color.SaddleBrown);
           
            Vector2 exitTextPosition = new Vector2(_exitButton.X + (_exitButton.Width - _font.MeasureString("Exit").X) / 2, _exitButton.Y + (_exitButton.Height - _font.MeasureString("Exit").Y) / 2);

            spriteBatch.DrawString(_font, "Exit", exitTextPosition, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);

            var mouseState = Mouse.GetState();

            spriteBatch.Draw(_cursorTexture, new Vector2(mouseState.X, mouseState.Y), Color.White);
        }
    }
}
