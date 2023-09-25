﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private const int DistanciaParaArboles = 250;
        private const int DistanciaParaArbustos = 15;
        private const int DistanciaParaFlores = 15;
        private const int DistanciaParaHongos = 15;
        private const int CantidadDeArboles = 300;
        private const int CantidadDeArbustos = 500;
        private const int CantidadDeFlores = 250;
        private const int CantidadDeHongos = 250;
        private const int CantidadDeRocas = 120;

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Model Model { get; set; }
        private Model T90 { get; set; }
        private Model T90A { get; set; }
        private Model T90B { get; set; }
        private Model T90C { get; set; }
        private Model Panzer{ get; set; }
        private Effect Effect { get; set; }
        private float Rotation { get; set; }

        private List<TanqueEnemigo> Tanques { get; set; }  

        private Object Prueba { get; set; }
        private Texture2D Textura { get; set; }
        private FollowCamera FollowCamera { get; set; }

        //private Suelo Suelo {get; set;}
        private QuadPrimitive Quad { get; set; }
        private Matrix FloorWorld {get;set;}
        
        private Model roca {get; set;}
        private Object Roca {get;set;}
        private Effect EffectRoca {get;set;}
        private Texture2D TexturaRoca {get;set;}

        private List<Object> Ambiente {get;set;}

        private Tanque MainTanque {get;set;}
        

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

            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();

            Tanques = new List<TanqueEnemigo>();

            // Configuramos nuestras matrices de la escena, en este caso se realiza en el objeto FollowCamara
            FollowCamera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);

            FloorWorld = Matrix.CreateScale(10000f, 1f, 10000f);

            Ambiente = new List<Object>();

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

            // Cargo el modelo, efecto y textura del tanque que controla el jugador.
            T90 = Content.Load<Model>(ContentFolder3D + "T90");
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            Textura = Content.Load<Texture2D>(ContentFolder3D + "textures_mod/hullA");
            MainTanque = new Tanque(
                    new Vector3(0f, 150, 0f), 
                    T90, 
                    Content.Load<Effect>(ContentFolderEffects + "BasicShader"), 
                    Content.Load<Texture2D>(ContentFolder3D + "textures_mod/hullA")
                    );
            MainTanque.LoadContent();
            
            Quad = new QuadPrimitive(GraphicsDevice, Content.Load<Texture2D>(ContentFolder3D + "textures_mod/tierra"));
            
            //cargo el modelo de la roca
            roca = Content.Load<Model>(ContentFolder3D + "Rock/rock");
            // este efecto esta hecho asi nomas y solo pone las cosas verdes
            EffectRoca = Content.Load<Effect>(ContentFolderEffects + "BasicShaderRock");
            //Roca = new Object(new Vector3(0f,0f,0f), roca, EffectRoca,null);
            //Roca.LoadContent();

            //Roca.World = Matrix.CreateScale(50f) * Roca.World;
            
            
            InitializeTanks();
            InitializeAmbient();
            
            Tanques.ForEach(o => o.LoadContent());
            Ambiente.ForEach(o => o.LoadContent());

            base.LoadContent();
        }
        private void InitializeAmbient()
        {
            List<String> posiblesArboles = new(){
                "BigTree","BigTree2", "Tree", "SmallTree"
            };

            // textura de árboles se usa tanto para árboles como arbustos
            List<String> posiblesTexturasArboles = new(){
                "Tree", "Tree2", "Tree3", "Tree4", "Tree5", "Tree6", "Tree7"
            };
            
            List<String> posiblesFlores = new(){
                "Flower","Flower2", "Flower3"
            };

            List<String> posiblesTexturasFlores = new(){
                "Flower","Flower2", "Flower3", "Flower4"
            };

            List<String> posiblesArbustos = new(){
                "Bush","BigBush", "BiggerBush"
            };

            List<String> posiblesRocas = new(){
                "roca1", "roca2", "roca3", "roca4", "roca5", "roca6", "roca7", "roca8"
            };

            List<String> posiblesTexturasRocas = new(){
                "TexturaRoca1", "TexturaRoca2"
            };
            
            Vector3 posicionAmbiente;
            
            // Árboles
            for (int i = 0; i < CantidadDeArboles; i++)
            {
                posicionAmbiente = SelectNewPosition(DistanciaParaArboles, 6800);
                String arbol = posiblesArboles[new Random().Next(0, 3)];
                Ambiente.Add(
                    new Object(
                        posicionAmbiente,
                        Content.Load<Model>(ContentFolder3D + "Ambiente/" + arbol),
                        Content.Load<Effect>(ContentFolderEffects + "BasicShader"),
                        Content.Load<Texture2D>(ContentFolderTextures + posiblesTexturasArboles[new Random().Next(0, 6)]),
                        arbol.Equals("Tree") 
                        )
                    );
            }
            
            // Anillo de árboles
            for (int i = 0; i < 500; i++)
            {
                float angle = new Random().Next(0, 360);
                float delta = new Random().Next(-150, 150);
                float dist = 7000 + delta;
                posicionAmbiente = new Vector3((dist * MathF.Cos(angle)), 0, (dist * MathF.Sin(angle)));
                String arbol = posiblesArboles[new Random().Next(0, 3)];
                Ambiente.Add(
                    new Object(
                        posicionAmbiente,
                        Content.Load<Model>(ContentFolder3D + "Ambiente/" + arbol),
                        Content.Load<Effect>(ContentFolderEffects + "BasicShader"),
                        Content.Load<Texture2D>(ContentFolderTextures + posiblesTexturasArboles[new Random().Next(0, 6)]),
                        arbol.Equals("Tree") 
                        )
                    );
            }

            // Arbustos
            for (int i = 0; i < CantidadDeArbustos; i++)
            {
                posicionAmbiente = SelectNewPosition(DistanciaParaArbustos, 1);

                Ambiente.Add(
                    new Object(
                        posicionAmbiente,
                        Content.Load<Model>(ContentFolder3D + "Ambiente/" + posiblesArbustos[new Random().Next(0, 3)]),
                        Content.Load<Effect>(ContentFolderEffects + "BasicShader"),
                        Content.Load<Texture2D>(ContentFolderTextures + posiblesTexturasArboles[new Random().Next(0, 6)]),
                        true
                        )
                    );
            }

            // Flores
            for (int i = 0; i < CantidadDeFlores; i++)
            {
                posicionAmbiente = SelectNewPosition(DistanciaParaFlores, 1);

                Ambiente.Add(
                    new Object(
                        posicionAmbiente,
                        Content.Load<Model>(ContentFolder3D + "Ambiente/" + posiblesFlores[new Random().Next(0, 2)]),
                        Content.Load<Effect>(ContentFolderEffects + "BasicShader"),
                        Content.Load<Texture2D>(ContentFolderTextures + posiblesTexturasFlores[new Random().Next(0, 3)]), 
                        true
                        )
                    );
            }

            // Hongos
            for (int i = 0; i < CantidadDeHongos; i++)
            {
                posicionAmbiente = SelectNewPosition(DistanciaParaHongos, 1);

                Ambiente.Add(
                    new Object(
                        posicionAmbiente,
                        Content.Load<Model>(ContentFolder3D + "Ambiente/Mushroom"),
                        Content.Load<Effect>(ContentFolderEffects + "BasicShader"),
                        Content.Load<Texture2D>(ContentFolderTextures + "Mushroom"),
                        true)
                    );
            }

            // Rocas
            for (int i = 0; i < CantidadDeRocas; i++)
            {
                posicionAmbiente = SelectNewPosition(100, 5000);
                posicionAmbiente += Vector3.Up * 30;

                Ambiente.Add(
                    new Object(
                        posicionAmbiente,
                        Content.Load<Model>(ContentFolder3D + "Rocas/" + posiblesRocas[new Random().Next(0, 7)]),
                        Content.Load<Effect>(ContentFolderEffects + "BasicShader"),
                        Content.Load<Texture2D>(ContentFolderTextures + posiblesTexturasRocas[new Random().Next(0, 1)]),
                        false)
                    );
                Ambiente.Last().World = Matrix.CreateScale(300f) * Ambiente.Last().World;
            }
        }

        private Vector3 SelectNewPosition(int distanciaMinimaEntreObjetos, int radio)
        {
            Vector3 posicionAmbiente = new Vector3();
            int X;
            int Z;
            do
            { //que genere posiciones hasta que esté a más de lo establecido por parámetro
                X = new Random().Next(-9500, 9500);
                Z = new Random().Next(-9500, 9500);
                posicionAmbiente = new Vector3(X, 0f, Z);
            }
            while (
                Ambiente.Exists( arbol => Vector3.Distance(arbol.Position, posicionAmbiente) < distanciaMinimaEntreObjetos ) ||
                Vector3.Distance(Vector3.Zero, posicionAmbiente) < radio
                //Tanques.Exists( tanqueEnemigo => Vector3.Distance(tanqueEnemigo.Position, posicionAmbiente) < 6000 ) ||
                //Vector3.Distance(MainTanque.Position, posicionAmbiente) < 6000
                );
            return posicionAmbiente;
        }

        private void InitializeTanks()
        {
            /*for (int i = 0; i < 10; i++)
            {
                objetos3D.Add(new Object(
                    new Vector3(1000f*i, 150, 0), 
                    T90, 
                    Content.Load<Effect>(ContentFolderEffects + "BasicShader"), 
                    Content.Load<Texture2D>(ContentFolder3D + "textures_mod/hullA")));

                objetos3D.Add(new Object(
                    new Vector3(1000f*i, 150, -1000f), 
                    T90, 
                    Content.Load<Effect>(ContentFolderEffects + "BasicShader"), 
                    Content.Load<Texture2D>(ContentFolder3D + "textures_mod/hullB")));
            }*/
            Tanques.Add(new TanqueEnemigo(new Vector3(1000f, 150, 0), T90, Content.Load<Effect>(ContentFolderEffects + "BasicShader"), Content.Load<Texture2D>(ContentFolder3D + "textures_mod/hullA")));
            Tanques.Add(new TanqueEnemigo(new Vector3(-1000f, 150, 0), T90, Content.Load<Effect>(ContentFolderEffects + "BasicShader"), Content.Load<Texture2D>(ContentFolder3D + "textures_mod/hullB")));
            Tanques.Add(new TanqueEnemigo(new Vector3(1000f, 150, 1000f), T90, Content.Load<Effect>(ContentFolderEffects + "BasicShader"), Content.Load<Texture2D>(ContentFolder3D + "textures_mod/hullC")));
            Tanques.Add(new TanqueEnemigo(new Vector3(-1000f, 150, 1000f), T90, Content.Load<Effect>(ContentFolderEffects + "BasicShader"), Content.Load<Texture2D>(ContentFolder3D + "textures_mod/mask")));
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        
        int frames = 0;
        float tiempo = 0;
        protected override void Update(GameTime gameTime)
        {
            tiempo += (float)(gameTime.ElapsedGameTime.TotalSeconds);
            frames++;
            Console.WriteLine("Frames: " + 1000f/(float)(gameTime.ElapsedGameTime.TotalMilliseconds));
            /*if(tiempo >= 1){
                Console.WriteLine("Frames: " + frames);
                frames = 0;
                tiempo = 0;
            }*/
            // Aca deberiamos poner toda la logica de actualizacion del juego.

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                Exit();
            }

            // Control del jugador
            MainTanque.Update(gameTime, Keyboard.GetState(), Ambiente, Tanques);

            // Colisión del MainTanque con el tanque principal
            /*Tanques.ForEach(TanqueEnemigoDeLista => {
                TanqueEnemigoDeLista.Update(gameTime);             // Actualiza el tanque enemigo
                MainTanque.Intersecta(TanqueEnemigoDeLista);       // Analiza la intersección con el main tanque
                TanqueEnemigoDeLista.Intersecta(Ambiente);         // Analiza la interseccion entre tanque y ambiente
            });*/
            Tanques.ForEach(TanqueEnemigoDeLista => {
                TanqueEnemigoDeLista.Update(gameTime, Ambiente);
                });
            // Colisión del MainTanque con el ambiente (plantas y rocas)
            /*Ambiente.ForEach(o => {
                MainTanque.Intersecta(o);
            });*/

            // Se eliminan los ambientes que hayan chocado
            Ambiente.RemoveAll(O => O.esVictima);
            
            

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.BlueViolet);

            //Prueba.Draw(gameTime, FollowCamera.View, FollowCamera.Projection);
            Tanques.ForEach(a => a.Draw(gameTime, FollowCamera.View, FollowCamera.Projection));
            Ambiente.ForEach(a => a.Draw(gameTime, FollowCamera.View, FollowCamera.Projection));
            //FollowCamera.Update(gameTime, objetos3D[3].World);

            //Suelo.Draw(gameTime,GraphicsDevice, FollowCamera.View, FollowCamera.Projection);
            Quad.Draw(FloorWorld, FollowCamera.View, FollowCamera.Projection);
           

            //Roca.Draw(gameTime, FollowCamera.View, FollowCamera.Projection);

            MainTanque.Draw(gameTime, FollowCamera.View, FollowCamera.Projection);
            
            FollowCamera.Update(gameTime, MainTanque.World);
            
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