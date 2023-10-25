using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Types;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Menu;

public class MainMenu
{
    private ButtonsGrid Buttons;
    private Texture2D Logo;
    private int _screenWidth;
    private int _screenHeight;

    // Background
    private Camera _camera;
    private Camera _environmentCamera;
    private SpriteFont Font;
    public SpriteBatch SpriteBatch { get; set; }
    public GraphicsDevice GraphicsDevice { get; set; }
    
    public bool loaded = false;

    public MainMenu (GraphicsDevice graphicsDevice, GameState gameState)
        {
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(graphicsDevice);
            loaded = true;
            
            _screenWidth = graphicsDevice.Viewport.Width;
            _screenHeight = graphicsDevice.Viewport.Height;

            var buttons = new List<Button>
            {
                new ("Comenzar", GameStatus.NormalGame),
                new ("Modo Dios", GameStatus.GodModeGame),
                new ("Salir", GameStatus.Exit),
            };
            Buttons = new ButtonsGrid(gameState, _screenWidth/2, _screenHeight/2, buttons);
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el procesamiento
        ///     que podemos pre calcular para nuestro juego.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            Logo = content.Load<Texture2D>(Utils.Textures.Menu.MenuImage.Path);
            Font = content.Load<SpriteFont>(Utils.Fonts.Fonts.Arial.Path);
            Buttons.LoadContent(content);
        }

        public void Update()
        {
            if (!loaded) 
                SpriteBatch = new SpriteBatch(GraphicsDevice);
            Buttons.Update(Mouse.GetState());
        }
        
        public void Draw(GameStatus gameStatus)
        {
            if (loaded)
            {
                GraphicsDevice.Clear(Color.Black);
                var destRectangle = new Rectangle(0, 0, Logo.Width, Logo.Height);
                SpriteBatch.Begin();
                SpriteBatch.Draw(Logo, Vector2.Zero, Color.White);
                if (gameStatus == GameStatus.DeathMenu)
                {
                    var text = "Perdiste!";
                    var size = Font.MeasureString(text);
                    SpriteBatch.DrawString(Font, text, new Vector2((_screenWidth - size.Y)/2, 20), Color.Red);
                }
                SpriteBatch.End();
                Buttons.Draw(SpriteBatch);
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
        }

        public void Dispose()
        {
            loaded = false;
            
            // SpriteBatch.Dispose();
        }
}
