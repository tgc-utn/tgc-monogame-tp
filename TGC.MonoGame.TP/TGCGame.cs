using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.Helpers.Gizmos;
using TGC.MonoGame.TP.HUD;
using TGC.MonoGame.TP.Maps;
using TGC.MonoGame.TP.Menu;
using TGC.MonoGame.TP.Types;
using TGC.MonoGame.TP.Types.Tanks;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.Utils.Models;

namespace TGC.MonoGame.TP
{
    public class TGCGame : Game
    {
        private GraphicsDeviceManager Graphics { get; }

        /* ESTO DEBERIA IR A LOS MAPAS */
        private Map Map { get; set; }
        private Map MenuMap { get; set; }
        private Camera PlayerCamera { get; set; } = null;
        private Gizmos Gizmos { get; set; }
        private GameState GameState { get; set; }
        private MainMenu Menu { get; set; }
        private float TimeSinceLastChange { get; set; }
        private Song Song { get; set; }
        private ScreenTime ScreenTime { get; set; }
        private Score Score { get; set; }

        /* SOMBRAS */
        private readonly float LightCameraFarPlaneDistance = 3000f;
        private readonly float LightCameraNearPlaneDistance = 5f;
        private RenderTarget2D ShadowMapRenderTarget;
        private TargetCamera TargetLightCamera { get; set; }

        /* OPTIMIZACION */
        private BoundingFrustum BoundingFrustum { get; set; }
        private TargetCamera OptimizationCamera { get; set; }
        
        /* DEBUG */
        private SpriteFont Font;
        private SpriteBatch SpriteBatch;
        private Camera DebugCamera { get; set; } = null;
        private bool showFPS = false;

        public TGCGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Gizmos = new Gizmos();

            GameState = new GameState();
            Map = new PlaneMap(5, Tanks.T90, Tanks.T90V2, Graphics);
            MenuMap = new MenuMap(Tanks.T90, Graphics);
            Menu = new MainMenu(GraphicsDevice, GameState, MenuMap);

            TargetLightCamera = new TargetCamera(1f, Map.SkyDome.LightPosition, Vector3.Zero, new Vector2(20f, 5f));
            TargetLightCamera.BuildProjection(1f, LightCameraNearPlaneDistance, LightCameraFarPlaneDistance,
                MathHelper.PiOver2);
            
            PlayerCamera = new TargetCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.One * 100f,
                Vector3.Zero, new Vector2(20f, 5f));
            
            DebugCamera = new DebugCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.UnitY * 20, 125f,
                1f);
            
            OptimizationCamera = new TargetCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.One * 100f,
                Vector3.Zero, new Vector2(100f, 25f));
            
            BoundingFrustum = new BoundingFrustum(PlayerCamera.View * PlayerCamera.Projection);

            TimeSinceLastChange = 0f;
            ScreenTime = new ScreenTime(GraphicsDevice, 180f);
            Score = new Score(GraphicsDevice);
            
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Menu.LoadContent(GraphicsDevice, Content);
            Map.Load(GraphicsDevice, Content);
            Gizmos.LoadContent(GraphicsDevice, new ContentManager(Content.ServiceProvider, Content.RootDirectory));
            ScreenTime.LoadContent(Content);
            Score.LoadContent(Content);
            Song = Content.Load<Song>($"{ContentFolder.Music}/world-of-tanks");
            Font = Content.Load<SpriteFont>($"{ContentFolder.Fonts}/Verdana16");
            ShadowMapRenderTarget = new RenderTarget2D(GraphicsDevice, TexturesRepository.ShadowmapSize, TexturesRepository.ShadowmapSize, false,
                SurfaceFormat.Single, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);
            GraphicsDevice.BlendState = BlendState.Opaque;
        
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            //Salgo del juego.
            if (keyboardState.IsKeyDown(Keys.Escape) && GameState.CurrentStatus != GameStatus.MainMenu &&
                GameState.CurrentStatus != GameStatus.Exit)
            {
                GameState.Set(GameStatus.MainMenu);
                TimeSinceLastChange = 0f;
            }

            if (keyboardState.IsKeyDown(Keys.F9))
            {
                GameState.Set(GameStatus.WinMenu);
                TimeSinceLastChange = 0f;
            }

            if (keyboardState.IsKeyDown(Keys.F10))
            {
                GameState.Set(GameStatus.DeathMenu);
                TimeSinceLastChange = 0f;
            }
            
            if (keyboardState.IsKeyDown(Keys.F12))
            {
                showFPS = !showFPS;
            }

            // Musica
            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Volume = 0.05f;
                MediaPlayer.Play(Song);
            }

            if (keyboardState.IsKeyDown(Keys.F1))
                GameState.Set(GameStatus.MainMenu);
            if (keyboardState.IsKeyDown(Keys.F2))
                GameState.Set(GameStatus.NormalGame);
            if (keyboardState.IsKeyDown(Keys.F3))
                GameState.Set(GameStatus.GodModeGame);

            switch (GameState.CurrentStatus)
            {
                case GameStatus.MainMenu:
                    Menu.Update(gameTime);
                    if (keyboardState.IsKeyDown(Keys.Escape) && TimeSinceLastChange > 0.5f)
                        GameState.Set(GameStatus.Exit);
                    break;
                case GameStatus.NormalGame:
                    if (GameState.FirstUpdate)
                    {
                        Menu.Dispose();
                        GameState.FirstUpdate = false;
                    }
                    ScreenTime.Update(gameTime);
                    Score.Update(100000f + (float)gameTime.ElapsedGameTime.TotalSeconds);
                    Map.Update(gameTime);
                    TargetLightCamera.Position = Map.SkyDome.LightPosition;
                    TargetLightCamera.BuildView();
                    PlayerCamera.Update(gameTime, Map.Player);
                    OptimizationCamera.Update(gameTime, Map.Player);
                    BoundingFrustum.Matrix = OptimizationCamera.View * OptimizationCamera.Projection;
                    break;
                case GameStatus.GodModeGame:
                    if (GameState.FirstUpdate)
                    {
                        Menu.Dispose();
                        GameState.FirstUpdate = false;
                        showFPS = true;
                    }
                    Map.Update(gameTime);
                    TargetLightCamera.Position = Map.SkyDome.LightPosition;
                    TargetLightCamera.BuildView();
                    DebugCamera.Update(gameTime, Map.Player);
                    PlayerCamera.Update(gameTime, Map.Player);
                    OptimizationCamera.Update(gameTime, Map.Player);
                    BoundingFrustum.Matrix = OptimizationCamera.View * OptimizationCamera.Projection;
                    Gizmos.UpdateViewProjection(DebugCamera.View, DebugCamera.Projection);
                    break;
                case GameStatus.WinMenu:
                case GameStatus.DeathMenu:
                    Menu.Update(gameTime);
                    break;
                case GameStatus.Exit:
                    Exit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            TimeSinceLastChange += (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            switch (GameState.CurrentStatus)
            {
                case GameStatus.MainMenu:
                    Menu.Draw(GameState.CurrentStatus, ShadowMapRenderTarget, TargetLightCamera, BoundingFrustum);
                    break;
                case GameStatus.NormalGame:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    Map.Draw(PlayerCamera, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera, BoundingFrustum);
                    ScreenTime.Draw();
                    Score.Draw();
                    break;
                case GameStatus.GodModeGame:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    Map.Draw(DebugCamera, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera, BoundingFrustum);
                    DrawBoundingBoxesDebug();
                    Gizmos.Draw();
                    break;
                case GameStatus.WinMenu:
                case GameStatus.DeathMenu:
                    Menu.Draw(GameState.CurrentStatus, ShadowMapRenderTarget, TargetLightCamera, BoundingFrustum);
                    break;
                case GameStatus.Exit:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (showFPS)
                DrawFPS(gameTime);
        }

        private void DrawBoundingBoxesDebug()
        {
            foreach (var prop in Map.Props.Where(prop => !prop.Destroyed).ToList())
                Gizmos.DrawCube((prop.Box.Max + prop.Box.Min) / 2f, prop.Box.Max - prop.Box.Min, Color.Red);
            // Gizmos.DrawCube(prop.World, Color.Red);
            foreach (var tank in Map.Tanks)
                Gizmos.DrawCube(tank.OBBWorld,
                    tank.Action is PlayerActionTank ? Color.Aqua :
                    tank.Action.isEnemy ? Color.HotPink : Color.DeepPink);
            foreach (var bullet in Map.Player.Bullets)
                Gizmos.DrawSphere(bullet.Box.Center, bullet.Box.Radius * Vector3.One, Color.Aqua);
            Gizmos.DrawFrustum(PlayerCamera.View * PlayerCamera.Projection, Color.Yellow);
            Gizmos.DrawFrustum(TargetLightCamera.View * TargetLightCamera.Projection, Color.White);
            Gizmos.DrawFrustum(OptimizationCamera.View * OptimizationCamera.Projection, Color.Green);
            
            /* LIMITES DEL MAPA */
            foreach (var limit in Map.Limits)
                Gizmos.DrawCube((limit.Max + limit.Min) / 2f, limit.Max - limit.Min, Color.Blue);
        }
        
        private void DrawFPS(GameTime gameTime)
        {
            var fps = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            var fpsString = $"FPS: {fps:0}";
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Font, fpsString, new Vector2( 10, 10), Color.Black, 0f, Vector2.Zero, 1f,
                SpriteEffects.None, 0);
            SpriteBatch.End();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            Gizmos.Dispose();
            base.UnloadContent();
        }
    }
}