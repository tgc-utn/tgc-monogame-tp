using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP
{
    public class TankGame : Game
    {
        private GraphicsDeviceManager Graphics;
        private GameObject ground;
        private Camera gameCamera;
        private SpriteBatch SpriteBatch { get; set; }
        private Effect Effect { get; set; }

        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        private Tanque tanque;

        private Model Antitanque { get; set; }
        private Model Casa { get; set; }
        private List<Texture2D> Textures { get; set; }

        private List<int> NumerosX { get; set; }
        private List<int> NumerosZ { get; set; }

        //private Barrier[] barriers;

        private Vector3 startPosition = new Vector3(10, GameConstants.HeightOffset - 8, 0);






        // States to store input values
        private KeyboardState lastKeyboardState = new KeyboardState();
        private KeyboardState currentKeyboardState = new KeyboardState();
        private GamePadState lastGamePadState = new GamePadState();
        private GamePadState currentGamePadState = new GamePadState();


        public TankGame()
        {
            Graphics = new GraphicsDeviceManager(this);

            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            ground = new GameObject();
            gameCamera = new Camera();

            NumerosX = new List<int>();
            NumerosZ = new List<int>();

            var Random = new Random();
<<<<<<< HEAD

            for(int i = 0; i < 50; i++)
=======
            //int Numero[] = Random.Next(1, 20);
            for (int i = 0; i < 50; i++)
>>>>>>> 616bb646442208c2824a8999664bbef9fbba3354
            {
                NumerosX.Add(Random.Next(-100, 100));
                NumerosZ.Add(Random.Next(-100, 100));
            }

            base.Initialize();

        }

<<<<<<< HEAD
    protected override void LoadContent()
    {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

=======





        protected override void LoadContent()
        {
>>>>>>> 616bb646442208c2824a8999664bbef9fbba3354
            ground.Model = Content.Load<Model>("Models/Grid/ground");

            Casa = Content.Load<Model>("Models/Casa/house");

            Antitanque = Content.Load<Model>("Models/assets militares/rsg_military_antitank_hedgehog_01");


            // Initialize and place barriers
            /* barriers = new Barrier[3];

             barriers[0] = new Barrier();
             barriers[0].LoadContent(Content, "Models/house");
             barriers[0].Position = new Vector3(0, 0, 30);*/
            /*barriers[1] = new Barrier();
            barriers[1].LoadContent(Content, "Models/house");
            barriers[1].Position = new Vector3(15, 0, 30);
            barriers[2] = new Barrier();
            barriers[2].LoadContent(Content, "Models/house");
            barriers[2].Position = new Vector3(-15, 0, 30);
            */

            // Initialize and place fuel carrier
            tanque = new Tanque();
            //tanque.LoadContent(Content, "Models/Panzer/Panzer");

            tanque.LoadContent(Content, "Models/Panzer/Panzer");
            //tanque = Content.Load<Model>("Models/Panzer/Panzer");

            //Casa = Content.Load<Model>("Models/house");

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.
            // Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            Textures = new List<Texture2D>();
            // Sacado del video de la clase de texturas
            Effect = Content.Load<Effect>("Effects/BasicShader");

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.
            foreach (var mesh in Casa.Meshes)
            {
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
            /*
            foreach (var mesh in Antitanque.Meshes)
            {
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
            */

<<<<<<< HEAD
            base.LoadContent();
=======
>>>>>>> 616bb646442208c2824a8999664bbef9fbba3354

        }

        protected override void Update(GameTime gameTime)
        {
            // Update input from sources, Keyboard and GamePad
<<<<<<< HEAD
=======
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            lastGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);
>>>>>>> 616bb646442208c2824a8999664bbef9fbba3354

            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            lastGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            tanque.Update(currentGamePadState, currentKeyboardState);
<<<<<<< HEAD
            gameCamera.Update(tanque.ForwardDirection, tanque.Position, Graphics.GraphicsDevice.Viewport.AspectRatio);
=======
            gameCamera.Update(tanque.ForwardDirection, tanque.Position, graphics.GraphicsDevice.Viewport.AspectRatio);
            // Allows the game to exit
            if (currentKeyboardState.IsKeyDown(Keys.Escape) || currentGamePadState.Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }
>>>>>>> 616bb646442208c2824a8999664bbef9fbba3354

            if (currentKeyboardState.IsKeyDown(Keys.Escape) || currentGamePadState.Buttons.Back == ButtonState.Pressed)
                {
                    this.Exit();
                }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            DrawTerrain(ground.Model);

            /*// Draw the barriers
            foreach (Barrier barrier in barriers)
            {
                barrier.Draw(gameCamera.ViewMatrix, gameCamera.ProjectionMatrix);
            }*/

            Matrix worldMatrix = Matrix.Identity;

            Matrix worldMatrixAntitanque = Matrix.Identity;

            Matrix translateMatrixCasa = Matrix.CreateTranslation(startPosition);

            Matrix scaleMatrix = Matrix.CreateScale(5f, 5f, 5f);

            worldMatrix = scaleMatrix * translateMatrixCasa;
            /*
            foreach (ModelMesh mesh in Casa.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = worldMatrix; // Define la matriz de transformación para la casa
                    effect.View = gameCamera.ViewMatrix;
                    effect.Projection = gameCamera.ProjectionMatrix;
                    // Aquí podrías asignar una textura al efecto si la casa usa texturas
                    // Por ejemplo: effect.Texture = tuTextura;
                }
                mesh.Draw();
            }
            */
            //Casa.Draw(worldMatrix, gameCamera.ViewMatrix, gameCamera.ProjectionMatrix);

            tanque.Draw(gameCamera.ViewMatrix, gameCamera.ProjectionMatrix);

            Effect.Parameters["View"].SetValue(gameCamera.ViewMatrix);
            Effect.Parameters["Projection"].SetValue(gameCamera.ProjectionMatrix);

            for (int i = 0; i < 50; i++)
            {
<<<<<<< HEAD
=======



>>>>>>> 616bb646442208c2824a8999664bbef9fbba3354
                Vector3 vector = new Vector3(NumerosX[i], 2, NumerosZ[i]);
                
                Matrix translateMatrixAntitanque = Matrix.CreateTranslation(vector);
                worldMatrixAntitanque = scaleMatrix * translateMatrixAntitanque;
<<<<<<< HEAD

=======
                /*
                foreach (var mesh in Casa.Meshes)
                {
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * worldMatrixAntitanque);
                    mesh.Draw();
                }
                */
>>>>>>> 616bb646442208c2824a8999664bbef9fbba3354
                Antitanque.Draw(worldMatrixAntitanque, gameCamera.ViewMatrix, gameCamera.ProjectionMatrix);
            }

            //Casa.Draw(World, View, Projection);

<<<<<<< HEAD
             // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
         
=======
            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
            
            foreach (var mesh in Casa.Meshes)
            {
                Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * worldMatrix);
                mesh.Draw();
            }


>>>>>>> 616bb646442208c2824a8999664bbef9fbba3354
            base.Draw(gameTime);
        }

        private void DrawTerrain(Model model)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = Matrix.Identity;

                    // Use the matrices provided by the game camera
                    effect.View = gameCamera.ViewMatrix;
                    effect.Projection = gameCamera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }

        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();

            base.UnloadContent();
        }


    }




}















