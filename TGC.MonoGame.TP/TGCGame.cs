using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;


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
        private Model Pinches { get; set; }
        private Model Wings { get; set; }
        private Model Moneda { get; set; }


        private Effect Effect { get; set; }
        private BasicEffect BasicEffect { get; set; }
        private float Rotation { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        public VertexBuffer Vertices { get; private set; }
        public IndexBuffer Indices { get; private set; }
        private Camera Camera { get; set; }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.
            // Seria hasta aca.

            // Configuramos nuestras matrices de la escena.
            World = Matrix.Identity;
            View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);
            var screenSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0, 0, 0), screenSize);
            // Setup our basic effect
            BasicEffect = new BasicEffect(GraphicsDevice)
            {
                World = World,
                View = View,
                Projection = Projection,
                VertexColorEnabled = true
            };

            float yPositionFloor = -20f;
            float xScaleFloor = 200f;
            float zScaleFloor = 200f;

            // Array of vertex positions and colors.
            var triangleVertices = new[]
            {
                new VertexPositionColor(new Vector3(-1f * xScaleFloor, yPositionFloor, 1f * zScaleFloor), Color.Blue),
                new VertexPositionColor(new Vector3(-1f * xScaleFloor, yPositionFloor, -1f * zScaleFloor), Color.Red),
                new VertexPositionColor(new Vector3(1f * xScaleFloor, yPositionFloor, -1f * zScaleFloor), Color.Green),
                new VertexPositionColor(new Vector3(1f * xScaleFloor, yPositionFloor, 1f * zScaleFloor), Color.Yellow)
            };

            Vertices = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, triangleVertices.Length,
                BufferUsage.WriteOnly);
            Vertices.SetData(triangleVertices);

            // Array of indices
            var triangleIndices = new ushort[]
            {
                0, 1, 2, 0, 2, 3
            };

            Indices = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, triangleIndices.Length, BufferUsage.None);
            Indices.SetData(triangleIndices);

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
            //cargo pinches
            Pinches = Content.Load<Model>(ContentFolder3D + "Marbel/Pinches/Pinches");
            //cargo wings
            Wings = Content.Load<Model>(ContentFolder3D + "Marbel/Wings/Wings");
            //cargo moneda
            Moneda = Content.Load<Model>(ContentFolder3D + "Marbel/Moneda/Moneda");
            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            
            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.
            // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
            //mesh Cartel
            foreach (var mesh in Cartel.Meshes)            
            foreach (var meshPart in mesh.MeshParts)
                meshPart.Effect = Effect;
            //mesh Cubo
            foreach (var mesh in Cubo.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;
            //mesh esfera
            foreach (var mesh in Esfera.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;
            //mesh tunel
            foreach (var mesh in TunnelChico.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;
            //mesh pinches
            foreach (var mesh in Pinches.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;
            //mesh wings
            foreach (var mesh in Wings.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;
            //mesh moneda
            foreach (var mesh in Moneda.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;
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
                //Salgo del juego.
                Exit();
            // Basado en el tiempo que paso se va generando una rotacion.
            Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Camera.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            float totalGameTime = Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds);

            // Aca deberiamos poner toda la logica de renderizado del juego.
            GraphicsDevice.Clear(Color.Black);

            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.
            Effect.Parameters["View"].SetValue(Camera.View);
            Effect.Parameters["Projection"].SetValue(Camera.Projection);


            // Para el piso
            // Set our vertex buffer.
            GraphicsDevice.SetVertexBuffer(Vertices);

            // Set our index buffer
            GraphicsDevice.Indices = Indices;

            BasicEffect.World = Matrix.Identity;
            BasicEffect.View = Camera.View;
            BasicEffect.Projection = Camera.Projection;

            foreach (var pass in BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawIndexedPrimitives(
                    // We’ll be rendering one triangles.
                    PrimitiveType.TriangleList,
                    // The offset, which is 0 since we want to start at the beginning of the Vertices array.
                    0,
                    // The start index in the Vertices array.
                    0,
                    // The number of triangles to draw.
                    2);
            }

            var rotationMatrix = Matrix.CreateRotationY(Rotation);

            //Se agrega el cartel
            foreach (var mesh in Cartel.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(new Vector3(50f, -10f, 0f));
                //asigno color verde amarillo 
                Effect.Parameters["DiffuseColor"].SetValue(Color.GreenYellow.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }

            //Se agregan la esferas
            foreach (var mesh in Esfera.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(new Vector3(-50f, -10f, 0f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
                mesh.Draw();
            }
            foreach (var mesh in Esfera.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.02f) * Matrix.CreateTranslation(new Vector3(0f, -14f, 30f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gold.ToVector3());
                mesh.Draw();
            }
            foreach (var mesh in Esfera.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.04f) * Matrix.CreateTranslation(new Vector3(100f, -0f, -100f)) * Matrix.CreateRotationZ(Rotation * 0.1f);
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gold.ToVector3());
                mesh.Draw();
            }

            //Se agrega los tuneles


            //Pista de Obstaculos
            //Nivel 1
            //Principio
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(0f, -18f, 30f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }

            //Plataforma con rampa
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(8f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(22f, -18f, 30f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateRotationZ(MathHelper.ToRadians(45f)) * Matrix.CreateTranslation(new Vector3(30f, -14f, 30f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(37f, -11f, 30f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }

            //Plataforma con Obstaculo
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(60f, -18f, 30f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkBlue.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(15f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(70f, -18f, 30f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkBlue.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(4f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(70f, -14f, 30f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkBlue.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 4f, 4.9f) * Matrix.CreateTranslation(new Vector3(70f, (-4f * MathF.Cos(totalGameTime)) - 12f, 30f)); //Agregar movimiento
                Effect.Parameters["DiffuseColor"].SetValue(Color.Yellow.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            //tunel
            foreach (var mesh in TunnelChico.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.008f) * Matrix.CreateRotationY(7.9f) * Matrix.CreateTranslation(new Vector3(70f, -12f, 30f));
                Effect.Parameters["World"].SetValue(World);
                //asigno color salmons
                Effect.Parameters["DiffuseColor"].SetValue(Color.Salmon.ToVector3());
                mesh.Draw();
            }


            //Primer punto de control (bandera)
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(80f, -11f, 28f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.PeachPuff.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 1f, 0.2f) * Matrix.CreateTranslation(new Vector3(82f, -7f, 28f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.FloralWhite.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }


            //primera plataforma del nivel 2
            //parte 2.1
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(20f, 2f, 2f) * Matrix.CreateRotationY(8f) * Matrix.CreateTranslation(new Vector3(84f, -18f, 60f));
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            //Transformador a pelota chica, pasa por agujeros chicos
            foreach (var mesh in Esfera.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(82f, -12f + MathF.Cos(totalGameTime * 2), 43f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.HotPink.ToVector3());
                mesh.Draw();
            }
            //cubo que necesita pelota chica del nivel 3
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(5f, 5f, 5f) * Matrix.CreateRotationY(3.14159f) * Matrix.CreateTranslation(new Vector3(84f, -10f, 60f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.LightYellow.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            //pinches que suben y baja
            foreach (var mesh in Pinches.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.001f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateTranslation(new Vector3(86f, -9f - (-8f * MathF.Cos(totalGameTime)), 70f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Black.ToVector3());
                mesh.Draw();
            }
            //alas de velocidad
            foreach (var mesh in Wings.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.007f) * Matrix.CreateRotationX(-0.785398f) * Matrix.CreateTranslation(new Vector3(86f, -16f, 75f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.BlueViolet.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }

            //parte 2.2
            //Plataforma
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(30f, 2f, 2f) * Matrix.CreateRotationY(7.5f) * Matrix.CreateTranslation(new Vector3(75f, -18f, 115f));
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            //cubo que necesita pelota chica del nivel 3.1
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(20f, 5f, 8f) * Matrix.CreateRotationY(7.5f) * Matrix.CreateTranslation(new Vector3(75f, -9f, 110f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gold.ToVector3());
                mesh.Draw();
            }
            //pinches que suben y baja
            foreach (var mesh in Pinches.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.0008f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateRotationY(0.5f) * Matrix.CreateTranslation(new Vector3(83f, -9f - (-8f * MathF.Cos(totalGameTime * 2)), 90f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Ivory.ToVector3());
                mesh.Draw();
            }
            foreach (var mesh in Pinches.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.0008f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateRotationY(0.5f) * Matrix.CreateTranslation(new Vector3(80f, -9f - (-8f * MathF.Cos((totalGameTime * 2) - 1)), 100f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Ivory.ToVector3());
                mesh.Draw();
            }
            foreach (var mesh in Pinches.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.0008f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateRotationY(0.5f) * Matrix.CreateTranslation(new Vector3(77f, -9f - (-8f * MathF.Cos((totalGameTime * 2) - 2)), 110f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Ivory.ToVector3());
                mesh.Draw();
            }
            foreach (var mesh in Pinches.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.0008f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateRotationY(0.5f) * Matrix.CreateTranslation(new Vector3(74f, -9f - (-8f * MathF.Cos((totalGameTime * 2) - 3)), 120f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Ivory.ToVector3());
                mesh.Draw();
            }
            foreach (var mesh in Pinches.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.0008f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateRotationY(0.5f) * Matrix.CreateTranslation(new Vector3(71f, -9f - (-8f * MathF.Cos((totalGameTime * 2) - 4)), 130f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Ivory.ToVector3());
                mesh.Draw();
            }

            //Transformador a pelota de roca, resistente a la lava
            foreach (var mesh in Esfera.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(65f, -13f + MathF.Cos(totalGameTime * 2), 142f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.HotPink.ToVector3());
                mesh.Draw();
            }

            //parte 2.3
            //plataforma 1 
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(52f, -18f, 140f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.SandyBrown.ToVector3());
                mesh.Draw();
            }
            //base
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(18f, 2f, 4f) * Matrix.CreateTranslation(new Vector3(35f, -20f, 140f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Ivory.ToVector3());
                mesh.Draw();
            }
            //"lava"1
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(10f, 3f, 4f) * Matrix.CreateTranslation(new Vector3(40f, -20f, 140f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                mesh.Draw();
            }
            //plataforma 2 
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(8f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(23f, -18f, 140f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.SandyBrown.ToVector3());
                mesh.Draw();
            }
            //"lava"2
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(3f, 20f, 4f) * Matrix.CreateTranslation(new Vector3(22f, -18f, 140f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                mesh.Draw();
            }
            //fuente de lava
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(5f, 3f, 5f) * Matrix.CreateTranslation(new Vector3(22f, 0f, 140f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.LightGray.ToVector3());
                mesh.Draw();
            }
            //Segundo CheckPoint
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(16f, -11f, 140f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.PeachPuff.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 1f, 0.2f) * Matrix.CreateTranslation(new Vector3(14f, -7f, 140f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.FloralWhite.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }



            //Nivel 3
            //part 3.1
            //plataforma 1
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(15f, 2f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-3f, -18f, 130f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.SandyBrown.ToVector3());
                mesh.Draw();
            }
            //asensor para subir a parte de arriba
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(4f, -12f + (4 * MathF.Cos(totalGameTime * 2)), 145f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());
                mesh.Draw();
            }
            //parte de arriba
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(10f, 2.5f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-3f, -12f, 130f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.SandyBrown.ToVector3());
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(7f, 3f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-6f, -8f, 130f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.SandyBrown.ToVector3());
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(10f, 2.5f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-3f, -2.5f, 130f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.SandyBrown.ToVector3());
                mesh.Draw();
            }
            //pelota para ser chica
            foreach (var mesh in Esfera.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(2.5f, -7.5f + MathF.Cos(totalGameTime * 2), 135f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.HotPink.ToVector3());
                mesh.Draw();
            }

            //parte 3.2
            //plataforma 1
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(18f, 2f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-36f, -18f, 113f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.SandyBrown.ToVector3());
                mesh.Draw();
            }
            //bloque salto 1
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 5f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-27f, -18f, 114f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.SandyBrown.ToVector3());
                mesh.Draw();
            }
            //pelota para saltar doble
            foreach (var mesh in Esfera.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(-32f, -13f + MathF.Cos(totalGameTime * 2), 112f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.HotPink.ToVector3());
                mesh.Draw();
            }
            //bloque salto 2
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 10f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-37f, -18f, 111f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.SandyBrown.ToVector3());
                mesh.Draw();
            }
            //bloque salto 3
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 10f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-52f, -18f, 106f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.SandyBrown.ToVector3());
                mesh.Draw();
            }
            //plataforma 2
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(8f, 1f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-59f, -9.2f, 102f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            //plataforma rotando
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(5f, 1f, 5f) * Matrix.CreateRotationY(MathHelper.ToRadians(-15f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-25f * totalGameTime)) * Matrix.CreateTranslation(new Vector3(-70f, -7f, 97.5f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(5f, 1f, 5f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-78f, -4f, 97.5f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            //plataforma 3
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(8f, 1f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-80f, -4f, 97.5f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            //pinches suben y baja
            foreach (var mesh in Pinches.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.001f) * Matrix.CreateTranslation(new Vector3(-80f, -9f + (-6f * MathF.Cos(totalGameTime)), 97.5f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Black.ToVector3());
                mesh.Draw();
            }
            //bloque 4
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(5f, 10f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-87.5f, -2f, 95f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.SandyBrown.ToVector3());
                mesh.Draw();
            }
            //Tercer CheckPoint
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(-87.5f, 12f, 95f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.PeachPuff.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 1f, 0.2f) * Matrix.CreateTranslation(new Vector3(-89f, 17f, 95f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.FloralWhite.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            //parte 4.1
            //plataforma 1
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(18f, 2f, 3f) * Matrix.CreateRotationY(MathHelper.ToRadians(-90f)) * Matrix.CreateTranslation(new Vector3(-87.5f, 10f, 72f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.SandyBrown.ToVector3());
                mesh.Draw();
            }
            //pinches
            foreach (var mesh in Pinches.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.001f) * Matrix.CreateRotationZ(MathHelper.ToRadians(-90f)) * Matrix.CreateTranslation(new Vector3(-87.5f + (MathF.Cos(totalGameTime) * 8), 13f, 79f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());
                mesh.Draw();
            }
            foreach (var mesh in Pinches.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.001f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(-87.5f - (MathF.Cos(totalGameTime) * 8), 13f, 73f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());
                mesh.Draw();
            }
            foreach (var mesh in Pinches.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.001f) * Matrix.CreateRotationZ(MathHelper.ToRadians(-90f)) * Matrix.CreateTranslation(new Vector3(-87.5f + (MathF.Cos(totalGameTime) * 8), 13f, 67f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());
                mesh.Draw();
            }
            foreach (var mesh in Pinches.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.001f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(-87.5f - (MathF.Cos(totalGameTime) * 8), 13f, 61f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());
                mesh.Draw();
            }

            //Parte 4.2
            //plataforma fija
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 0.3f, 2f) * Matrix.CreateTranslation(new Vector3(-87.5f, 0f, 55f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());
                mesh.Draw();
            }
            //Transformador a pelota de roca, resistente a la lava
            foreach (var mesh in Esfera.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(-87.5f, 3f + MathF.Cos(totalGameTime * 2), 55f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.HotPink.ToVector3());
                mesh.Draw();
            }
            //asensor 1
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-87.5f, 8f + (8 * MathF.Cos(totalGameTime * 2)), 50f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());
                mesh.Draw();
            }
            //asensor 2
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-80f, 8f + (8 * MathF.Cos((totalGameTime * 2)+2)), 47f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());
                mesh.Draw();
            }
            //asensor 3
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-72.5f, 8f + (8 * MathF.Cos((totalGameTime * 1.5f) + 4)), 47f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());
                mesh.Draw();
            }
            //asensor 4
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-62.5f, 8f + (8 * MathF.Cos((totalGameTime * 3f) + 6)), 47f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());
                mesh.Draw();
            }
            //"lava"1
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(3f, 40f, 4f) * Matrix.CreateTranslation(new Vector3(-57.5f, 0f, 47f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                mesh.Draw();
            }
            //asensor 5
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-51f, 8f + (8 * MathF.Cos((totalGameTime * 2.5f) + 8)), 47f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());
                mesh.Draw();
            }
            //asensor 6
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-43.5f, 15f + (9 * MathF.Cos((totalGameTime * 4f) + 10)), 47f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());
                mesh.Draw();
            }
            //Parte 4.3
            //plataforma 2
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(15f, 1f, 3f) * Matrix.CreateTranslation(new Vector3(-25f, 20f, 47f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.SandyBrown.ToVector3());
                mesh.Draw();
            }
            //Transformador a pelota normal
            foreach (var mesh in Esfera.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(-43.5f, 15f + MathF.Cos(totalGameTime * 2), 47f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.HotPink.ToVector3());
                mesh.Draw();
            }
            //"lava"1
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-37f, 16f + (3f*MathF.Cos((totalGameTime * 2f)+4)), 47f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                mesh.Draw();
            }
            //"lava"2
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-32f, 16f + (4f * MathF.Cos((totalGameTime * 2f)+3)), 47f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                mesh.Draw();
            }
            //"lava"3
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-27f, 16f + (4f * MathF.Cos((totalGameTime * 2f) + 2)), 47f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                mesh.Draw();
            }
            //"lava"4
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-22f, 16f + (4f * MathF.Cos((totalGameTime * 2f) + 1)), 47f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                mesh.Draw();
            }
            //"lava"5
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-17f, 16f + (4f * MathF.Cos(totalGameTime * 2f)), 47f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
                mesh.Draw();
            }
            //plataforma 3
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(15f, 1f, 3f) * Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(-5f, 22f, 53f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.SandyBrown.ToVector3());
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(-5f,28f, 53f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.PeachPuff.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 1f, 0.2f) * Matrix.CreateTranslation(new Vector3(-3f, 32f, 53f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.FloralWhite.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            //Background
            //Se agregan cubos
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 18f, 2f) * Matrix.CreateTranslation(new Vector3(0f, -10f, -23f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.DeepPink.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(20f, 0.5f, 1f) * Matrix.CreateRotationY(MathHelper.ToRadians(45f)) * Matrix.CreateTranslation(new Vector3(-40f, -18f, 0f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.Crimson.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(3f, 3f, 1f) * Matrix.CreateRotationY(MathHelper.ToRadians(75f)) * Matrix.CreateTranslation(new Vector3(-30f, -18f, 0f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.Pink.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(6f, 6f, 6f) * Matrix.CreateRotationX(MathHelper.ToRadians(45f)) * Matrix.CreateTranslation(new Vector3(80f, -12f, 0f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.Pink.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(3f, 3f, 3f) * Matrix.CreateRotationZ(MathHelper.ToRadians(45f)) * Matrix.CreateTranslation(new Vector3(-20f, -15f, 50f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.Fuchsia.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(10f, 2f, 10f) * Matrix.CreateTranslation(new Vector3(-30f, 10f, -50f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.WhiteSmoke.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 2f, 10f) * Matrix.CreateRotationX(MathHelper.ToRadians(45f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(45f)) * Matrix.CreateTranslation(new Vector3(30f, -10f, 20f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.WhiteSmoke.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 2f, 2f)  * Matrix.CreateTranslation(new Vector3(30f, -15f, 20f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.RoyalBlue.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 2f, 2f) * Matrix.CreateRotationX(Rotation) * Matrix.CreateTranslation(new Vector3(15f, 0f, -20f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.RoyalBlue.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(1f, 10f, 1f) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(new Vector3(0f, 5f, -20f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.Orange.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(1f, 10f, 1f) * Matrix.CreateRotationZ(Rotation + MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(0f, 5f, -20f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.Orange.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(1f, 1f, 10f) * Matrix.CreateRotationY(Rotation * 4) * Matrix.CreateTranslation(new Vector3(40f, 20f, -20f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.Maroon.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(1f, 1f, 10f) * Matrix.CreateRotationY(Rotation * 4 + MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(40f, 20f, -20f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.Maroon.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(1f, 3f, 1f) * Matrix.CreateTranslation(new Vector3(40f, 18f, -20f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.Maroon.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(7f, 3f, 1f) * Matrix.CreateTranslation(new Vector3(40f, 12f, -20f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.LightYellow.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(5f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(28f, 14f, -20f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.LightYellow.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(1f, 5f, 1f) * Matrix.CreateRotationZ(Rotation * 3) * Matrix.CreateTranslation(new Vector3(25f, 14f, -20f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.Maroon.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(1f, 5f, 1f) * Matrix.CreateRotationZ(Rotation * 3 + MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(25f, 14f, -20f));
                Effect.Parameters["DiffuseColor"].SetValue(Color.Maroon.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }

            //mas carteles
            foreach (var mesh in Cartel.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.07f) * Matrix.CreateTranslation(new Vector3(10f, -18f, 13f));
                //asigno color verde amarillo 
                Effect.Parameters["DiffuseColor"].SetValue(Color.Aquamarine.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cartel.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.05f) * Matrix.CreateTranslation(new Vector3(0f, -20f, 10f));
                //asigno color verde amarillo 
                Effect.Parameters["DiffuseColor"].SetValue(Color.Blue.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cartel.Meshes)
            {
                World = mesh.ParentBone.Transform* Matrix.CreateScale(0.04f) * Matrix.CreateTranslation(new Vector3(-10f, -18f, 7f));
                //asigno color verde amarillo 
                Effect.Parameters["DiffuseColor"].SetValue(Color.Aqua.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            //MONEDAS poner aca las ubicaciones despues
            foreach (var mesh in Moneda.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateRotationZ(totalGameTime) * Matrix.CreateTranslation(new Vector3(-87.5f, 15f, 65f));
                Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.Gold.ToVector3());
                mesh.Draw();
            }
            //moneda -43.5f, 20f + MathF.Cos(totalGameTime * 2), 55f
            //10, -10 + MathF.Cos(totalGameTime * 2), 30
            //25, -14, 30
            //37, -5, 30
            //53, -10, 30
            //63, -10, 30
            //35f, -20f, 140f
            //50, -13, 140
            //55, -14, 140
            //45, -16, 140
            //40, -14, 140
            //35, -14, 140
            //27.5f, -12.5f, 140
            //22.5f, -12.5f, 140
            //4f, -12f, 145
            //4f, -8f, 145
            //4f, -5f, 145
            //7f, -12f, 137.5f
            //-17.5f, -12f, 122.5f
            //-22.5f, -12f, 117.5f
            //-27.5f, -7f, 115f
            //-27.5f, -2f, 115f
            //-27.5f, 1f, 115f
            //-37.5f, -2f, 112.5f
            //-37.5f, 2f, 112.5f
            //-42.5f, 0f, 110f
            //-45f, -3f, 110f
            //-48f, -6f, 108f
            //-47.5f, -12.5f, 108f
            //-52.5f, -2.5f, 105f
            //-52.5f, 0f, 105f
            //-52.5f, 2.5f, 105f
            //-57.5f, -2.5f, 107.5f
            //-67.5f, 5f, 100f
            //-67.5f, 0f, 100f
            //-72.5f, 0f, 97.5f
            //-77.5f, 0f, 92.5f
            //-77.5f, 5f, 92.5f
            //-77.5f, 7.5f, 92.5f
            //-87.5f, 15f, 79f
            //-87.5f, 15f, 75f
            //-87.5f, 15f, 70f

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

// idea obstaculo: El cartel puede ser un obstaculo que si lo hacemos rotar el jugador tendria que evitarlo