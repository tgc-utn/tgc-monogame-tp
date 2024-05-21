using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BepuPhysics;
using BepuPhysics.Constraints;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Viewer.Gizmos;
using TGC.MonoGame.Samples.Physics.Bepu;
using TGC.MonoGame.TP.Geometries;
using NumericVector3 = System.Numerics.Vector3;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer mas ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        /// <summary>
        ///     Gets the simulation created by the demo's Initialize call.
        /// </summary>
        public Simulation Simulation { get; protected set; }

        //Note that the buffer pool used by the simulation is not considered to be *owned* by the simulation. The simulation merely uses the pool.
        //Disposing the simulation will not dispose or clear the buffer pool.
        /// <summary>
        ///     Gets the buffer pool used by the demo's simulation.
        /// </summary>
        public BufferPool BufferPool { get; private set; }

        /// <summary>
        ///     Gets the thread dispatcher available for use by the simulation.
        /// </summary>
        public SimpleThreadDispatcher ThreadDispatcher { get; private set; }
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";
        public const float ViewDistance = 20f;
        public const float Offset = 10f;
        private const int SEED = 0;
        public const float CameraSpeed = 50f;
        public Vector3 LookAtVector = new Vector3(0, 0, Offset);

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);

            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;

            Gizmos = new Gizmos();


            // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }
        public Gizmos Gizmos { get; set; }
        private GraphicsDeviceManager Graphics { get; set; }
        private Random _random;
        private FollowCamera FollowCamera { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private Model Model { get; set; }
        private Car MainCar {get; set; }
        private GameModel TreeModel { get; set; }
        private List<StaticObject> Trees = new List<StaticObject>();
        public GameModel BoxModel { get; private set; }
        public List<StaticObject> Boxes = new List<StaticObject>();
        private GameModel WeaponModel { get; set; }
        private List<StaticObject> Weapons = new List<StaticObject>();
        private GameModel VehicleModel { get; set; }
        private StaticObject Vehicle { get; set; }
        private GameModel GasolineModel { get; set; }
        private List<StaticObject> Gasolines = new List<StaticObject>();
        private GameModel RampModel { get; set; }
        private List<StaticObject> Ramps = new List<StaticObject>();
        private GameModel CarDBZModel { get; set; }
        private StaticObject CarDBZ { get; set; }
        private GameModel Car2Model { get; set; }
        private StaticObject Car2 { get; set; }
        private GameModel TowerModel { get; set; }
        private List<StaticObject> Towers = new List<StaticObject>();
        private GameModel BushModel { get; set; }
        private List<StaticObject> Bushes = new List<StaticObject>();
        public GameModel TruckModel { get; private set; }
        private StaticObject Truck { get; set; }
        public GameModel Fence1Model { get; private set; }
        private List<StaticObject> Fences1 = new List<StaticObject>();
        public GameModel Fence2Model { get; private set; }
        private List<StaticObject> Fences2 = new List<StaticObject>();
        public GameModel SceneCarsModel { get; private set; }
        private GameModel CottageModel { get; set; }
        private StaticObject Cottage { get; set; }
        private CubePrimitive Box { get; set; }
        private List<Matrix> WallWorlds = new List<Matrix>();
        private Effect Effect { get; set; }
        private Effect EffectNoTextures { get; set; }
        private int ArenaWidth = 200;
        private int ArenaHeight = 200;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            BufferPool = new BufferPool();
            var targetThreadCount = Math.Max(1,
                Environment.ProcessorCount > 4 ? Environment.ProcessorCount - 2 : Environment.ProcessorCount - 1);
            ThreadDispatcher = new SimpleThreadDispatcher(targetThreadCount);


            _random = new Random(SEED);


            // Apago el backface culling.
            // Esto se hace por un problema en el diseno del modelo del logo de la materia.
            // Una vez que empiecen su juego, esto no es mas necesario y lo pueden sacar.
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            // Creo una camara para seguir a nuestro auto.
            FollowCamera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);

            // Seria hasta aca.

            Box = new CubePrimitive(GraphicsDevice, 1, Color.DarkSeaGreen);

            WallWorlds.Add(Matrix.CreateRotationY(0f) * Matrix.CreateTranslation(200f, 0f, 0f));
            WallWorlds.Add(Matrix.CreateRotationY(0f) * Matrix.CreateTranslation(-200f, 0f, 0f));
            WallWorlds.Add(Matrix.CreateRotationY(Convert.ToSingle(Math.PI / 2)) * Matrix.CreateTranslation(0f, 0f, 200f));
            WallWorlds.Add(Matrix.CreateRotationY(Convert.ToSingle(Math.PI / 2)) * Matrix.CreateTranslation(0f, 0f, -200f));

            MainCar = new Car(Vector3.Zero);

            Vehicle = new StaticObject(new Vector3(30f, 0f, 30f));
            CarDBZ = new StaticObject(new Vector3(-30f, 0f, 30f));
            Car2 = new StaticObject(new Vector3(-60f, 0f, 60f));
            Truck = new StaticObject(new Vector3(60f, 0f, -60f));
            Cottage = new StaticObject(new Vector3(-20, 0, -20));


            for (int i = 0; i < 100; i++)
            {
                Vector3 treeTranslation = new Vector3(_random.Next(-ArenaWidth, ArenaWidth), 0, _random.Next(-ArenaHeight, ArenaHeight));
                Trees.Add(new StaticObject(treeTranslation));
            }

            for (int i = 0; i < 100; i++)
            {
                Vector3 boxTraslation = new Vector3(_random.Next(-ArenaWidth, ArenaWidth), 0, _random.Next(-ArenaHeight, ArenaHeight));
                Boxes.Add(new StaticObject(boxTraslation));
            }

            for (int i = 0; i < 15; i++)
            {
                Vector3 towerTraslation = new Vector3(_random.Next(-ArenaWidth, ArenaWidth), 0, _random.Next(-ArenaHeight, ArenaHeight));
                Towers.Add(new StaticObject(towerTraslation));
            }

            for (int i = 0; i < 30; i++)
            {
                Vector3 rampTranslation = new Vector3(_random.Next(-ArenaWidth, ArenaWidth), 0, _random.Next(-ArenaHeight, ArenaHeight));
                Ramps.Add(new StaticObject(rampTranslation));
            }

            for (int i = 0; i < 15; i++)
            {
                Vector3 gasolineTranslation = new Vector3(_random.Next(-ArenaWidth, ArenaWidth), 0, _random.Next(-ArenaHeight, ArenaHeight));
                Gasolines.Add(new StaticObject(gasolineTranslation));
            }

            for (int i = 0; i < 100; i++)
            {
                Vector3 bushTranslation = new Vector3(_random.Next(-ArenaWidth, ArenaWidth), 0, _random.Next(-ArenaHeight, ArenaHeight));
                Bushes.Add(new StaticObject(bushTranslation));
            }
            for (int i = 0; i < 20; i++)
            {
                Vector3 armaTranslation = new Vector3(_random.Next(-ArenaWidth, ArenaWidth), 0, _random.Next(-ArenaHeight, ArenaHeight));
                Weapons.Add(new StaticObject(armaTranslation));
            }

            base.Initialize();
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el procesamiento
        ///     que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            Simulation = Simulation.Create(BufferPool, new NarrowPhaseCallbacks(new SpringSettings(30, 1)),
                new PoseIntegratorCallbacks(new NumericVector3(0, -10, 0)), new SolveDescription(8, 1));


            // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Gizmos.LoadContent(GraphicsDevice, Content);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            EffectNoTextures = Content.Load<Effect>(ContentFolderEffects + "BasicShaderNoTextures");

            var CarModel = Content.Load<Model>(ContentFolder3D + "car/RacingCar");
            MainCar.Load(CarModel, Effect);
            MainCar.LoadPhysics(Simulation);


            // Cargo el modelo del auto.
            TreeModel = new GameModel(Content.Load<Model>(ContentFolder3D + "trees/Tree2"), Effect, 60f);
            BoxModel = new GameModel(Content.Load<Model>(ContentFolder3D + "Street/model/Electronic box"), Effect);
            TowerModel = new GameModel(Content.Load<Model>(ContentFolder3D + "Street/model/towers"), Effect);
            WeaponModel = new GameModel(Content.Load<Model>(ContentFolder3D + "weapons/Weapons"), Effect, 0.1f);
            VehicleModel = new GameModel(Content.Load<Model>(ContentFolder3D + "weapons/Vehicle"), Effect, 0.1f);
            RampModel = new GameModel(Content.Load<Model>(ContentFolder3D + "ramp/ramp"), Effect, 4f);
            GasolineModel = new GameModel(Content.Load<Model>(ContentFolder3D + "gasoline/gasoline"), Effect, 1.5f);
            CarDBZModel = new GameModel(Content.Load<Model>(ContentFolder3D + "carDBZ/carDBZ"), Effect);
            Car2Model = new GameModel(Content.Load<Model>(ContentFolder3D + "car2/car2"), Effect);
            BushModel = new GameModel(Content.Load<Model>(ContentFolder3D + "Bushes/source/bush1"), Effect);
            TruckModel = new GameModel(Content.Load<Model>(ContentFolder3D + "Truck/source/KAMAZ"), Effect);
            Fence1Model = new GameModel(Content.Load<Model>(ContentFolder3D + "Street/model/fence"), Effect);
            Fence2Model = new GameModel(Content.Load<Model>(ContentFolder3D + "Street/model/fence2"), Effect);
            SceneCarsModel = new GameModel(Content.Load<Model>(ContentFolder3D + "Street/model/WatercolorScene"), Effect);
            CottageModel = new GameModel(Content.Load<Model>(ContentFolder3D + "Street/model/House"), Effect);

            Vehicle.setModel(VehicleModel);
            CarDBZ.setModel(CarDBZModel);
            Car2.setModel(Car2Model);
            Truck.setModel(TruckModel);
            Cottage.setModel(CottageModel);
            Vector3 cottagePos = Cottage.getPosition();
            var cottageBox = new Box(17.5f, 10f, 17.5f);
            var cottageDescription = new StaticDescription(
            new System.Numerics.Vector3(cottagePos.X, cottagePos.Y, cottagePos.Z), 
            Simulation.Shapes.Add(cottageBox));
            Simulation.Statics.Add(cottageDescription);

            foreach (var tree in Trees)
            {
                tree.setModel(TreeModel);
                Vector3 position = tree.getPosition();
                var box1 = new Sphere(1.5f);
                var boxDescription = new StaticDescription(
                new System.Numerics.Vector3(position.X, position.Y, position.Z), 
                Simulation.Shapes.Add(box1));
                Simulation.Statics.Add(boxDescription);

            }
            foreach (var box in Boxes)
            {
                box.setModel(BoxModel);
                Vector3 position = box.getPosition();
                var box1 = new Box(3, 3f, 2);
                var boxDescription = new StaticDescription(
                    new System.Numerics.Vector3(position.X, position.Y, position.Z), 
                    Simulation.Shapes.Add(box1));
                Simulation.Statics.Add(boxDescription);

            }
            foreach (var tower in Towers)
             {
                tower.setModel(TowerModel);
                Vector3 position = tower.getPosition();
                var box1 = new Box(5, 10f, 4);
                var boxDescription = new StaticDescription(
                new System.Numerics.Vector3(position.X, position.Y, position.Z), 
                Simulation.Shapes.Add(box1));
                Simulation.Statics.Add(boxDescription);
            }
            foreach (var ramp in Ramps) {
                ramp.setModel(RampModel);
                var vertices = new NumericVector3[] {
                    // Bottom vertices
                    new NumericVector3(0f, 0.0f, 0f),
                    new NumericVector3(4.5f, 0.0f, 0f),
                    new NumericVector3(4.5f, 0.0f, 9f),
                    new NumericVector3(0f, 0.0f, 9f),

                    // Top vertices
                    new NumericVector3(0f, 0.5f, 1f),
                    new NumericVector3(4.5f, 0.5f, 1f),
                    new NumericVector3(0f, 1.2f, 2.5f),
                    new NumericVector3(4.5f, 1.2f, 2.5f),
                    new NumericVector3(0f, 2.0f, 4.5f),
                    new NumericVector3(4.5f, 2.0f, 4.5f),
                    new NumericVector3(4.5f, 3f, 9f),
                    new NumericVector3(0f, 3f, 9f)
                };
                NumericVector3 center;
                var convexHullShape  = new ConvexHull(vertices, Simulation.BufferPool, out center);
                var rampPosition = ramp.getPosition();
                var rampDescription = new StaticDescription(
                    new NumericVector3(rampPosition.X + 2f, rampPosition.Y, rampPosition.Z + 4f),
                    Simulation.Shapes.Add(convexHullShape)
                );
                Simulation.Statics.Add(rampDescription);
            }
            foreach (var gasoline in Gasolines)
            {
                gasoline.setModel(GasolineModel);
                Vector3 position = gasoline.getPosition();
                var box1 = new Box(2, 3f, 2);
                var boxDescription = new StaticDescription(
                new System.Numerics.Vector3(position.X, position.Y, position.Z), 
                Simulation.Shapes.Add(box1));
                Simulation.Statics.Add(boxDescription);

            }
            foreach (var bush in Bushes)
                bush.setModel(BushModel);
            foreach (var weapon in Weapons)
            {
                weapon.setModel(WeaponModel);
                Vector3 position = weapon.getPosition();
                var box1 = new Box(2.5f, 2f, 2.5f);
                var boxDescription = new StaticDescription(
                new System.Numerics.Vector3(position.X, position.Y, position.Z), 
                Simulation.Shapes.Add(box1));
                Simulation.Statics.Add(boxDescription);
            }


            var planeShape = new Box(2500f, 1f, 2500f);
            var planeDescription = new StaticDescription(
                new System.Numerics.Vector3(0, -0.5f, 0), 
                Simulation.Shapes.Add(planeShape)
            );
            Simulation.Statics.Add(planeDescription);

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.
            Simulation.Timestep(1 / 60f, ThreadDispatcher);

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                Exit();
            }

            // Capto el estado del teclado.
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                // Salgo del juego.
                Exit();
            }

            // Actualizar estado del auto
            MainCar.Update(Keyboard.GetState(), gameTime, Simulation);
            var CarWorld = MainCar.getWorld();

            // Actualizo la camara, enviandole la matriz de mundo del auto.
            FollowCamera.Update(gameTime, CarWorld);
            Gizmos.UpdateViewProjection(FollowCamera.View, FollowCamera.Projection);


            base.Update(gameTime);
        }



        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.Beige);

            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
            Effect.Parameters["View"].SetValue(FollowCamera.View);
            Effect.Parameters["Projection"].SetValue(FollowCamera.Projection);
            EffectNoTextures.Parameters["View"].SetValue(FollowCamera.View);
            EffectNoTextures.Parameters["Projection"].SetValue(FollowCamera.Projection);


            DrawFloor(Box);
            DrawWalls();

            MainCar.Draw(GraphicsDevice);

            foreach (var tree in Trees)
                tree.Draw();

            foreach (var box in Boxes)
                box.Draw();

            foreach (var weapon in Weapons)
                weapon.Draw();

            Vehicle.Draw();

            foreach (var tower in Towers)
                tower.Draw();

            foreach (var ramp in Ramps)
                ramp.Draw();

            foreach (var bush in Bushes)
                bush.Draw();

            Cottage.Draw();

            foreach (var gasoline in Gasolines)
                gasoline.Draw();

            CarDBZ.Draw();

        }


        private void DrawFloor(GeometricPrimitive geometry)
        {
            EffectNoTextures.Parameters["DiffuseColor"].SetValue(Color.DarkSeaGreen.ToVector3());
            EffectNoTextures.Parameters["World"].SetValue(Matrix.CreateScale(new Vector3(1000, 2, 1000)) * Matrix.CreateTranslation(new Vector3(0, -1, 0)));
            geometry.Draw(EffectNoTextures);
        }

        private void DrawWalls() {
            var prim = new BoxPrimitive(GraphicsDevice, new Vector3(1f, 10f, 200f), Color.HotPink);
            foreach (var wall in WallWorlds) {
                EffectNoTextures.Parameters["DiffuseColor"].SetValue(Color.HotPink.ToVector3());
                EffectNoTextures.Parameters["World"].SetValue(wall);
                prim.Draw(EffectNoTextures);
            }
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
    }
}