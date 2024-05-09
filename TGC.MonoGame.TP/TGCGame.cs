using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
        public Matrix CarWorld { get; private set; }
        private SpriteBatch SpriteBatch { get; set; }
        private Model Model { get; set; }
        private Model Tree1 { get; set; }
        public Model Box1 { get; private set; }
        private Model Weapon1 { get; set; }
        private Model Vehicle { get; set; }
        private Model Gasoline { get; set; }
        private Model Ramp { get; set; }
        private Model CarDBZ { get; set; }
        private Model Car2 { get; set; }
        private Model Tower { get; set; }
        private Model Bush1 { get; set; }
        public Model Truck { get; private set; }
        public Model Fence1 { get; private set; }
        public Model Fence2 { get; private set; }
        public Model SceneCars { get; private set; }
        private Edificio Cottage { get; set; }
        private Edificio School { get; set; }
        private CubePrimitive Box { get; set; }
        private Effect Effect { get; set; }
        private float Rotation { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private float XMovementPosition { get; set; }
        private float ZMovementPosition { get; set; }

        private Vector3 CarPosition = Vector3.Zero;

        private float CarVelocity { get; set; }

        private float CarRotation { get; set; }

        private List<Model> Models3d = new List<Model>();

        //Aceleración y frenado
        float acceleration = 3f;
        float frictionCoefficient = 0.5f;
        float maxVelocity = 0.7f;
        float minVelocity = -0.7f;
        float stopCar = 0f;

        //Rotacion
        float carRotatingVelocity = 2.3f;

        //Salto
        float jumpSpeed = 100f;
        float gravity = 10f;
        float carMass = 3.8f;
        float carInFloor = 0f;

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

            // Configuro la matriz de mundo del auto.
            CarWorld = Matrix.Identity;

            // Seria hasta aca.

            Box = new CubePrimitive(GraphicsDevice, 1, Color.DarkSeaGreen);

            //// Configuramos nuestras matrices de la escena.
            //XMovementPosition = 0f;
            //ZMovementPosition = 0f;
            //World = Matrix.Identity;
            //View = Matrix.CreateLookAt(new Vector3(-ViewDistance + XMovementPosition, ViewDistance, -ViewDistance + Offset + ZMovementPosition), LookAtVector, Vector3.Up);
            //Projection =
            //    Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 1000);

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

            // Cargo el modelo del logo.
            Model = Content.Load<Model>(ContentFolder3D + "car/RacingCar");
            Tree1 = Content.Load<Model>(ContentFolder3D + "trees/Tree2");
            Box1 = Content.Load<Model>(ContentFolder3D + "Street/model/Electronic box");
            Tower = Content.Load<Model>(ContentFolder3D + "Street/model/towers");
            Weapon1 = Content.Load<Model>(ContentFolder3D + "weapons/Weapons");
            Vehicle = Content.Load<Model>(ContentFolder3D + "weapons/Vehicle");
            Ramp = Content.Load<Model>(ContentFolder3D + "ramp/ramp");
            Gasoline = Content.Load<Model>(ContentFolder3D + "gasoline/gasoline");
            CarDBZ = Content.Load<Model>(ContentFolder3D + "carDBZ/carDBZ");
            Car2 = Content.Load<Model>(ContentFolder3D + "car2/car2");
            Ramp = Content.Load<Model>(ContentFolder3D + "Street/model/ramp");
            Bush1 = Content.Load<Model>(ContentFolder3D + "Bushes/source/bush1");
            Truck = Content.Load<Model>(ContentFolder3D + "Truck/source/KAMAZ");
            Fence1 = Content.Load<Model>(ContentFolder3D + "Street/model/fence");
            Fence2 = Content.Load<Model>(ContentFolder3D + "Street/model/fence2");
            SceneCars = Content.Load<Model>(ContentFolder3D + "Street/model/WatercolorScene");


            Cottage = new Edificio(new Vector3(-20, 0, -20));
            var cottageModel = Content.Load<Model>(ContentFolder3D + "Street/model/House");
            Cottage.Load(cottageModel, Effect);

            School = new Edificio(new Vector3(20, 0, 20));
            var schoolModel = Content.Load<Model>(ContentFolder3D + "Street/model/House");
            School.Load(schoolModel, Effect);

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.
            LoadEffect(Model);
            LoadEffect(Tree1);
            LoadEffect(Box1);
            LoadEffect(Weapon1);
            LoadEffect(Vehicle);
            LoadEffect(Ramp);
            LoadEffect(Gasoline);
            LoadEffect(CarDBZ);
            LoadEffect(Car2);
            LoadEffect(Tower);
            LoadEffect(Ramp);
            LoadEffect(Bush1);
            LoadEffect(Truck);
            LoadEffect(Fence1);
            LoadEffect(Fence2);
            LoadEffect(SceneCars);

            Models3d.Add(Truck);
            Models3d.Add(Fence1);
            Models3d.Add(Fence2);
            Models3d.Add(SceneCars);

            base.LoadContent();
        }

        private void LoadEffect(Model model)
        {
            foreach (var mesh in model.Meshes)
            {
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
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
            //Move(gameTime);

            // Capto el estado del teclado.
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                // Salgo del juego.
                Exit();
            }

            MovePrincipalCarFollowCamara(gameTime);


            //View = Matrix.CreateLookAt(new Vector3(-ViewDistance + XMovementPosition, ViewDistance, -ViewDistance + Offset + ZMovementPosition), LookAtVector, Vector3.Up);
            base.Update(gameTime);
        }

        private void MovePrincipalCarFollowCamara(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                //Mantener apretado para saltar 
                CarPosition.Y += jumpSpeed * deltaTime;
            }
            //Fuerza de gravedad 
            CarPosition.Y -= carMass * gravity * deltaTime;

            if (CarPosition.Y <= carInFloor)
            {
                // Reiniciar la posición vertical del coche
                CarPosition.Y = carInFloor;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                // Acelerar hacia adelante en la dirección del coche
                CarVelocity += (acceleration) * deltaTime;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                // Acelerar hacia atrás en la dirección opuesta del coche
                CarVelocity += (-acceleration) * deltaTime;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                // Rotar hacia la izquierda
                CarRotation += (carRotatingVelocity) * deltaTime;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                // Rotar hacia la Derecha
                CarRotation += (-carRotatingVelocity) * deltaTime;

            }

            // Frenado gradual por fricción
            if (CarVelocity != stopCar)
            {
                // Calcular la dirección opuesta a la velocidad actual
                float direction = Math.Sign(CarVelocity);

                // Calcular la cantidad de frenado basada en la velocidad actual y el coeficiente de fricción
                float friction = frictionCoefficient * direction * deltaTime;

                // Aplicar el frenado por fricción
                CarVelocity -= friction;

                // Asegurar que la velocidad no se vuelva negativa
                if (Math.Sign(CarVelocity) != direction)
                {
                    CarVelocity = stopCar;
                }
            }



            // Actualizar la posición del coche en función de su velocidad, rotacion y ultima posicion 
            CarPosition = CarPosition + Vector3.Transform(Vector3.Backward, Matrix.CreateRotationY(CarRotation)) * CarVelocity;

            // Limitar la velocidad máxima y mínima
            CarVelocity = MathHelper.Clamp(CarVelocity, minVelocity, maxVelocity);

            // Actualizar la matriz de transformación del coche
            CarWorld = Matrix.CreateRotationY(CarRotation) * Matrix.CreateTranslation(CarPosition);

            // Actualizo la camara, enviandole la matriz de mundo del auto.
            FollowCamera.Update(gameTime, CarWorld);
        }

        private void Move(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                ZMovementPosition += View.Forward.Z * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
                XMovementPosition += View.Forward.Z * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
                LookAtVector.Z += View.Forward.Z * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
                LookAtVector.X += View.Forward.Z * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                ZMovementPosition -= View.Forward.Z * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
                XMovementPosition -= View.Forward.Z * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
                LookAtVector.Z -= View.Forward.Z * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
                LookAtVector.X -= View.Forward.Z * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                ZMovementPosition -= View.Forward.X * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
                XMovementPosition -= View.Forward.Z * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
                LookAtVector.Z -= View.Forward.X * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
                LookAtVector.X -= View.Forward.Z * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                ZMovementPosition += View.Forward.X * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
                XMovementPosition += View.Forward.Z * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
                LookAtVector.Z += View.Forward.X * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
                LookAtVector.X += View.Forward.Z * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
            }
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

            _random = new Random(SEED);

            DrawFloor(Box);
            DrawCar();
            DrawTrees();
            DrawBox();
            DrawWeapon1();
            DrawVehicle();
            DrawTowers();
            DrawRamps();
            DrawBushes();
            Cottage.Draw();
            School.Draw();
            DrawRamp();
            DrawGasoline();
            DrawCarDBZ();
            DrawModels(Models3d);
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

        private void DrawCar()
        {
            foreach (var mesh in Model.Meshes)
            {
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkBlue.ToVector3());
                Effect.Parameters["World"].SetValue(CarWorld);
                mesh.Draw();
            }
        }
        private void DrawCarDBZ()
        {
            foreach (var mesh in CarDBZ.Meshes)
            {
                Effect.Parameters["DiffuseColor"].SetValue(Color.BlueViolet.ToVector3());
                Effect.Parameters["World"].SetValue(mesh.ParentBone.ModelTransform * Matrix.CreateTranslation(new Vector3(40, 0, 40)));
                mesh.Draw();
            }
        }

        private void DrawBox()
        {
            for (int i = 0; i < 100; i++)
            {
                Vector3 boxTraslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));
                foreach (var mesh in Box1.Meshes)
                {
                    Effect.Parameters["DiffuseColor"].SetValue(Color.DarkGreen.ToVector3());
                    Effect.Parameters["World"].SetValue(Matrix.CreateScale(new Vector3(1, 1, 1)) * mesh.ParentBone.ModelTransform * Matrix.CreateTranslation(boxTraslation));
                    mesh.Draw();
                }
            }
        }

        private void DrawTowers()
        {
            for (int i = 0; i < 15; i++)
            {
                Vector3 towerTraslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));
                foreach (var mesh in Tower.Meshes)
                {
                    Effect.Parameters["DiffuseColor"].SetValue(Color.Yellow.ToVector3());
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.ModelTransform * Matrix.CreateTranslation(towerTraslation));
                    mesh.Draw();
                }
            }
        }
        private void DrawRamps()
        {
            for (int i = 0; i < 15; i++)
            {
                Vector3 rampTranslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));
                var randomRotation = Convert.ToSingle(_random.NextDouble() * 2.0 * Math.PI);
                foreach (var mesh in Ramp.Meshes)
                {
                    Effect.Parameters["DiffuseColor"].SetValue(Color.Pink.ToVector3());
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.ModelTransform * Matrix.CreateScale(4f) * Matrix.CreateRotationY(randomRotation) * Matrix.CreateTranslation(rampTranslation));
                    mesh.Draw();
                }
            }
        }
        private void DrawBushes()
        {
            for (int i = 0; i < 100; i++)
            {
                Vector3 rampTranslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));
                foreach (var mesh in Bush1.Meshes)
                {
                    Effect.Parameters["DiffuseColor"].SetValue(Color.Pink.ToVector3());
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.ModelTransform * Matrix.CreateTranslation(rampTranslation));
                    mesh.Draw();
                }
            }
        }

        private void DrawTrees()
        {
            for (int i = 0; i < 100; i++)
            {
                Vector3 treeTranslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));
                foreach (var mesh in Tree1.Meshes)
                {
                    Effect.Parameters["DiffuseColor"].SetValue(Color.DarkBlue.ToVector3());
                    Effect.Parameters["World"].SetValue(Matrix.CreateScale(new Vector3(60, 60, 60)) * mesh.ParentBone.ModelTransform * Matrix.CreateTranslation(treeTranslation));
                    mesh.Draw();
                }
            }
        }

        private void DrawWeapon1()
        {
            for (int i = 0; i < 20; i++)
            {
                Vector3 armaTranslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));
                foreach (var mesh in Weapon1.Meshes)
                {
                    Effect.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());
                    Effect.Parameters["World"].SetValue(Matrix.CreateScale(new Vector3(0.1f, 0.1f, 0.1f)) * mesh.ParentBone.ModelTransform * Matrix.CreateTranslation(armaTranslation));
                    mesh.Draw();
                }
            }
        }

        private void DrawVehicle()
        {

            Vector3 vehicleTranslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));

            foreach (var mesh in Vehicle.Meshes)
            {
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkCyan.ToVector3());
                Effect.Parameters["World"].SetValue(Matrix.CreateScale(new Vector3(0.03f, 0.03f, 0.03f)) * mesh.ParentBone.ModelTransform * Matrix.CreateTranslation(vehicleTranslation));
                mesh.Draw();
            }
        }
        private void DrawRamp()
        {
            for (int i = 0; i < 25; i++)
            {
                Vector3 rampTranslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));
                foreach (var mesh in Ramp.Meshes)
                {
                    Effect.Parameters["DiffuseColor"].SetValue(Color.Black.ToVector3());
                    Effect.Parameters["World"].SetValue(Matrix.CreateScale(new Vector3(2f, 2f, 2f)) * mesh.ParentBone.ModelTransform * Matrix.CreateTranslation(rampTranslation));
                    mesh.Draw();
                }
            }
        }
        private void DrawGasoline()
        {
            for (int i = 0; i < 15; i++)
            {
                Vector3 gasolineTranslation = new Vector3(_random.Next(-450, 450), 0, _random.Next(-450, 450));
                foreach (var mesh in Gasoline.Meshes)
                {
                    Effect.Parameters["DiffuseColor"].SetValue(Color.Brown.ToVector3());
                    Effect.Parameters["World"].SetValue(Matrix.CreateScale(new Vector3(1.5f, 1.5f, 1.5f)) * mesh.ParentBone.ModelTransform * Matrix.CreateTranslation(gasolineTranslation));
                    mesh.Draw();
                }
            }
        }

        private void DrawFloor(GeometricPrimitive geometry)
        {
            Effect.Parameters["DiffuseColor"].SetValue(Color.DarkSeaGreen.ToVector3());
            Effect.Parameters["World"].SetValue(Matrix.CreateScale(new Vector3(1000, 2, 1000)) * Matrix.CreateTranslation(new Vector3(0, -1, 0)));
            geometry.Draw(Effect);
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