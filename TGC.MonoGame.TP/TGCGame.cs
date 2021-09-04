using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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


        private Effect Effect { get; set; }
        private BasicEffect BasicEffect { get; set; }
        private float Rotation { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        public VertexBuffer Vertices { get; private set; }
        public IndexBuffer Indices { get; private set; }

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

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de renderizado del juego.
            GraphicsDevice.Clear(Color.Black);

            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
            Effect.Parameters["View"].SetValue(View);
            Effect.Parameters["Projection"].SetValue(Projection);
            
            var rotationMatrix = Matrix.CreateRotationY(Rotation);

            //Se agrega el cartel
            foreach (var mesh in Cartel.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.1f)  * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(new Vector3(50f, -10f, 0f));
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
                   //asigno color rojo
                Effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
                mesh.Draw();
            }
            foreach (var mesh in Esfera.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.02f) * Matrix.CreateTranslation(new Vector3(0f, -18f, 20f));
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
            foreach (var mesh in TunnelChico.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(0.005f) * Matrix.CreateTranslation(new Vector3(-20f, -20f, 0f));
                Effect.Parameters["World"].SetValue(World);
                //asigno color salmon
                Effect.Parameters["DiffuseColor"].SetValue(Color.Salmon.ToVector3());
                mesh.Draw();
            }

            //Se agregan cubos
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(2f, 20f, 2f) * Matrix.CreateTranslation(new Vector3(0f, -10f, -40f));
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            foreach (var mesh in Cubo.Meshes)
            {
                World = mesh.ParentBone.Transform * Matrix.CreateScale(20f, 1f, 1f) * Matrix.CreateRotationY(MathHelper.ToRadians(45f)) * Matrix.CreateTranslation(new Vector3(-40f, -18f, 0f));
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
            // Para el piso
            // Set our vertex buffer.
            GraphicsDevice.SetVertexBuffer(Vertices);

            // Set our index buffer
            GraphicsDevice.Indices = Indices;

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