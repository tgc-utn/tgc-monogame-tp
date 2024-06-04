using System;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
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

        private ContentManager _contentManager;

        private GraphicsDeviceManager Graphics { get; }

        Viewport viewport;


        private Song backgroundSound { get; set; }

        private SpriteFont _font;

        private Rectangle _playButton;
        private Rectangle _exitButton;
        private Rectangle _soundOnButton;
        private Rectangle _soundOffButton;

        private Texture2D _cursorTexture;

        private bool _playing = true;

        //BOTONES
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



        public Menu(SpriteFont font, Texture2D cursorTexture, Song background, ContentManager contentManager)
        {

            _contentManager = contentManager;

            backgroundSound = background;
            _font = font;
            _cursorTexture = cursorTexture;

            // Define la posición y tamaño de los botones
            int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int buttonWidth = 400;
            int buttonHeight = 200;
            int buttonSpacing = 20;
            int buttonY = (screenHeight - 2 * buttonHeight - buttonSpacing) / 2; // Centra los botones verticalmente
            //int buttonX = (screenWidth - buttonWidth) / 2; // Centra los botones horizontalmente
            int buttonX = screenWidth - buttonWidth - 300;
            _playButton = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
            _exitButton = new Rectangle(buttonX, buttonY + buttonHeight + buttonSpacing, buttonWidth, buttonHeight);
            _soundOnButton = new Rectangle(50, screenHeight - 300, 100, 100);
            _soundOffButton = new Rectangle(50, screenHeight - 300, 100, 100);

        }

        public void LoadContent(ContentManager Content)
        {

            RectangleButtonNormal = Content.Load<Texture2D>(ContentFolderTextures + "Menu/Default@4x");
            PlayButtonNormal = Content.Load<Texture2D>(ContentFolderTextures + "Menu/PlayButton");
            SoundOnButtonNormal = Content.Load<Texture2D>(ContentFolderTextures + "Menu/SoundOn");
            SoundOffButtonNormal = Content.Load<Texture2D>(ContentFolderTextures + "Menu/SoundOff");

            //HOVERS
            RectangleButtonHover = Content.Load<Texture2D>(ContentFolderTextures + "Menu/RectangleButtonHover");
            PlayButtonHover = Content.Load<Texture2D>(ContentFolderTextures + "Menu/PlayButtonHover");
            SoundOnButtonHover = Content.Load<Texture2D>(ContentFolderTextures + "Menu/SoundOnHover");
            SoundOffButtonHover = Content.Load<Texture2D>(ContentFolderTextures + "Menu/SoundOffHover");

        }


        private bool SoundIsOn = true;
        public void Update(ref bool juegoIniciado, GameTime gameTime)
        {
            // Verifica si se hace clic en los botones
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_playButton.Contains(mousePosition))
                {
                    // Lógica para iniciar el juego
                    juegoIniciado = true;
                    MediaPlayer.Stop();
                }
                else if (_exitButton.Contains(mousePosition))
                {

                    // Lógica para salir del juego
                    Environment.Exit(0); // Cierra la aplicación

                }
                if (SoundIsOn && _soundOffButton.Contains(mousePosition))
                {
                    // Pausa el sonido si está reproduciéndose
                    if (MediaPlayer.State == MediaState.Playing)
                        MediaPlayer.Pause();

                    // Cambia el estado del sonido y actualiza la textura del botón
                    SoundIsOn = false;
                }
                else if (!SoundIsOn && _soundOnButton.Contains(mousePosition))
                {
                    // Reanuda el sonido si está pausado
                    if (MediaPlayer.State == MediaState.Paused)
                        MediaPlayer.Resume();

                    // Cambia el estado del sonido y actualiza la textura del botón
                    SoundIsOn = true;
                }
            }
            // Verifica si el mouse está sobre los botones para cambiar a las texturas hover
            if (_playButton.Contains(mousePosition))
            {
                //_playButton = _playButtonHover;
                PlayButton = PlayButtonHover;
            }
            else
            {
                PlayButton = PlayButtonNormal;
            }
            if (_exitButton.Contains(mousePosition))
            {
                //_exitButton = _exitButtonHover;
                RectangleButton = RectangleButtonHover;
            }
            else
            {
                RectangleButton = RectangleButtonNormal;
            }
            if (_soundOnButton.Contains(mousePosition))
            {
                //_soundOnButton = _soundOnButtonHover;
                SoundOnButton = SoundOnButtonHover;
            }
            else
            {
                SoundOnButton = SoundOnButtonNormal;
            }
            if (_soundOffButton.Contains(mousePosition))
            {
                //_soundOffButton = _soundOffButtonHover;
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

            // Ajusta la posición del texto dentro de los botones
            Vector2 playTextPosition = new Vector2(_playButton.X + (_playButton.Width - _font.MeasureString("Play").X) / 2, _playButton.Y + (_playButton.Height - _font.MeasureString("Play").Y) / 2);
            Vector2 exitTextPosition = new Vector2(_exitButton.X + (_exitButton.Width - _font.MeasureString("Exit").X) / 2, _exitButton.Y + (_exitButton.Height - _font.MeasureString("Exit").Y) / 2);

            // Dibuja el texto en los botones agrandando el tamaño
            spriteBatch.DrawString(_font, "Exit", exitTextPosition, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);

            var mouseState = Mouse.GetState();
            spriteBatch.Draw(_cursorTexture, new Vector2(mouseState.X, mouseState.Y), Color.White);
        }


    }
}
