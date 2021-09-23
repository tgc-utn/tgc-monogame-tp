using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;
using System.Collections.Generic;
using TGC.MonoGame.TP.Quads;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal  del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
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

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            // Descomentar para que el juego sea pantalla completa.
            // Graphics.IsFullScreen = true;
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }

        //Modelos
        private Model Cartel { get; set; }
        private Model Esfera { get; set; }
        private Model TunnelChico { get; set; }
        private Model Cubo { get; set; }
        private Model Platform { get; set; }
        private Model Flag { get; set; }
        private Model Pinches { get; set; }
        private Model Wings { get; set; }
        private Model Moneda { get; set; }


        private Effect Effect { get; set; }
        public Effect TextureEffect { get; set; }
        public Effect LavaEffect { get; set; }
        private Texture2D MarbleTexture { get; set; }
        private Texture2D CoinTexture { get; set; }
        private Texture2D SpikesTexture { get; set; }
        private Texture2D LavaTexture { get; set; }
        private Texture2D MagmaTexture { get; set; }
        private Texture2D CartelTexture { get; set; }
        private Texture2D WoodTexture { get; set; }
        private Texture2D StoneTexture { get; set; }
        private Texture2D MetalTexture { get; set; }
        public Texture2D FlagCheckeredTexture { get; set; }
        public Texture2D FlagCheckpointTexture { get; set; }
        public Texture2D BluePlatformTexture { get; set; }
        public Texture2D BluePlatformBasicTexture { get; set; }
        public Texture2D RedPlatformTexture { get; set; }
        public Texture2D RedPlatformBasicTexture { get; set; }
        public Texture2D GreenPlatformTexture { get; set; }
        public Texture2D GreenPlatformBasicTexture { get; set; }
        public Texture2D BluePlaceholderTexture { get; set; }
        public Texture2D WhitePlaceholderTexture { get; set; }
        private float Rotation { get; set; }
        public bool OnGround { get; private set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private FollowCamera FollowCamera { get; set; }
        public Quad quad { get; set; }

        private BoundingBox platformCollider;

        public Vector3 MarblePosition { get; private set; }
        public Matrix MarbleWorld { get; private set; }
        public Vector3 MarbleVelocity { get; private set; }
        public Vector3 MarbleAcceleration { get; private set; }

        private BoundingSphere MarbleSphere;

        public VertexDeclaration vertexDeclaration { get; set; }
        public Matrix MarbleScale { get; private set; }

        private float Gravity = 100f;
        private float JumpSpeed = 50f;
        private float x = -50f;
        private float y = -10f;
        private float z = 0f;
        
        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.
            // Seria hasta aca.
            OnGround = false;

            // Configuramos nuestras matrices de la escena.
            World = Matrix.Identity;
            //configuro pantalla
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);
            var screenSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            FollowCamera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);

            float yPositionFloor = -20f;
            float xScaleFloor = 400f;
            float zScaleFloor = 400f;

            quad = new Quad(new Vector3(0f, yPositionFloor, 0f), Vector3.Up, Vector3.Forward, xScaleFloor, zScaleFloor);

            platformCollider = new BoundingBox(new Vector3(-25f, -21f, -15f), new Vector3(5f, -16f, 15f));

            MarblePosition = new Vector3(-10f, -10f, 0f);
            MarbleVelocity = Vector3.Zero;
            MarbleAcceleration = Vector3.Down * Gravity;
            MarbleSphere = new BoundingSphere(MarblePosition, 2f);
            MarbleScale = Matrix.CreateScale(0.02f);

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

            // Cargo el Cartel
            Cartel = Content.Load<Model>(ContentFolder3D + "Marbel/Sign/StreetSign");
            //Cargo la esfera
            Esfera = Content.Load<Model>(ContentFolder3D + "Marbel/Pelota/pelota");
            //cargo tunel
            TunnelChico = Content.Load<Model>(ContentFolder3D + "Marbel/TunelChico/TunnelChico");
            //cargo Cubo
            Cubo = Content.Load<Model>(ContentFolder3D + "Marbel/Cubo/cubo");
            //cargo Bandera
            Flag = Content.Load<Model>(ContentFolder3D + "Marbel/Cubo/flag");
            //cargo Plataforma
            Platform = Content.Load<Model>(ContentFolder3D + "Marbel/Cubo/platform");
            //cargo pinches
            Pinches = Content.Load<Model>(ContentFolder3D + "Marbel/Pinches/Pinches");
            //cargo wings
            Wings = Content.Load<Model>(ContentFolder3D + "Marbel/Wings/Wings");
            //cargo moneda
            Moneda = Content.Load<Model>(ContentFolder3D + "Marbel/Moneda/Coin");

            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            TextureEffect = Content.Load<Effect>(ContentFolderEffects + "TextureShader");

            LavaEffect = Content.Load<Effect>(ContentFolderEffects + "LavaShader");

            MarbleTexture = Content.Load<Texture2D>(ContentFolderTextures + "marble");
            CoinTexture = Content.Load<Texture2D>(ContentFolderTextures + "Coin");
            SpikesTexture = Content.Load<Texture2D>(ContentFolderTextures + "Spikes");
            LavaTexture = Content.Load<Texture2D>(ContentFolderTextures + "Lava");
            MagmaTexture = Content.Load<Texture2D>(ContentFolderTextures + "Rock");
            CartelTexture = Content.Load<Texture2D>(ContentFolderTextures + "Sign");
            WoodTexture = Content.Load<Texture2D>(ContentFolderTextures + "caja-madera-2");
            StoneTexture = Content.Load<Texture2D>(ContentFolderTextures + "stones");
            MetalTexture = Content.Load<Texture2D>(ContentFolderTextures + "metal");
            FlagCheckeredTexture = Content.Load<Texture2D>(ContentFolderTextures + "CheckeredFlag");
            FlagCheckpointTexture = Content.Load<Texture2D>(ContentFolderTextures + "CheckpointFlag");
            BluePlatformTexture = Content.Load<Texture2D>(ContentFolderTextures + "platformBlue");
            BluePlatformBasicTexture = Content.Load<Texture2D>(ContentFolderTextures + "platformBlueNoStar");
            RedPlatformTexture = Content.Load<Texture2D>(ContentFolderTextures + "platformRed");
            RedPlatformBasicTexture = Content.Load<Texture2D>(ContentFolderTextures + "platformRedNoStar");
            GreenPlatformTexture = Content.Load<Texture2D>(ContentFolderTextures + "platformGreen");
            GreenPlatformBasicTexture = Content.Load<Texture2D>(ContentFolderTextures + "platformGreenNoStar");
            BluePlaceholderTexture = Content.Load<Texture2D>(ContentFolderTextures + "Blue");
            WhitePlaceholderTexture = Content.Load<Texture2D>(ContentFolderTextures + "White");

            LavaEffect.Parameters["Texture"].SetValue(LavaTexture);
            LavaEffect.Parameters["tiling"].SetValue(new Vector2(4f, 4f));

            TextureEffect.Parameters["Texture"].SetValue(BluePlaceholderTexture);

            vertexDeclaration = new VertexDeclaration(new VertexElement[]
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
            });

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.
            // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
            //mesh Cartel
            foreach (var mesh in Cartel.Meshes)            
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = TextureEffect;
            //mesh Cubo
            foreach (var mesh in Cubo.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = TextureEffect;
            //mesh bandera
            foreach (var mesh in Flag.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = TextureEffect;
            //mesh platform
            foreach (var mesh in Platform.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = TextureEffect;
            //mesh esfera
            foreach (var mesh in Esfera.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = TextureEffect;
            //mesh tunel
            foreach (var mesh in TunnelChico.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;
            //mesh pinches
            foreach (var mesh in Pinches.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = TextureEffect;
            //mesh wings
            foreach (var mesh in Wings.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;
            //mesh moneda
            foreach (var mesh in Moneda.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = TextureEffect;

            MarbleWorld = MarbleScale * Matrix.CreateTranslation(MarblePosition);

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            var deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Aca deberiamos poner toda la logica de actualizacion del juego.
            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();
            // Basado en el tiempo que paso se va generando una rotacion.
            Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Check for the Jump key press, and add velocity in Y only if the Robot is on the ground
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && OnGround)
                MarbleVelocity += Vector3.Up * JumpSpeed;

            MarbleVelocity += MarbleAcceleration * deltaTime;

            // Scale the velocity by deltaTime
            var scaledVelocity = MarbleVelocity * deltaTime;

            // Solve the Vertical Movement first (could be done in other order)
            SolveVerticalMovement(scaledVelocity);

            // Update the RobotPosition based on the updated Cylinder center
            MarblePosition = MarbleSphere.Center;

            // Update the Robot World Matrix
            MarbleWorld = MarbleScale * /*MarbleRotation **/ Matrix.CreateTranslation(MarblePosition);
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
            } 
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
              
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            { }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            { }

            FollowCamera.Update(gameTime, MarbleWorld);

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        /// 

        public void DrawMeshes(Matrix matrizDelModelo, Color color, Model modelo)
        {
            foreach (var mesh in modelo.Meshes)
            {
                World = mesh.ParentBone.Transform * matrizDelModelo;
                Effect.Parameters["DiffuseColor"].SetValue(color.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
        }

        public void DrawMeshes(Matrix matrizDelModelo, Texture2D texture, Model modelo)
        {
            foreach (var mesh in modelo.Meshes)
            {
                World = mesh.ParentBone.Transform * matrizDelModelo;
                TextureEffect.Parameters["Texture"].SetValue(texture);
                TextureEffect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            float totalGameTime = Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds);

            // Aca deberiamos poner toda la logica de renderizado del juego.
            GraphicsDevice.Clear(Color.Black);

            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.
            Effect.Parameters["View"].SetValue(FollowCamera.View);
            Effect.Parameters["Projection"].SetValue(FollowCamera.Projection);
            TextureEffect.Parameters["View"].SetValue(FollowCamera.View);
            TextureEffect.Parameters["Projection"].SetValue(FollowCamera.Projection);

            // Para el piso
            LavaEffect.Parameters["World"].SetValue(Matrix.Identity);
            LavaEffect.Parameters["View"].SetValue(FollowCamera.View);
            LavaEffect.Parameters["Projection"].SetValue(FollowCamera.Projection);
            LavaEffect.Parameters["Time"].SetValue(totalGameTime);

            foreach (var pass in LavaEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList,
                    quad.Vertices, 0, 4,
                    quad.Indexes, 0, 2);
            }

            var rotationMatrix = Matrix.CreateRotationY(Rotation);

            //Jugador
            DrawMeshes(MarbleWorld, MarbleTexture, Esfera);

            ////Se agregan la esferas
            DrawMeshes( ( Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(Rotation * 0.2f) * Matrix.CreateTranslation(new Vector3(-50f, -10f, 0f)) ), MagmaTexture, Esfera);

            DrawMeshes( ( Matrix.CreateScale(0.04f) * Matrix.CreateRotationX(Rotation * 1.5f) * Matrix.CreateTranslation(new Vector3(100f, -0f, -100f) ) * Matrix.CreateRotationZ(Rotation * 0.1f)), LavaTexture, Esfera);

            //Se agrega los tuneles


            //Pista de Obstaculos
            //Nivel 1
            //Principio

            DrawMeshes( ( Matrix.CreateScale(15f, 2f, 15f) * Matrix.CreateTranslation(new Vector3(-10f, -18f, 0f)) ), BluePlatformTexture, Platform);

            //Plataforma con rampa
            DrawMeshes( ( Matrix.CreateScale(8f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(22f, -18f, 0f)) ), BluePlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateRotationZ(MathHelper.ToRadians(45f)) * Matrix.CreateTranslation(new Vector3(30f, -14f, 0f)) ), BluePlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(37f, -11f, 0f)) ), BluePlatformBasicTexture, Platform);


            //Plataforma con Obstaculo
            DrawMeshes( ( Matrix.CreateScale(15f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(70f, -18f, 0f)) ), BluePlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(4f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(70f, -14f, 0f)) ), BluePlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(2f, 4f, 4.9f) * Matrix.CreateTranslation(new Vector3(70f, (-4f * MathF.Cos(totalGameTime)) - 12f, 0f)) ), BluePlatformBasicTexture, Platform);

            //tunel
            DrawMeshes( ( Matrix.CreateScale(0.008f) * Matrix.CreateRotationY(7.9f) * Matrix.CreateTranslation(new Vector3(70f, -12f, 0f)) ), Color.Salmon, TunnelChico);



            //Primer punto de control (bandera)
            DrawMeshes( ( Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(80f, -11f, -2f)) ), BluePlaceholderTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(4f, 3f, 0.2f) * Matrix.CreateTranslation(new Vector3(81.8f, -7.5f, -2f)) ), FlagCheckpointTexture, Flag);

            //primera plataforma del nivel 2
            //parte 2.1
            DrawMeshes( ( Matrix.CreateScale(20f, 2f, 2f) * Matrix.CreateRotationY(8f) * Matrix.CreateTranslation(new Vector3(84f, -18f, 30f)) ), GreenPlatformBasicTexture, Platform); //Este no deberia tener color

            //Transformador a pelota chica, pasa por agujeros chicos
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(82f, -12f + MathF.Cos(totalGameTime * 2), 13f)) ), BluePlaceholderTexture, Esfera);

            //cubo que necesita pelota chica del nivel 3
            DrawMeshes( ( Matrix.CreateScale(5f, 5f, 5f) * Matrix.CreateRotationY(3.14159f) * Matrix.CreateTranslation(new Vector3(84f, -10f, 30f)) ), GreenPlatformBasicTexture, Platform);

            //pinches que suben y baja
            DrawMeshes( ( Matrix.CreateScale(0.001f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateTranslation(new Vector3(86f, -9f - (-8f * MathF.Cos(totalGameTime)), 40f)) ), SpikesTexture, Pinches);

            //alas de velocidad
            DrawMeshes( ( Matrix.CreateScale(0.007f) * Matrix.CreateRotationX(-0.785398f) * Matrix.CreateTranslation(new Vector3(86f, -16f, 45f)) ), Color.BlueViolet, Wings);

            //parte 2.2
            //Plataforma
            DrawMeshes( ( Matrix.CreateScale(30f, 2f, 2f) * Matrix.CreateRotationY(7.5f) * Matrix.CreateTranslation(new Vector3(75f, -18f, 85f)) ), GreenPlatformBasicTexture, Platform);

            //cubo que necesita pelota chica del nivel 3.1
            DrawMeshes( ( Matrix.CreateScale(20f, 5f, 8f) * Matrix.CreateRotationY(7.5f) * Matrix.CreateTranslation(new Vector3(75f, -9f, 80f)) ), GreenPlatformBasicTexture, Platform);


            //pinches que suben y baja
            DrawMeshes( ( Matrix.CreateScale(0.0008f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateRotationY(0.5f) * Matrix.CreateTranslation(new Vector3(83f, -7f - (-7f * MathF.Cos(totalGameTime * 2)), 60f)) ), SpikesTexture, Pinches);

            DrawMeshes( ( Matrix.CreateScale(0.0008f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateRotationY(0.5f) * Matrix.CreateTranslation(new Vector3(80f, -7f - (-7f * MathF.Cos((totalGameTime * 2) - 1)), 70f)) ), SpikesTexture, Pinches);

            DrawMeshes( ( Matrix.CreateScale(0.0008f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateRotationY(0.5f) * Matrix.CreateTranslation(new Vector3(77f, -7f - (-7f * MathF.Cos((totalGameTime * 2) - 2)), 80f)) ), SpikesTexture, Pinches);

            DrawMeshes( ( Matrix.CreateScale(0.0008f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateRotationY(0.5f) * Matrix.CreateTranslation(new Vector3(74f, -7f - (-7f * MathF.Cos((totalGameTime * 2) - 3)), 90f)) ), SpikesTexture, Pinches);

            DrawMeshes( ( Matrix.CreateScale(0.0008f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateRotationY(0.5f) * Matrix.CreateTranslation(new Vector3(71f, -7f - (-7f * MathF.Cos((totalGameTime * 2) - 4)), 100f)) ), SpikesTexture, Pinches);


            //Transformador a pelota de roca, resistente a la lava
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(65f, -13f + MathF.Cos(totalGameTime * 2), 112f)) ), BluePlaceholderTexture, Esfera);


            //parte 2.3
            //plataforma 1 
            DrawMeshes( ( Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(52f, -18f, 110f)) ), GreenPlatformTexture, Platform);

            //base
            DrawMeshes( ( Matrix.CreateScale(18f, 2f, 4f) * Matrix.CreateTranslation(new Vector3(35f, -20f, 110f)) ), GreenPlatformBasicTexture, Platform);

            //"lava"1
            DrawMeshes( ( Matrix.CreateScale(10f, 3f, 4f) * Matrix.CreateTranslation(new Vector3(40f, -20f, 110f)) ), BluePlaceholderTexture, Cubo);

            //plataforma 2 
            DrawMeshes( ( Matrix.CreateScale(8f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(23f, -18f, 110f)) ), GreenPlatformBasicTexture, Platform);

            //"lava"2
            DrawMeshes( (Matrix.CreateScale(3f, 20f, 4f) * Matrix.CreateTranslation(new Vector3(22f, -18f, 110f)) ), BluePlaceholderTexture, Cubo);

            //fuente de lava
            DrawMeshes( ( Matrix.CreateScale(5f, 3f, 5f) * Matrix.CreateTranslation(new Vector3(22f, 0f, 110f)) ), MagmaTexture, Cubo);

            //Segundo CheckPoint
            DrawMeshes( ( Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(16f, -11f, 110f)) ), BluePlaceholderTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(4f, 3f, 0.2f) * Matrix.CreateTranslation(new Vector3(14.2f, -7.5f, 110f)) ), FlagCheckpointTexture, Flag);




            //Nivel 3
            //part 3.1
            //plataforma 1
            DrawMeshes( ( Matrix.CreateScale(15f, 2f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-3f, -18f, 100f)) ), RedPlatformBasicTexture, Platform);

            //asensor para subir a parte de arriba
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(4f, -12f + (4 * MathF.Cos(totalGameTime * 2)), 115f)) ), RedPlatformTexture, Platform);

            //parte de arriba
            DrawMeshes( ( Matrix.CreateScale(10f, 2.5f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-3f, -12f, 100f)) ), RedPlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(7f, 3f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-5.5f, -6.5f, 98.9f)) ), RedPlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(10f, 2.5f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-3f, -1f, 100f)) ), RedPlatformBasicTexture, Platform);

            //pelota para ser chica
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(2.5f, -7.5f + MathF.Cos(totalGameTime * 2), 105f)) ), BluePlaceholderTexture, Esfera);

            //parte 3.2
            //plataforma 1
            DrawMeshes( ( Matrix.CreateScale(18f, 2f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-36f, -18f, 83f)) ), RedPlatformBasicTexture, Platform);

            //bloque salto 1
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-27f, -18f, 84f)) ), RedPlatformBasicTexture, Platform);

            //pelota para saltar doble
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(-32f, -13f + MathF.Cos(totalGameTime * 2), 82f)) ), BluePlaceholderTexture, Esfera);

            //bloque salto 2
            DrawMeshes( ( Matrix.CreateScale(2f, 10f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-37f, -18f, 81f)) ), RedPlatformBasicTexture, Platform);

            //bloque salto 3
            DrawMeshes( ( Matrix.CreateScale(2f, 10f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-52f, -18f, 76f)) ), RedPlatformBasicTexture, Platform);

            //plataforma 2
            DrawMeshes( ( Matrix.CreateScale(8f, 1f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-59f, -9.2f, 72f)) ), RedPlatformBasicTexture, Platform);

            //plataforma rotando
            DrawMeshes( ( Matrix.CreateScale(5f, 1f, 5f) * Matrix.CreateRotationY(MathHelper.ToRadians(-15f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-25f * totalGameTime)) * Matrix.CreateTranslation(new Vector3(-70f, -7f, 67.5f)) ), RedPlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(5f, 1f, 5f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-78f, -4f, 67.5f)) ), RedPlatformBasicTexture, Platform);

            //plataforma 3
            DrawMeshes( ( Matrix.CreateScale(8f, 1f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-80f, -4f, 67.5f)) ), RedPlatformBasicTexture, Platform);

            //pinches suben y baja
            DrawMeshes( ( Matrix.CreateScale(0.001f) * Matrix.CreateTranslation(new Vector3(-80f, -9f + (-6f * MathF.Cos(totalGameTime)), 67.5f)) ), SpikesTexture, Pinches);

            //bloque 4
            DrawMeshes( ( Matrix.CreateScale(5f, 10f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-87.5f, -2f, 65f)) ), RedPlatformTexture, Platform);

            //Tercer CheckPoint
            DrawMeshes( ( Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(-87.5f, 12f, 65f)) ), BluePlaceholderTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(4f, 3f, 0.2f) * Matrix.CreateTranslation(new Vector3(-89.2f, 15.5f, 65f)) ), FlagCheckpointTexture, Flag);

            //parte 4
            //plataforma 1
            DrawMeshes( ( Matrix.CreateScale(18f, 2f, 3f) * Matrix.CreateRotationY(MathHelper.ToRadians(-90f)) * Matrix.CreateTranslation(new Vector3(-87.5f, 10f, 42f)) ), RedPlatformBasicTexture, Platform);

            //pinches
            DrawMeshes( ( Matrix.CreateScale(0.001f) * Matrix.CreateRotationZ(MathHelper.ToRadians(-90f)) * Matrix.CreateTranslation(new Vector3(-87.5f + (MathF.Cos(totalGameTime) * 8), 13f, 49f)) ), SpikesTexture, Pinches);

            DrawMeshes( ( Matrix.CreateScale(0.001f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(-87.5f - (MathF.Cos(totalGameTime) * 8), 13f, 43f)) ), SpikesTexture, Pinches);

            DrawMeshes( ( Matrix.CreateScale(0.001f) * Matrix.CreateRotationZ(MathHelper.ToRadians(-90f)) * Matrix.CreateTranslation(new Vector3(-87.5f + (MathF.Cos(totalGameTime) * 8), 13f, 37f)) ), SpikesTexture, Pinches);

            DrawMeshes( ( Matrix.CreateScale(0.001f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(-87.5f - (MathF.Cos(totalGameTime) * 8), 13f, 31f)) ), SpikesTexture, Pinches);




            //Parte 4.2
            //plataforma fija
            DrawMeshes( ( Matrix.CreateScale(2f, 0.3f, 2f) * Matrix.CreateTranslation(new Vector3(-87.5f, 0f, 25f)) ), RedPlatformBasicTexture, Platform);

            //Transformador a pelota de roca, resistente a la lava
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(-87.5f, 3f + MathF.Cos(totalGameTime * 2), 25f)) ), BluePlaceholderTexture, Esfera);

            //asensor 1
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-87.5f, 8f + (8 * MathF.Cos(totalGameTime * 2)), 20f)) ), RedPlatformTexture, Platform);

            //asensor 2
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-80f, 8f + (8 * MathF.Cos((totalGameTime * 2) + 2)), 17f)) ), RedPlatformTexture, Platform);

            //asensor 3
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-72.5f, 8f + (8 * MathF.Cos((totalGameTime * 1.5f) + 4)), 17f)) ), RedPlatformTexture, Platform);

            //asensor 4
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-62.5f, 8f + (8 * MathF.Cos((totalGameTime * 3f) + 6)), 17f)) ), RedPlatformTexture, Platform);

            //"lava"2
            DrawMeshes( ( Matrix.CreateScale(3f, 40f, 4f) * Matrix.CreateTranslation(new Vector3(-57.5f, 0f, 17f)) ), BluePlaceholderTexture, Cubo);

            //asensor 5
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-51f, 8f + (8 * MathF.Cos((totalGameTime * 2.5f) + 8)), 17f)) ), RedPlatformTexture, Platform);

            //asensor 6
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-43.5f, 15f + (9 * MathF.Cos((totalGameTime * 4f) + 10)), 17f)) ), RedPlatformTexture, Platform);


            //Parte 4.3
            //plataforma 2
            DrawMeshes( ( Matrix.CreateScale(15f, 1f, 3f) * Matrix.CreateTranslation(new Vector3(-25f, 20f, 17f)) ), RedPlatformBasicTexture, Platform);

            //Transformador a pelota normal
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(-43.5f, 15f + MathF.Cos(totalGameTime * 2), 17f)) ), BluePlaceholderTexture, Esfera);

            //"lava"1
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-37f, 16f + (3f * MathF.Cos((totalGameTime * 2f) + 4)), 17f)) ), BluePlaceholderTexture, Cubo);

            //"lava"2
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-32f, 16f + (4f * MathF.Cos((totalGameTime * 2f) + 3)), 17f)) ), BluePlaceholderTexture, Cubo);

            //"lava"3
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-27f, 16f + (4f * MathF.Cos((totalGameTime * 2f) + 2)), 17f)) ), BluePlaceholderTexture, Cubo);

            //"lava"4
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-22f, 16f + (4f * MathF.Cos((totalGameTime * 2f) + 1)), 17f)) ), BluePlaceholderTexture, Cubo);

            //"lava"5
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-17f, 16f + (4f * MathF.Cos(totalGameTime * 2f)), 17f)) ), BluePlaceholderTexture, Cubo);

            //plataforma 3
            DrawMeshes( ( Matrix.CreateScale(15f, 1f, 3f) * Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(2f, 22f, 10f)) ), RedPlatformBasicTexture, Platform);
            
            //Ultimo Checkpoint
            DrawMeshes( ( Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(0f, 28f, 3f)) ), BluePlaceholderTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(4f, 3f, 0.2f) * Matrix.CreateTranslation(new Vector3(1.8f, 31.5f, 3f)) ), FlagCheckeredTexture, Flag);


            //Background
            //Se agregan cubos

            //Molino
            DrawMeshes( ( Matrix.CreateScale(2f, 20f, 2f) * Matrix.CreateTranslation(new Vector3(-4f, 0f, 24f)) ), StoneTexture, Cubo);

            DrawMeshes((Matrix.CreateScale(0.5f, 12f, 1f) * Matrix.CreateRotationX(Rotation) * Matrix.CreateTranslation(new Vector3(-6.5f, 18f, 24f))), WoodTexture, Cubo);

            DrawMeshes((Matrix.CreateScale(0.5f, 12f, 1f) * Matrix.CreateRotationX(Rotation + MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(-6.5f, 18f, 24f))), WoodTexture, Cubo);

            //DrawMeshes( ( Matrix.CreateScale(20f, 0.5f, 1f) * Matrix.CreateRotationY(MathHelper.ToRadians(45f)) * Matrix.CreateTranslation(new Vector3(-40f, -18f, 0f)) ), Color.Crimson, Cubo);

            //DrawMeshes( ( Matrix.CreateScale(3f, 3f, 1f) * Matrix.CreateRotationY(MathHelper.ToRadians(75f)) * Matrix.CreateTranslation(new Vector3(-30f, -18f, 10f)) ), Color.Pink, Cubo);

            DrawMeshes( ( Matrix.CreateScale(6f, 6f, 6f) * Matrix.CreateRotationX(Rotation) * Matrix.CreateTranslation(new Vector3(120f, -12f, 0f)) ), MagmaTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(3f, 3f, 3f) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(new Vector3(-60f, 0f, 100f)) ), MagmaTexture, Cubo);

            //Nubes
            DrawMeshes( ( Matrix.CreateScale(10f, 2f, 10f) * Matrix.CreateTranslation(new Vector3(-30f, 30f, -50f)) ), WhitePlaceholderTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(10f, 2f, 10f) * Matrix.CreateTranslation(new Vector3(30f, 30f, 50f)) ), WhitePlaceholderTexture, Cubo);

            //DrawMeshes( ( Matrix.CreateScale(2f, 2f, 10f) * Matrix.CreateRotationX(MathHelper.ToRadians(45f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(45f)) * Matrix.CreateTranslation(new Vector3(30f, -10f, 20f)) ), Color.WhiteSmoke, Cubo);

            //DrawMeshes( ( Matrix.CreateScale(2f, 2f, 2f) * Matrix.CreateTranslation(new Vector3(30f, -15f, 20f)) ), Color.RoyalBlue, Cubo);

            DrawMeshes( ( Matrix.CreateScale(2f, 2f, 2f) * Matrix.CreateRotationX(Rotation) * Matrix.CreateTranslation(new Vector3(15f, 10f, 85f)) ), MagmaTexture, Cubo);

            //Helicoptero
            DrawMeshes( ( Matrix.CreateScale(1f, 0.1f, 11f) * Matrix.CreateRotationY(Rotation * 10) * Matrix.CreateTranslation(new Vector3(12f, 30f, -20f)) ), MetalTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(1f, 0.1f, 11f) * Matrix.CreateRotationY(Rotation * 10 + MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(10f, 30f, -20f)) ), MetalTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(0.5f, 2.7f, 0.5f) * Matrix.CreateTranslation(new Vector3(10f, 28f, -20f)) ), BluePlaceholderTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(7f, 3f, 3f) * Matrix.CreateTranslation(new Vector3(8f, 22f, -20f)) ), BluePlaceholderTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(5f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(-4f, 24f, -20f)) ), BluePlaceholderTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(1f, 5f, 0.1f) * Matrix.CreateRotationZ(Rotation * 5) * Matrix.CreateTranslation(new Vector3(-7f, 24f, -21f)) ), MetalTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(1f, 5f, 0.1f) * Matrix.CreateRotationZ(Rotation * 5 + MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(-7f, 24f, -21f)) ), MetalTexture, Cubo);

            //mas carteles
            //DrawMeshes( ( Matrix.CreateScale(0.07f) * Matrix.CreateTranslation(new Vector3(10f, -18f, 13f)) ), CartelTexture, Cartel);

            //DrawMeshes( ( Matrix.CreateScale(0.05f) * Matrix.CreateTranslation(new Vector3(0f, -20f, 10f)) ), Color.Blue, Cartel);

            //DrawMeshes( ( Matrix.CreateScale(0.04f) * Matrix.CreateTranslation(new Vector3(-10f, -18f, 7f)) ), Color.Aqua, Cartel);

            //DrawMeshes((Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(new Vector3(50f, -10f, 0f))), Color.GreenYellow, Cartel);


            List<Vector3> monedas = new List<Vector3>
            {
                new Vector3(-43.5f, 20f + MathF.Cos(totalGameTime * 2), 25f),
                new Vector3(10, -10 + MathF.Cos(totalGameTime * 2), 0),
                new Vector3(25, -14+ MathF.Cos(totalGameTime * 2), 0),
                new Vector3(37, -5+ MathF.Cos(totalGameTime * 2), 0),
                new Vector3(53, -10+ MathF.Cos(totalGameTime * 2), 0),
                new Vector3(63, -10+ MathF.Cos(totalGameTime * 2), 0),
                new Vector3(35f, -20f+ MathF.Cos(totalGameTime * 2), 110f),
                new Vector3(50, -13+ MathF.Cos(totalGameTime * 2), 110),
                new Vector3(55, -14+ MathF.Cos(totalGameTime * 2), 110),
                new Vector3(45, -16+ MathF.Cos(totalGameTime * 2), 110),
                new Vector3(40, -14+ MathF.Cos(totalGameTime * 2), 110),
                new Vector3(35, -14+ MathF.Cos(totalGameTime * 2), 110),
                new Vector3(27.5f, -12.5f+ MathF.Cos(totalGameTime * 2), 110),
                new Vector3(22.5f, -12.5f+ MathF.Cos(totalGameTime * 2), 110),
                new Vector3(4f, -12f+ MathF.Cos(totalGameTime * 2), 115),
                new Vector3(4f, -8f+ MathF.Cos(totalGameTime * 2), 115),
                new Vector3(4f, -5f+ MathF.Cos(totalGameTime * 2), 115),
                new Vector3(7f, -12f+ MathF.Cos(totalGameTime * 2), 107.5f),
                new Vector3(-17.5f, -12f+ MathF.Cos(totalGameTime * 2), 92.5f),
                new Vector3(-22.5f, -12f+ MathF.Cos(totalGameTime * 2), 87.5f),
                new Vector3(-27.5f, -7f+ MathF.Cos(totalGameTime * 2), 85f),
                new Vector3(-27.5f, -2f+ MathF.Cos(totalGameTime * 2), 85f),
                new Vector3(-27.5f, 1f+ MathF.Cos(totalGameTime * 2), 85f),
                new Vector3(-37.5f, -2f+ MathF.Cos(totalGameTime * 2), 82.5f),
                new Vector3(-37.5f, 2f+ MathF.Cos(totalGameTime * 2), 82.5f),
                new Vector3(-42.5f, 0f+ MathF.Cos(totalGameTime * 2), 80f),
                new Vector3(-45f, -3f+ MathF.Cos(totalGameTime * 2), 80f),
                new Vector3(-48f, -6f+ MathF.Cos(totalGameTime * 2), 78f),
                new Vector3(-47.5f, -12.5f+ MathF.Cos(totalGameTime * 2), 78f),
                new Vector3(-52.5f, -2.5f+ MathF.Cos(totalGameTime * 2), 75f),
                new Vector3(-52.5f, 0f+ MathF.Cos(totalGameTime * 2), 75f),
                new Vector3(-52.5f, 2.5f+ MathF.Cos(totalGameTime * 2), 75f),
                new Vector3(-57.5f, -2.5f+ MathF.Cos(totalGameTime * 2), 77.5f),
                new Vector3(-67.5f, 5f+ MathF.Cos(totalGameTime * 2), 70f),
                new Vector3(-67.5f, 0f+ MathF.Cos(totalGameTime * 2), 70f),
                new Vector3(-72.5f, 0f+ MathF.Cos(totalGameTime * 2), 67.5f),
                new Vector3(-77.5f, 0f+ MathF.Cos(totalGameTime * 2), 62.5f),
                new Vector3(-77.5f, 5f+ MathF.Cos(totalGameTime * 2), 62.5f),
                new Vector3(-77.5f, 7.5f+ MathF.Cos(totalGameTime * 2), 62.5f),
                new Vector3(-87.5f, 15f+ MathF.Cos(totalGameTime * 2), 49f),
                new Vector3(-87.5f, 15f+ MathF.Cos(totalGameTime * 2), 45f),
                new Vector3(-87.5f, 15f+ MathF.Cos(totalGameTime * 2), 40f),
                new Vector3(-87.5f, 15f+ MathF.Cos(totalGameTime * 2), 35f)
            };
            foreach (Vector3 vector in monedas)
            {
                DrawMeshes((Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateRotationZ(totalGameTime) * Matrix.CreateTranslation(vector)), CoinTexture, Moneda);
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

        private void SolveVerticalMovement(Vector3 scaledVelocity)
        {
            // If the Robot has vertical velocity
            if (scaledVelocity.Y == 0f)
                return;

            // Start by moving the Cylinder
            MarbleSphere.Center += Vector3.Up * scaledVelocity.Y;
            // Set the OnGround flag on false, update it later if we find a collision
            OnGround = false;


            // Collision detection
            var collided = false;

            if (MarbleSphere.Intersects(platformCollider))
            {
                collided = true;

                // If we collided with something, set our velocity in Y to zero to reset acceleration
                MarbleVelocity = new Vector3(MarbleVelocity.X, 0f, MarbleVelocity.Z);
            }          

            
            // We correct based on differences in Y until we don't collide anymore
            // Not usual to iterate here more than once, but could happen
            while (collided)
            {
                var collider = platformCollider;
                var max = collider.Max;
                var min = collider.Min;
                Vector3 center = (max + min) * 0.5f;
                var colliderY = center.Y;
                var cylinderY = MarbleSphere.Center.Y;
                Vector3 extents = (max - min) * 0.5f;

                float penetration;
                // If we are on top of the collider, push up
                // Also, set the OnGround flag to true
                if (cylinderY > colliderY)
                {
                    penetration = colliderY + extents.Y - cylinderY + MarbleSphere.Radius;
                    OnGround = true;
                }

                // If we are on bottom of the collider, push down
                else
                    penetration = -cylinderY - MarbleSphere.Radius + colliderY - extents.Y;

                // Move our Cylinder so we are not colliding anymore
                MarbleSphere.Center += Vector3.Up * penetration;
                collided = false;
                /*
                // Check for collisions again
                for (var index = 0; index < Colliders.Length; index++)
                {
                    if (!RobotCylinder.Intersects(Colliders[index]).Equals(BoxCylinderIntersection.Intersecting))
                        continue;

                    // Iterate until we don't collide with anything anymore
                    collided = true;
                    foundIndex = index;
                    break;
                }*/
            }

        }
    }
}

// idea obstaculo: El cartel puede ser un obstaculo que si lo hacemos rotar el jugador tendria que evitarlo