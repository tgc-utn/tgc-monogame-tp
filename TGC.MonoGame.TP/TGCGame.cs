﻿namespace TGC.MonoGame.TP
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using System;
    using System.Collections.Generic;
    using TGC.MonoGame.TP.Components.Bullet;
    using TGC.MonoGame.TP.Components.Camera;
    using TGC.MonoGame.TP.Components.Enemy;
    using TGC.MonoGame.TP.Components.Map;
    using TGC.MonoGame.TP.Components.Player;
    using TGC.MonoGame.TP.Components.Spawner;

    /// <summary>
    /// Esta es la clase principal  del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        /// <summary>
        /// Defines the ContentFolder3D.
        /// </summary>
        public const string ContentFolder3D = "Models/";

        /// <summary>
        /// Defines the ContentFolderEffect.
        /// </summary>
        public const string ContentFolderEffect = "Effects/";

        /// <summary>
        /// Defines the ContentFolderMusic.
        /// </summary>
        public const string ContentFolderMusic = "Music/";

        /// <summary>
        /// Defines the ContentFolderSounds.
        /// </summary>
        public const string ContentFolderSounds = "Sounds/";

        /// <summary>
        /// Defines the ContentFolderSpriteFonts.
        /// </summary>
        public const string ContentFolderSpriteFonts = "SpriteFonts/";

        /// <summary>
        /// Defines the ContentFolderTextures.
        /// </summary>
        public const string ContentFolderTextures = "Textures/";

        /// <summary>
        /// Initializes a new instance of the <see cref="TGCGame"/> class.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);

            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = false;
        }

        /// <summary>
        /// Gets the Graphics.
        /// </summary>
        private GraphicsDeviceManager Graphics { get; }

        /// <summary>
        /// Gets or sets the SpriteBatch.
        /// </summary>
        private SpriteBatch SpriteBatch { get; set; }

        private Song Song { get; set; }

        /// <summary>
        /// Gets or sets the Column.
        /// </summary>
        private Model Column { get; set; }
        private Model SpawnerModel { get; set; }
        private Spawner Spawner { get; set; }
        private Model Skull { get; set; }
        private Model BulletModel { get; set; }
        private List<Bullet> Bullets { get; set; }
        private Floor Floor { get; set; }
        private Matrix QuadWorld { get; set; }

        /// <summary>
        /// Gets or sets the Shotgun.
        /// </summary>
        private Model Shotgun { get; set; }
        private float Recoil { get; set; }
        /// <summary>
        /// Gets or sets the World.
        /// </summary>

        private Matrix World { get; set; }

        /// <summary>
        /// Gets or sets the Camera.
        /// </summary>
        private FreeCamera Camera { get; set; }

        /// <summary>
        /// Gets or sets the GamePause.
        /// </summary>
        private Boolean GamePause { get; set; }
        private Boolean ClickPressed { get; set; }

        private Player Player { get; set; }

        private Enemy Enemy { get; set; }

        private List<Enemy> Enemies { get; set; }

        /// <summary>
        /// Gets or sets the Map.
        /// </summary>
        private Map Map { get; set; }

        /// <summary>
        /// Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: todo procesamiento que podemos pre calcular para nuestro juego.
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // Adapt screen size
            Graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            // Enable fullscreen 
            //Graphics.IsFullScreen = true;
            // Apply changes
            Graphics.ApplyChanges();

            ClickPressed = false;

            // Initialize Camera
            var screenSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.UnitZ * 350, screenSize);

            // Initialize player
            Player = new Player(Camera.Position);
            
            // Enable backface culling
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;

            // Create World matrix
            World = Matrix.CreateRotationY(MathHelper.Pi);

            Map = new Map();
            
            Bullets = new List<Bullet>();
            
            Enemy = new Enemy();
            Enemy.SetPosition(new Vector3(200, 13, 250));

            Enemies = new List<Enemy>();

            Spawner = new Spawner();
            Spawner.SetPosition(new Vector3(850, 0, 850));

            // Arranco el Game Pause en true para evitar que el jugador se mueva
            GamePause = true;

            // Configuramos nuestras matrices de la escena.
            World = Matrix.Identity;

            base.Initialize();
        }

        /// <summary>
        /// Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el
        ///     procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            var texture = Content.Load<Texture2D>(ContentFolderTextures + "ccreteflr016a_COLOR");
            
            Column = Content.Load<Model>(ContentFolder3D + "bonecolumn/bonecolumn");
            Map.LoadContent(Column,texture,GraphicsDevice);

            // Load bullet model
            BulletModel = Content.Load<Model>(ContentFolder3D + "bullet/Bullet_9x19");
            var modelEffectBullet = (BasicEffect)BulletModel.Meshes[0].Effects[0];
            modelEffectBullet.DiffuseColor = Color.White.ToVector3();
            modelEffectBullet.EnableDefaultLighting();

            // Load enemy model
            Skull = Content.Load<Model>(ContentFolder3D + "bullskulleyes/bullskulleyes");
            var modelEffectSkull = (BasicEffect)BulletModel.Meshes[0].Effects[0];
            modelEffectSkull.DiffuseColor = Color.White.ToVector3();
            modelEffectSkull.EnableDefaultLighting();

            // Load Spawner model
            SpawnerModel = Content.Load<Model>(ContentFolder3D + "monsterlarge/MonsterLarge");
            var modelEffectSpawner = (BasicEffect)SpawnerModel.Meshes[0].Effects[0];
            modelEffectSpawner.DiffuseColor = Color.White.ToVector3();
            modelEffectSpawner.EnableDefaultLighting();

            // Load shotgun model
            Shotgun = Content.Load<Model>(ContentFolder3D + "shotgun/shotgun");
            var modelEffect = (BasicEffect)Shotgun.Meshes[0].Effects[0];
            modelEffect.DiffuseColor = Color.White.ToVector3();
            modelEffect.EnableDefaultLighting();

            Song = Content.Load<Song>(ContentFolderMusic + "doom-ost-damnation");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Song);
            base.LoadContent();
        }

        /// <summary>
        /// Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        /// <param name="gameTime">The gameTime<see cref="GameTime"/>.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                // Exit game
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                // Pause/Start game
                GamePause = !GamePause;
            }

            Spawner.Update(this);

            // If GamePause is false i can move and play
            if (!GamePause)
            {
                Camera.Update(gameTime);
                Player.SetPosition(Camera.Position);
            
                // Creates new bullets when left click
                var mouse = Mouse.GetState();

                if (mouse.LeftButton == ButtonState.Pressed && !ClickPressed && Bullets.Count < 15)
                {
                    Bullet singleBullet = new Bullet();
                    singleBullet.SetPosition(Camera.Position + Camera.FrontDirection * 50f);
                    singleBullet.SetDirection(Camera.FrontDirection);
                    singleBullet.SetUp(Camera.UpDirection);
                    Bullets.Add(singleBullet);
                    ClickPressed = true;
                }

                if (mouse.RightButton == ButtonState.Pressed && !ClickPressed && Bullets.Count < 15)
                {
                    Bullet Bullet1 = new Bullet();
                    Bullet1.SetPosition(Camera.Position + Camera.FrontDirection * 190f);
                    Bullet1.SetDirection(Camera.FrontDirection);
                    Bullet1.SetUp(Camera.UpDirection);
                    Bullets.Add(Bullet1);

                    Bullet Bullet2 = new Bullet();
                    Bullet2.SetPosition(Camera.Position + Vector3.Up * 5 + Camera.FrontDirection * 50f);
                    Bullet2.SetDirection(Camera.FrontDirection);
                    Bullet2.SetUp(Camera.UpDirection);
                    Bullets.Add(Bullet2);

                    ClickPressed = true;
                }

                if (mouse.LeftButton == ButtonState.Released && mouse.RightButton == ButtonState.Released && ClickPressed) ClickPressed = false;

                if (Bullets.Count >= 15)
                {
                    Bullets.Clear();
                }

                foreach (Bullet bullet in Bullets)
                {
                    bullet.Update();

                    if(Vector3.Distance(bullet.GetPosition(), Enemy.GetPosition())<50 && !bullet.GetDidDamage())
                    {
                        Enemy.TakeDamage(bullet.GetDamage());
                        bullet.DoDamage();
                    }
                }

                Vector3 EnemyPosition = Enemy.GetPosition();
                Enemy.SetUp(Camera.UpDirection);
                Enemy.SetDirection(Vector3.Normalize(new Vector3(EnemyPosition.X - Camera.Position.X, 0, EnemyPosition.Z - Camera.Position.Z)));
                Enemy.Update(Camera.Position);

                foreach (Enemy enemy in Enemies)
                {
                    Vector3 enemyPos = enemy.GetPosition();
                    enemy.SetUp(Camera.UpDirection);
                    enemy.SetDirection(Vector3.Normalize(new Vector3(enemyPos.X - Camera.Position.X, 0, enemyPos.Z - Camera.Position.Z)));
                    enemy.Update(Camera.Position);

                    foreach (Bullet bullet in Bullets)
                    {
                        bullet.Update();

                        if (Vector3.Distance(bullet.GetPosition(), enemy.GetPosition()) < 50 && !bullet.GetDidDamage())
                        {
                            enemy.TakeDamage(bullet.GetDamage());
                            bullet.DoDamage();
                        }
                    }
                }

                Player.SetPosition(Camera.Position);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        /// </summary>
        /// <param name="gameTime">The gameTime<see cref="GameTime"/>.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (Recoil > 0)
            {
                Recoil -= 0.25f;
            }

            Vector3 cameraRight = Vector3.Cross(Camera.FrontDirection, Camera.UpDirection);
            Vector3 weaponPosition = new Vector3(Camera.Position.X, 0, Camera.Position.Z) + new Vector3(0, -15, 0) + Camera.FrontDirection * MathHelper.Lerp(40, 35, Recoil) + cameraRight * 10 - Camera.UpDirection * 4;

            var mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                Recoil = 10;
            }
            if (mouse.RightButton == ButtonState.Pressed)
            {
                Recoil = 13.5f;
            }

            Map.Draw(World, Camera.View, Camera.Projection);
            SpawnerModel.Draw(World * Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalSeconds) * Matrix.CreateTranslation(Spawner.GetPosition().X, 13 * MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds) + 50, Spawner.GetPosition().Z), Camera.View, Camera.Projection);

            if (Enemy.GetLife() > 0)
            {
                Vector3 SkullRight = Vector3.Cross(Enemy.GetDirection(), Enemy.GetUp());
                Vector3 SkullPosition = Enemy.GetPosition() + Enemy.GetDirection() + SkullRight - Enemy.GetUp();
                Skull.Draw(Matrix.CreateScale(0.75f) * Matrix.CreateWorld(SkullPosition, -SkullRight, Enemy.GetUp()), Camera.View, Camera.Projection);
            }

            foreach (Enemy enemy in Enemies)
            {
                if (enemy.GetLife() > 0)
                {
                    Vector3 SkullRight = Vector3.Cross(enemy.GetDirection(), enemy.GetUp());
                    Vector3 SkullPosition = enemy.GetPosition() + enemy.GetDirection() + SkullRight - enemy.GetUp();
                    Skull.Draw(Matrix.CreateScale(0.75f) * Matrix.CreateWorld(SkullPosition, -SkullRight, enemy.GetUp()), Camera.View, Camera.Projection);
                }
            }
            
            foreach(Bullet bullet in Bullets)
            {
                Vector3 BulletRight = Vector3.Cross(bullet.GetDirection(), bullet.GetUp());
                Vector3 BulletPosition = bullet.GetPosition() + bullet.GetDirection()+ BulletRight - bullet.GetUp();
                if (Vector3.Distance(bullet.GetPosition(), Camera.Position) < 2550) BulletModel.Draw(Matrix.CreateScale(5) * Matrix.CreateWorld(BulletPosition, -BulletRight, bullet.GetDirection()), Camera.View, Camera.Projection);
            }            

            Matrix shotgunWorld = Matrix.CreateScale(0.1f, 0.1f, 0.1f) * Matrix.CreateWorld(weaponPosition,-cameraRight,Camera.UpDirection);
            Shotgun.Draw(shotgunWorld, Camera.View, Camera.Projection);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();

            base.UnloadContent();
        }

        public void AddEnemy(Vector3 enemyPosition)
        {
            Enemy enemy = new Enemy();
            enemy.SetPosition(enemyPosition);

            Enemies.Add(enemy);
        }
    }
}
