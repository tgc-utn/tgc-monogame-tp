using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.Helpers.Gizmos;
using TGC.MonoGame.TP.Maps;
using TGC.MonoGame.TP.Menu;
using TGC.MonoGame.TP.Types;
using TGC.MonoGame.TP.Types.Tanks;
using TGC.MonoGame.TP.Utils.Models;

namespace TGC.MonoGame.TP
{
    public class TGCGame : Game
    {
        private GraphicsDeviceManager Graphics { get; }

        /* ESTO DEBERIA IR A LOS MAPAS */
        private Map Map { get; set; }
        private Camera Camera { get; set; } = null;
        private Gizmos Gizmos { get; set; }
        private GameState GameState { get; set; }
        private MainMenu Menu { get; set; }

        /* SOMBRAS */
        private const int ShadowmapSize = 2048;
        private readonly float LightCameraFarPlaneDistance = 3000f;
        private readonly float LightCameraNearPlaneDistance = 5f;
        private RenderTarget2D ShadowMapRenderTarget;
        private FullScreenQuad FullScreenQuad;
        private TargetCamera TargetLightCamera { get; set; }

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

            Menu = new MainMenu(GraphicsDevice, GameState);

            TargetLightCamera = new TargetCamera(1f, Map.SkyDome.LightPosition, Vector3.Zero);
            TargetLightCamera.BuildProjection(1f, LightCameraNearPlaneDistance, LightCameraFarPlaneDistance,
                MathHelper.PiOver2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Menu.LoadContent(Content);
            Map.Load(GraphicsDevice, Content);
            Gizmos.LoadContent(GraphicsDevice, new ContentManager(Content.ServiceProvider, Content.RootDirectory));

            FullScreenQuad = new FullScreenQuad(GraphicsDevice);
            ShadowMapRenderTarget = new RenderTarget2D(GraphicsDevice, ShadowmapSize, ShadowmapSize, false,
                SurfaceFormat.Single, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);
            GraphicsDevice.BlendState = BlendState.Opaque;

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            //Salgo del juego.
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();
            
            if (keyboardState.IsKeyDown(Keys.F1))
                GameState.Set(GameStatus.MainMenu);
            if (keyboardState.IsKeyDown(Keys.F2))
                GameState.Set(GameStatus.NormalGame);
            if (keyboardState.IsKeyDown(Keys.F3))
                GameState.Set(GameStatus.GodModeGame);

            switch (GameState.CurrentStatus)
            {
                case GameStatus.MainMenu:
                    Menu.Update();
                    break;
                case GameStatus.NormalGame:
                    if (GameState.FirstUpdate)
                    {
                        Menu.Dispose();
                        Camera = new TargetCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.One * 100f,
                            Vector3.Zero);
                        GameState.FirstUpdate = false;
                    }
                    Map.Update(gameTime);
                    TargetLightCamera.Position = Map.SkyDome.LightPosition;
                    TargetLightCamera.BuildView();
                    try
                    {
                        Camera.Update(gameTime, Map.Player);
                    }
                    catch (Exception e)
                    {
                    }
                    break;
                case GameStatus.GodModeGame:
                    if (GameState.FirstUpdate)
                    {
                        Menu.Dispose();
                        Camera = new DebugCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.UnitY * 20, 125f, 1f);
                        GameState.FirstUpdate = false;
                    }
                    Map.Update(gameTime);
                    TargetLightCamera.Position = Map.SkyDome.LightPosition;
                    TargetLightCamera.BuildView();
                    try
                    {
                        Camera.Update(gameTime, Map.Player);
                        Gizmos.UpdateViewProjection(Camera.View, Camera.Projection);
                    }
                    catch (Exception e)
                    {
                    }
                    break;
                case GameStatus.DeathMenu:
                    Menu.Update();
                    break;
                case GameStatus.Exit:
                    Exit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            switch (GameState.CurrentStatus)
            {
                case GameStatus.MainMenu:
                    Menu.Draw(GameState.CurrentStatus);
                    break;
                case GameStatus.NormalGame:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    try
                    {
                        Map.Draw(Camera, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
                    }
                    catch (Exception e)
                    {
                    }
                    break;
                case GameStatus.GodModeGame:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    try
                    {
                        Map.Draw(Camera, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
                        DrawBoundingBoxesDebug();
                        Gizmos.Draw();
                    }
                    catch (Exception e)
                    {
                    }
                    break;
                case GameStatus.DeathMenu:
                    Menu.Draw(GameState.CurrentStatus);
                    break;
                case GameStatus.Exit:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DrawBoundingBoxesDebug()
        {
            foreach (var prop in Map.Props.Where(prop => !prop.Destroyed).ToList())
                Gizmos.DrawCube((prop.Box.Max + prop.Box.Min) / 2f, prop.Box.Max - prop.Box.Min, Color.Red);
            // Gizmos.DrawCube(prop.World, Color.Red);
            foreach (var tank in Map.Tanks)
                Gizmos.DrawCube(tank.OBBWorld, tank.Action is PlayerActionTank ? Color.Aqua : 1 == tank.Action.Team ? Color.HotPink : Color.DeepPink);
            foreach (var bullet in Map.Player.Bullets)
                Gizmos.DrawSphere(bullet.Box.Center, bullet.Box.Radius * Vector3.One, Color.Aqua);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            Gizmos.Dispose();
            base.UnloadContent();
        }
    }
}