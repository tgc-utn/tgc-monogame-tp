﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Types;
using TGC.MonoGame.TP.Utils.Fonts;

namespace TGC.MonoGame.TP.Menu;

public class MainMenu
{
    private ButtonsGrid Buttons;
    private Texture2D _logo;
    private int _screenWidth;
    private int _screenHeight;

    // Background
    private Map _menuMap;
    private Camera _camera;
    
    private SpriteFont Font;
    public SpriteBatch SpriteBatch { get; set; }
    public GraphicsDevice GraphicsDevice { get; }

    public MainMenu (GraphicsDevice graphicsDevice, GameState gameState, Map menuMap)
        {
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(graphicsDevice);
            
            _screenWidth = graphicsDevice.Viewport.Width;
            _screenHeight = graphicsDevice.Viewport.Height;
            
            _menuMap = menuMap;
            _camera = new AngularCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0f,50f,10f), _menuMap.Player.Position, (float)Math.PI/4);
            
            var buttons = new List<Button>
            {
                new ("Nuevo Juego", GameStatus.NormalGame),
                new ("Modo Dios", GameStatus.GodModeGame),
                new ("Salir", GameStatus.Exit),
            };
            Buttons = new ButtonsGrid(gameState, _screenWidth/2, _screenHeight/2, buttons);
        }
    
        public void LoadContent(GraphicsDevice graphicsDevice,ContentManager content)
        {
            _logo = content.Load<Texture2D>(Utils.Textures.Menu.MenuImage.Path);
            Font = content.Load<SpriteFont>(Fonts.Arial.Path);
            _menuMap.Load(graphicsDevice, content);
            Buttons.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            _menuMap.Update(gameTime);
            Buttons.Update(Mouse.GetState());
        }
        
        public void Draw(GameStatus gameStatus, RenderTarget2D ShadowMapRenderTarget, Camera TargetLightCamera)
        {
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            try
            {
                _menuMap.Draw(_camera, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
            }
            catch (Exception e) { }
            GraphicsDevice.DepthStencilState = DepthStencilState.Default; 
            SpriteBatch.Begin();
            var destRectangle = new Rectangle((_screenWidth - _logo.Width*2/3)/2,
                _screenHeight/50, _logo.Width*2/3, _logo.Height*2/3);
            SpriteBatch.Draw(_logo, destRectangle, Color.White);
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

        public void Dispose()
        {
            SpriteBatch.Dispose();
        }
}