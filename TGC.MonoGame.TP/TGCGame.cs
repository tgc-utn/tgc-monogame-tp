using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Helpers.Gizmos;
using TGC.MonoGame.TP.Maps;
using TGC.MonoGame.TP.Menu;
using TGC.MonoGame.TP.Types;
using TGC.MonoGame.TP.Utils.Models;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer mas ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        private GraphicsDeviceManager Graphics { get; }

        /* ESTO DEBERIA IR A LOS MAPAS */
        private Map Map { get; set; }
        private Camera Camera { get; set; } = null;
        private Gizmos Gizmos { get; set; }
        private GameState GameState { get; set; }
        private MainMenu Menu { get; set; }

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            Gizmos = new Gizmos();

            GameState = new GameState();
            Map = new PlaneMap(5, Tanks.T90, Tanks.T90V2, Graphics);

            Menu = new MainMenu(GraphicsDevice, GameState);

            base.Initialize();
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el procesamiento
        ///     que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.

            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.

            Menu.LoadContent(Content);
            Map.Load(GraphicsDevice, Content);
            Gizmos.LoadContent(GraphicsDevice, new ContentManager(Content.ServiceProvider, Content.RootDirectory));
            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
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

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            switch (GameState.CurrentStatus)
            {
                case GameStatus.MainMenu:
                    Menu.Draw(GameState.CurrentStatus);
                    break;
                case GameStatus.NormalGame:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    try
                    {
                        Map.Draw(Camera.View, Camera.Projection);
                    }
                    catch (Exception e)
                    {
                    }

                    break;
                case GameStatus.GodModeGame:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    try
                    {
                        Map.Draw(Camera.View, Camera.Projection);
                        DrawBoundingBoxesDebug();
                        Gizmos.Draw();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ERRORRRR");
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
            foreach (var prop in Map.Props)
                Gizmos.DrawCube((prop.Box.Max + prop.Box.Min) / 2f, prop.Box.Max - prop.Box.Min, Color.Red);
            // Gizmos.DrawCube(prop.World, Color.Red);
            foreach (var enemy in Map.Enemies)
                Gizmos.DrawCube(enemy.OBBWorld, Color.DeepPink);
            foreach (var ally in Map.Alies)
                Gizmos.DrawCube(ally.OBBWorld, Color.HotPink);
            Gizmos.DrawCube(Map.Player.OBBWorld, Color.Aqua);
            foreach (var bullet in Map.Player.Bullets)
                Gizmos.DrawSphere(bullet.Box.Center, bullet.Box.Radius * Vector3.One, Color.Aqua);
        }

        /// <summary>
        ///     Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();
            Gizmos.Dispose();
            base.UnloadContent();
        }
    }
}