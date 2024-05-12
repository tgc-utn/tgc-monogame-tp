using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BepuPhysics.Trees;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Geometries;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer mas ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
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



            // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        private GraphicsDeviceManager Graphics { get; set; }
        private Random _random;
        private FollowCamera FollowCamera { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private Model Model { get; set; }
        private Car MainCar {get; set; }
        private GameModel Tree1 { get; set; }
        public GameModel Box1 { get; private set; }
        private GameModel Weapon1 { get; set; }
        private GameModel Vehicle { get; set; }
        private GameModel Gasoline { get; set; }
        private GameModel Ramp { get; set; }
        private GameModel CarDBZ { get; set; }
        private GameModel Car2 { get; set; }
        private GameModel Tower { get; set; }
        private GameModel Bush1 { get; set; }
        public GameModel Truck { get; private set; }
        public GameModel Fence1 { get; private set; }
        public GameModel Fence2 { get; private set; }
        public GameModel SceneCars { get; private set; }
        private GameModel Cottage { get; set; }
        private GameModel School { get; set; }
        private CubePrimitive Box { get; set; }
        private Effect Effect { get; set; }
        private Effect EffectNoTextures { get; set; }
        // private float XMovementPosition { get; set; }
        // private float ZMovementPosition { get; set; }


        // private List<Model> Models3d = new List<Model>();

        //Aceleración y frenado

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.



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

            MainCar = new Car(Vector3.Zero);

            Tree1 = new GameModel(Vector3.Zero, 60f);
            Box1 = new GameModel(Vector3.Zero);
            Tower = new GameModel(Vector3.Zero);
            Weapon1 = new GameModel(Vector3.Zero, 0.1f);
            Vehicle = new GameModel(Vector3.Zero, 0.3f);
            Ramp = new GameModel(Vector3.Zero, 4f);
            Gasoline = new GameModel(Vector3.Zero, 1.5f);
            CarDBZ = new GameModel(Vector3.Zero);
            Car2 = new GameModel(Vector3.Zero);
            Bush1 = new GameModel(Vector3.Zero);
            Truck = new GameModel(Vector3.Zero);
            Fence1 = new GameModel(Vector3.Zero);
            Fence2 = new GameModel(Vector3.Zero);
            SceneCars = new GameModel(Vector3.Zero);
            Cottage = new GameModel(new Vector3(-20, 0, -20));
            // School = new GameModel(new Vector3(20, 0, 20));

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
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            EffectNoTextures = Content.Load<Effect>(ContentFolderEffects + "BasicShaderNoTextures");

            // Cargo el modelo del auto.
            var CarModel = Content.Load<Model>(ContentFolder3D + "car/RacingCar");
            MainCar.Load(CarModel, Effect);

            var Tree1Model = Content.Load<Model>(ContentFolder3D + "trees/Tree2");
            Tree1.Load(Tree1Model, Effect);

            var Box1Model = Content.Load<Model>(ContentFolder3D + "Street/model/Electronic box");
            Box1.Load(Box1Model, Effect);

            var TowerModel = Content.Load<Model>(ContentFolder3D + "Street/model/towers");
            Tower.Load(TowerModel, Effect);

            var Weapon1Model = Content.Load<Model>(ContentFolder3D + "weapons/Weapons");
            Weapon1.Load(Weapon1Model, Effect);

            var VehicleModel = Content.Load<Model>(ContentFolder3D + "weapons/Vehicle");
            Vehicle.Load(VehicleModel, Effect);
            
            var RampModel = Content.Load<Model>(ContentFolder3D + "ramp/ramp");
            Ramp.Load(RampModel, Effect);

            var GasolineModel = Content.Load<Model>(ContentFolder3D + "gasoline/gasoline");
            Gasoline.Load(GasolineModel, Effect);

            var CarDBZModel = Content.Load<Model>(ContentFolder3D + "carDBZ/carDBZ");
            CarDBZ.Load(CarDBZModel, Effect);

            var Car2Model = Content.Load<Model>(ContentFolder3D + "car2/car2");
            Car2.Load(Car2Model, Effect);

            var Bush1Model = Content.Load<Model>(ContentFolder3D + "Bushes/source/bush1");
            Bush1.Load(Bush1Model, Effect);

            var TruckModel = Content.Load<Model>(ContentFolder3D + "Truck/source/KAMAZ");
            Truck.Load(TruckModel, Effect);

            var Fence1Model = Content.Load<Model>(ContentFolder3D + "Street/model/fence");
            Fence1.Load(Fence1Model, Effect);

            var Fence2Model = Content.Load<Model>(ContentFolder3D + "Street/model/fence2");
            Fence2.Load(Fence2Model, Effect);

            var SceneCarsModel = Content.Load<Model>(ContentFolder3D + "Street/model/WatercolorScene");
            SceneCars.Load(SceneCarsModel, Effect);
            
            var cottageModel = Content.Load<Model>(ContentFolder3D + "Street/model/House");
            Cottage.Load(cottageModel, Effect);

            // var schoolModel = Content.Load<Model>(ContentFolder3D + "Street/model/House");
            // School.Load(schoolModel, Effect);

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
            MainCar.Update(Keyboard.GetState(), gameTime);
            var CarWorld = MainCar.getWorld();

            // Actualizo la camara, enviandole la matriz de mundo del auto.
            FollowCamera.Update(gameTime, CarWorld);


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

            _random = new Random(SEED);

            DrawFloor(Box);

            MainCar.Draw();

            for (int i = 0; i < 100; i++)
            {
                Vector3 treeTranslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));
                Tree1.Draw(treeTranslation);
            }

            for (int i = 0; i < 100; i++)
            {
                Vector3 boxTraslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));
                Box1.Draw(boxTraslation);
            }

            for (int i = 0; i < 20; i++)
            {
                Vector3 armaTranslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));
                Weapon1.Draw();
            }

            Vehicle.Draw(new Vector3(30f, 0f, 30f));

            for (int i = 0; i < 15; i++)
            {
                Vector3 towerTraslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));
                Tower.Draw();
            }

            for (int i = 0; i < 30; i++)
            {
                Vector3 rampTranslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));
                Ramp.Draw(rampTranslation);
            }

            for (int i = 0; i < 100; i++)
            {
                Vector3 rampTranslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));
                Bush1.Draw();
            }

            Cottage.Draw();

            // School.Draw();

            for (int i = 0; i < 15; i++)
            {
                Vector3 gasolineTranslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));
                Gasoline.Draw(gasolineTranslation);
            }

            CarDBZ.Draw();

            // DrawModels(Models3d);
        }

        private void DrawModels(List<Model> models)
        {
            foreach (Model model in models)
            {
                for (int i = 0; i < 15; i++)
                {
                    Vector3 modelTraslation = new Vector3(_random.Next(-200, 200), 0, _random.Next(-450, 450));
                    foreach (var mesh in model.Meshes)
                    {
                        Effect.Parameters["DiffuseColor"].SetValue(Color.Yellow.ToVector3());
                        Effect.Parameters["World"].SetValue(mesh.ParentBone.ModelTransform * Matrix.CreateTranslation(modelTraslation));
                        mesh.Draw();
                    }
                }
            }


        }

        private void DrawFloor(GeometricPrimitive geometry)
        {
            EffectNoTextures.Parameters["DiffuseColor"].SetValue(Color.DarkSeaGreen.ToVector3());
            EffectNoTextures.Parameters["World"].SetValue(Matrix.CreateScale(new Vector3(1000, 2, 1000)) * Matrix.CreateTranslation(new Vector3(0, -1, 0)));
            geometry.Draw(EffectNoTextures);
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