using Chinchulines.Menu.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Chinchulines.Menu.States
{
    public class MenuScreen : Screen
    {
        private Texture2D backGround;
        private Texture2D Title;
        private Texture2D BGInstructions;

        private List<Button> _ButtonsMainMenu;

        private Texture2D buttonTexture;
        private SpriteFont buttonFont;

        Button newGameButton;
        Button godGameButton;
        Button manualGameButton;
        Button quitGameButton;
        Button goBackButton;

        public MenuScreen(Game game, GraphicsDeviceManager graphics, ContentManager content)
          : base(game, graphics, content)
        {

            backGround = _content.Load<Texture2D>("Background/Background");
            Title = _content.Load<Texture2D>("Background/Titulo");
            BGInstructions = _content.Load<Texture2D>("Background/BackgroundInstruccions");

            buttonTexture = _content.Load<Texture2D>("Controls/Button");
            buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            newGameButton = new Button(buttonTexture, buttonFont, true)
            {
                Position = new Vector2(300, 250),
                Text = "Jugar",
            };

            newGameButton.Click += NewGameButton_Click;

            godGameButton = new Button(buttonTexture, buttonFont, true)
            {
                Position = new Vector2(300, 300),
                Text = "Modo Dios",
            };

            godGameButton.Click += GodGameButton_Click;

            manualGameButton = new Button(buttonTexture, buttonFont, true)
            {
                Position = new Vector2(300, 350),
                Text = "Instrucciones",
            };

            manualGameButton.Click += ManualGameButton_Click;

            quitGameButton = new Button(buttonTexture, buttonFont, true)
            {
                Position = new Vector2(300, 400),
                Text = "Salir",
            };

            quitGameButton.Click += QuitGameButton_Click;

            goBackButton = new Button(buttonTexture, buttonFont, false)
            {
                Position = new Vector2(600, 410),
                Text = "Go Back",
            };

            goBackButton.Click += goBackButton_Click;

            _ButtonsMainMenu = new List<Button>()
            {
                newGameButton,
                godGameButton,
                manualGameButton,
                quitGameButton,
                goBackButton,
            };
        }

        private void goBackButton_Click(object sender, EventArgs e)
        {
            foreach (Button button in _ButtonsMainMenu) button.visible = !button.visible;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(backGround, new Vector2(0, 0), Color.White);

            if(goBackButton.visible)
            {
                spriteBatch.Draw(BGInstructions, Vector2.Zero, Color.White);
                spriteBatch.DrawString(buttonFont, $"Movimiento de la nave:\n" +
                            $"\n" +
                            $"      W: Rotar hacia arriba\n" +
                            $"      A: Rotar hacia la izquierda\n" +
                            $"      S: Rotar hacia abajo\n" +
                            $"      D: Rotar hacia la derecha\n" +
                            $"\n" +
                            $"Barrel Roll:\n" +
                            $"\n" +
                            $"      Q: Hacia izquierda\n" +
                            $"      E: Hacia derecha\n" +
                            $"\n" +
                            $"Giro hacia atras: X", new Vector2(300,75), Color.White);
            }
            else
            {

                spriteBatch.Draw(Title, new Vector2(150, 0), Color.White);
                spriteBatch.Draw(backGround, new Vector2(0, 0), Color.White);
            }

            foreach (Button button in _ButtonsMainMenu)
                button.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void ManualGameButton_Click(object sender, EventArgs e)
        {
            foreach(Button button in _ButtonsMainMenu)button.visible = !button.visible;
        }

        private void GodGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("GOD mode");
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            using (var game = new ChinchuGame())
                game.Run();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var button in _ButtonsMainMenu)
                button.Update(gameTime);
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
    }
}
