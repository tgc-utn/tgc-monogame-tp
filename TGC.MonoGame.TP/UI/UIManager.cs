using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.MonoGame.TP.UI
{
    internal class UIManager
    {
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        public GameStatus UIStatus { get; set; }
        public MenuOption MenuStatus { get; set; }
        public TimeSpan Timer { get; set; }
        public int Score { get; set; }

        private KeyboardState _previouskeyboardState;

        public UIManager(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, SpriteFont font)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _spriteFont = font;
            _previouskeyboardState = Keyboard.GetState();

            UIStatus = GameStatus.Title;
            MenuStatus = MenuOption.Resume;
            Timer = TimeSpan.Zero;
            Score = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (UIStatus == GameStatus.Playing)
            {
                Timer += gameTime.ElapsedGameTime;
            }
            else if (UIStatus == GameStatus.Title)
            {
                if (TitleScreen.PressAnyKey())
                {
                    UIStatus = GameStatus.Playing;
                }
            }
            else if (UIStatus == GameStatus.Menu)
            {
                KeyboardState keyboardState = Keyboard.GetState();
                if ((IsKeyPressed(keyboardState, _previouskeyboardState, Keys.Down) || IsKeyPressed(keyboardState, _previouskeyboardState, Keys.S)) && MenuStatus < MenuOption.Exit)
                {
                    MenuStatus++;
                    //TODO: AudioManager.SelectMenuSound.Play();
                }
                else if ((IsKeyPressed(keyboardState, _previouskeyboardState, Keys.Up) || IsKeyPressed(keyboardState, _previouskeyboardState, Keys.W)) && MenuStatus > MenuOption.Resume)
                {
                    MenuStatus--;
                    //TODO: AudioManager.SelectMenuSound.Play();
                }
                else if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    HandleMenuSelection();
                }

                _previouskeyboardState = keyboardState;
            }
        }

        private bool IsKeyPressed(KeyboardState keyboardState, KeyboardState previouskeyboardState, Keys key)
        {
            return keyboardState.IsKeyDown(key) && previouskeyboardState.IsKeyUp(key);
        }

        private void HandleMenuSelection()
        {
            switch (MenuStatus)
            {
                case MenuOption.Resume:
                    // TODO: AudioManager.ResumeBackgroundMusic();
                    UIStatus = GameStatus.Playing;
                    break;

                case MenuOption.Restart:
                    // TODO: AudioManager.ResumeBackgroundMusic();
                    // TODO: Restart position, timer, score 
                    UIStatus = GameStatus.Playing;
                    break;

                case MenuOption.GodMode:
                    // TODO: God mode
                    break;

                case MenuOption.SelectStage:
                    // TODO: select stage
                    break;

                case MenuOption.Exit:
                    // TODO: select stage
                    UIStatus = GameStatus.Exit;
                    break;
            }
        }

        public void Draw()
        {
            if (UIStatus == GameStatus.Playing)
            {
                HUD.Draw(_graphicsDevice, _spriteBatch, _spriteFont, Timer, Score);
            }
            else if (UIStatus == GameStatus.Title)
            {
                TitleScreen.Draw(_graphicsDevice, _spriteBatch, _spriteFont);
            }
            else if (UIStatus == GameStatus.Menu)
            {
                Menu.Draw(_graphicsDevice, _spriteBatch, _spriteFont, MenuStatus);
            }
        }
    }
}
