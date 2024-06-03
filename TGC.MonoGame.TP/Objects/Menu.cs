using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;



namespace ThunderingTanks.Objects
{
    public class Menu
    {

        private SpriteFont _font;
        private Rectangle _playButton;
        private Rectangle _exitButton;
        private Texture2D _cursorTexture;
        private Song backgroundSound { get; set; }

        private bool _playing = true;

        public const string ContentFolderMusic = "Music/";



        public Menu(SpriteFont font, Texture2D cursorTexture, Song background)
        {
            backgroundSound = background;
            _font = font;
            _cursorTexture = cursorTexture;

            // Define la posición y tamaño de los botones
            int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int buttonWidth = 200;
            int buttonHeight = 50;
            int buttonSpacing = 20;
            int buttonY = (screenHeight - 2 * buttonHeight - buttonSpacing) / 2; // Centra los botones verticalmente
            int buttonX = (screenWidth - buttonWidth) / 2; // Centra los botones horizontalmente
            _playButton = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
            _exitButton = new Rectangle(buttonX, buttonY + buttonHeight + buttonSpacing, buttonWidth, buttonHeight);
        }




        public void Update(ref bool juegoIniciado)
        {
            // Verifica si se hace clic en los botones
            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                var mousePosition = new Point(mouseState.X, mouseState.Y);
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
            }
            if (_playing)
            {
                //Parar y reproducir MP3
                MediaPlayer.Play(backgroundSound);
                _playing = false;
            }
       
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Dibuja los botones como rectángulos grises
            spriteBatch.Draw(_cursorTexture, _playButton, Color.Gray);
            spriteBatch.Draw(_cursorTexture, _exitButton, Color.Gray);

            // Ajusta la posición del texto dentro de los botones
            Vector2 playTextPosition = new Vector2(_playButton.X + (_playButton.Width - _font.MeasureString("Play").X) / 2, _playButton.Y + (_playButton.Height - _font.MeasureString("Play").Y) / 2);
            Vector2 exitTextPosition = new Vector2(_exitButton.X + (_exitButton.Width - _font.MeasureString("Exit").X) / 2, _exitButton.Y + (_exitButton.Height - _font.MeasureString("Exit").Y) / 2);

            // Dibuja el texto en los botones agrandando el tamaño
            spriteBatch.DrawString(_font, "Play", playTextPosition, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(_font, "Exit", exitTextPosition, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);

            var mouseState = Mouse.GetState();
            spriteBatch.Draw(_cursorTexture, new Vector2(mouseState.X, mouseState.Y), Color.White);
        }


    }
}
