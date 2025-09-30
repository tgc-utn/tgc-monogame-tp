using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP;

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
    
    private readonly GraphicsDeviceManager _graphics;

    private Effect _effect;
    private Model _model;
    private Matrix _projection;
    private float _rotation;
    private SpriteBatch _spriteBatch;
    private Matrix _view;
    private Matrix _world;
    
    private VertexBuffer _vertexBuffer; // Como son mis vertices
    private IndexBuffer _indexBuffer; // Como forman triangulos

    private Matrix[] _translations;
    private float[] _angles;
    private Color[] _colors;

    /// <summary>
    ///     Constructor del juego.
    /// </summary>
    public TGCGame()
    {
        // Maneja la configuracion y la administracion del dispositivo grafico.
        _graphics = new GraphicsDeviceManager(this);

        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;

        // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
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
        // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

        // Apago el backface culling.
        // Esto se hace por un problema en el diseno del modelo del logo de la materia.
        // Una vez que empiecen su juego, esto no es mas necesario y lo pueden sacar.
        var rasterizerState = new RasterizerState();
        rasterizerState.CullMode = CullMode.None;
        GraphicsDevice.RasterizerState = rasterizerState;
        // Seria hasta aca.

        // Configuramos nuestras matrices de la escena.
        _world = Matrix.Identity;
        // _view = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
        _view = Matrix.CreateLookAt(new Vector3(10f, 10f, 20f), Vector3.Zero, Vector3.Up);
        // _projection =
        //     Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);
        _projection =
            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.1f, 2500f);

        Random random = new Random();

        _translations = new Matrix[100];
        _colors = new Color[100];
        _angles =  new float[100];
        
        for (int i = 0; i < 100; i++)
        {
            _translations[i] = Matrix.CreateTranslation(
                ScaleRange(random.NextSingle(), -10f, 10f),
                ScaleRange(random.NextSingle(), -10f, 10f),
                random.NextSingle() * 0.1f);
                // 0f); // Esto produce Z-Fighting
            _colors[i] = new Color(new Vector3(random.NextSingle(), random.NextSingle(), random.NextSingle()));
            _angles[i] = ScaleRange(random.NextSingle(), 0f, MathF.PI * 2f);
        }

        base.Initialize();
    }

    private float ScaleRange(float value, float min, float max)
    {
        return value * (max - min) + min;
    }

    /// <summary>
    ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
    ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el procesamiento
    ///     que podemos pre calcular para nuestro juego.
    /// </summary>
    protected override void LoadContent()
    {
        // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Cargo el modelo del logo.
        _model = Content.Load<Model>(ContentFolder3D + "tgc-logo/tgc-logo");

        // Cargo un efecto basico propio declarado en el Content pipeline.
        // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
        _effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

        // Asigno el efecto que cargue a cada parte del mesh.
        // Un modelo puede tener mas de 1 mesh internamente.
        foreach (var mesh in _model.Meshes)
        {
            // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
            foreach (var meshPart in mesh.MeshParts)
            {
                meshPart.Effect = _effect;
            }
        }

        // El QUAD tienen 4 vértices
        var vertices = new VertexPosition[4]
        {
            new VertexPosition(new Vector3(-1f, -1f, 0f)),
            new VertexPosition(new Vector3(-1f, 1f, 0f)),
            new VertexPosition(new Vector3(1f, -1f, 0f)),
            new VertexPosition(new Vector3(1f, 1f, 0f))
            // new (new Vector3(-1f, -1f, 0f)),
            // new (new Vector3(-1f, 1f, 0f)),
            // new (new Vector3(1f, -1f, 0f)),
            // new (new Vector3(1f, 1f, 0f))
        };
        
        // Creando un VertxBuffer
        _vertexBuffer = new VertexBuffer(
            GraphicsDevice, // Clase que se usa en todo Monogame para crear cosas de gráficos
            VertexPosition.VertexDeclaration, // De que TIPO es el vértice
            vertices.Length, // Cantidad de vértices (El Quad tienen 4 vértices)
            BufferUsage.None // Este parámetro siempre es "None"
            );
        // Rellenando los datos del VertexBuffer
        _vertexBuffer.SetData(vertices); // A este VertexBuffer le voy a asignar los datos
        
        // Creando IndexBuffer
        var indices = new ushort[]
        {
            0, 1, 2, // Triangulo 1 (Horario)
            // 0, 2, 1, // Triangulo 1 (Antihorario)
            3, 2, 1  // Triangulo 2 (Horario)
            // 1, 2, 3  // Triangulo 2 (Antihorario)
        };
        _indexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.None);
        _indexBuffer.SetData(indices);

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

        // Basado en el tiempo que paso se va generando una rotacion.
        _rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

        _world = Matrix.CreateRotationY(_rotation);

        for (int i = 0; i < 100; i++)
        {
            _angles[i] += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
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
        GraphicsDevice.Clear(Color.Black);

        /*
        // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
        _effect.Parameters["View"].SetValue(_view);
        _effect.Parameters["Projection"].SetValue(_projection);
        _effect.Parameters["DiffuseColor"].SetValue(Color.DarkBlue.ToVector3()); // Estamos dibujándolo de azul

        foreach (var mesh in _model.Meshes)
        {
            _effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _world);
            mesh.Draw();
        }
        */
        
        // Esto prende el BackFace Culling ======================================
        // Que es una técnica para descartar triángulos que miran para el otro lado.
        //==========================================================================
        // >>Descarta: Triángulos en sentido Antihorario - Cuando estás fuera del objeto (Normal)
        // GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise; // Antihorario
        // =========================================================================
        
        
        // Alpha Blending ==========================================================
        // Para aplicar transparencias - Se necesita mas pasos en el video - Clase 04
        // GraphicsDevice.BlendState = BlendState.AlphaBlend;
        // =========================================================================
        
        // Como dibujar un modelo de la forma mas primitiva posible
        GraphicsDevice.SetVertexBuffer(_vertexBuffer);
        GraphicsDevice.Indices = _indexBuffer;
        
        _effect.Parameters["View"].SetValue(_view);
        _effect.Parameters["Projection"].SetValue(_projection);
        _effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());

        for (int i = 0; i < 100; i++)
        {
            _effect.Parameters["World"].SetValue(Matrix.CreateRotationY(_angles[i]) * _translations[i]);
            _effect.Parameters["DiffuseColor"].SetValue(_colors[i].ToVector3());
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
            }
        }
        
        // _effect.Parameters["World"].SetValue(Matrix.CreateTranslation(0f, 3f, 0f));
        // _effect.Parameters["DiffuseColor"].SetValue(Color.Green.ToVector3());
        //
        // foreach (var pass in _effect.CurrentTechnique.Passes)
        // {
        //     pass.Apply();
        //     GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
        // }
        
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