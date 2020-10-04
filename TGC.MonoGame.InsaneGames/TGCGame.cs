using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;
using TGC.MonoGame.InsaneGames.Maps;
using TGC.MonoGame.InsaneGames.Entities;
using TGC.MonoGame.InsaneGames.Weapons;
using TGC.MonoGame.InsaneGames.Collectibles;
using TGC.MonoGame.InsaneGames.Obstacles;
using TGC.MonoGame.InsaneGames.Utils;
using System;
using System.Linq;

namespace TGC.MonoGame.InsaneGames
{
    /// <summary>
    ///     Esta es la clase principal  del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            // Descomentar para que el juego sea pantalla completa.
            //Graphics.IsFullScreen = true;
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";

            ContentManager.MakeInstance(Content);
        }

        private GraphicsDeviceManager Graphics { get; }
        public Camera Camera { get; private set; }

        private Map Map { get; set; }

        private Weapon Weapon { get; set; }
        private SpriteBatch spriteBatch;
        private SpriteFont font;


        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: todo procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            // Apago el backface culling.
            // Esto se hace por un problema en el diseno del modelo del logo de la materia.
            // Una vez que empiecen su juego, esto no es mas necesario y lo pueden sacar.
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;
            // Seria hasta aca.

            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0, 20, 60), Point.Zero);

            Map = CreateMap();
            Map.Initialize(this);
            Weapon = new MachineGun();
            Weapon.Initialize(this);
            base.Initialize();
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el
        ///     procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            Map.Load();
            Weapon.Load();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = ContentManager.Instance.LoadSpriteFont("Basic");

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();

            Camera.Update(gameTime);

            Map.Update(gameTime);
            Weapon.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;

            Map.Draw(gameTime);
            Weapon.Draw(gameTime);


            Vector2 lifeStringPosition = new Vector2(10, GraphicsDevice.Viewport.Height - 50);
            Vector2 armorStringPosition = new Vector2(170, GraphicsDevice.Viewport.Height - 50);

            spriteBatch.Begin();
            // TODO: Take life and armor from player
            spriteBatch.DrawString(font, "LIFE: 100", lifeStringPosition, Color.Red);
            spriteBatch.DrawString(font, "ARMOR: 100", armorStringPosition, Color.DarkBlue);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        ///     Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();

            base.UnloadContent();
        }

        //Temporal
        private Map CreateMap()
        {
            var wallsEffect = new BasicEffect(GraphicsDevice);
            wallsEffect.TextureEnabled = true;
            wallsEffect.Texture = ContentManager.Instance.LoadTexture2D("Wall/Wall2");
            var floorEffect = new BasicEffect(GraphicsDevice);
            floorEffect.TextureEnabled = true;
            floorEffect.Texture = ContentManager.Instance.LoadTexture2D("Checked-Floor/Checked-Floor");
            var ceilingEffect = new BasicEffect(GraphicsDevice);
            ceilingEffect.TextureEnabled = true;
            ceilingEffect.Texture = ContentManager.Instance.LoadTexture2D("ceiling/Ceiling");
            var dict1 = new Dictionary<WallId, BasicEffect> { { WallId.Ceiling, ceilingEffect }, { WallId.Floor, floorEffect }, { WallId.Right, wallsEffect }, { WallId.Back, wallsEffect } };
            var dict2 = new Dictionary<WallId, BasicEffect> { { WallId.Ceiling, ceilingEffect }, { WallId.Floor, floorEffect }, { WallId.Right, wallsEffect }, { WallId.Front, wallsEffect } };
            var dict3 = new Dictionary<WallId, BasicEffect> { { WallId.Ceiling, ceilingEffect }, { WallId.Floor, floorEffect }, { WallId.Left, wallsEffect }, { WallId.Front, wallsEffect } };
            var dict4 = new Dictionary<WallId, BasicEffect> { { WallId.Ceiling, ceilingEffect }, { WallId.Floor, floorEffect }, { WallId.Left, wallsEffect }, { WallId.Back, wallsEffect } };
            var textRepet = new Dictionary<WallId, (float, float)> { { WallId.Front, (2, 1) } };
            var box1 = new Box(dict1, new Vector3(500, 100, 500), new Vector3(0, 50, 0));
            var box2 = new Box(dict2, new Vector3(500, 100, 500), new Vector3(0, 50, -500), textureRepeats: textRepet);
            var box3 = new Box(dict3, new Vector3(500, 100, 500), new Vector3(-500, 50, -500));
            var box4 = new Box(dict4, new Vector3(500, 100, 500), new Vector3(-500, 50, 0));
            var TGCito = new TGCito(Matrix.CreateTranslation(25, 0, 25));
            var life = new Life(Matrix.CreateTranslation(50, 0, -100));
            var armor = new Armor(Matrix.CreateTranslation(25, 8, -100));

            List<Obstacle> obstacles = CreateObstacles();

            return new Map(new Room[] { box1, box2, box3, box4 }, new Enemy[] { TGCito }, new Collectible[] { life, armor }, obstacles.ToArray());
        }

        private List<Obstacle> CreateObstacles()
        {
            List<Obstacle> obstacles = new List<Obstacle>();

            var boxesObstacles = ObstaclesBuilder.ObtainBoxesObstaclesInLine(6, Matrix.CreateTranslation(-50, 0, -300), true);
            var barriers = ObstaclesBuilder.ObtainBarriersObstaclesInLine(4, Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(-250, 0, -500), false);
            var sawhorses = ObstaclesBuilder.ObtainSawhorsesObstaclesInLine(4, Matrix.CreateTranslation(-300, 0, 0), true);
            var cones = ObstaclesBuilder.ObtainConesObstaclesInLine(6, Matrix.CreateTranslation(0, 0, -200), true);

            obstacles = obstacles.Union(boxesObstacles).ToList();
            obstacles = obstacles.Union(barriers).ToList();
            obstacles = obstacles.Union(sawhorses).ToList();
            obstacles = obstacles.Union(cones).ToList();

            return obstacles;
        }
    }
}