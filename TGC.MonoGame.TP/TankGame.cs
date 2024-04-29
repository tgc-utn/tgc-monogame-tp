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

            tanque = new Tanque();
            Textures = new List<Texture2D>();

            var Random = new Random();

            for(int i = 0; i < 50; i++)
            {
                NumerosX.Add(Random.Next(-100, 100));
                NumerosZ.Add(Random.Next(-100, 100));
            }

            base.Initialize();

        }

    protected override void LoadContent()
    {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            ground.Model = Content.Load<Model>(ContentFolder3D + "Grid/ground");

            Casa = Content.Load<Model>(ContentFolder3D + "Casa/house");

            Antitanque = Content.Load<Model>(ContentFolder3D + "assets militares/rsg_military_antitank_hedgehog_01");

            tanque.LoadContent(Content, ContentFolder3D + "Panzer/Panzer");

            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            base.LoadContent();

        }

        protected override void Update(GameTime gameTime)
        {

            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            lastGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            tanque.Update(currentGamePadState, currentKeyboardState);

            gameCamera.Update(tanque.ForwardDirection, tanque.Position, Graphics.GraphicsDevice.Viewport.AspectRatio);

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

            Matrix worldMatrix;
            Matrix worldMatrixAntitanque;
            Matrix scaleMatrix = Matrix.CreateScale(5f, 5f, 5f);

            worldMatrix = scaleMatrix * Matrix.CreateTranslation(startPosition);
            
            foreach (ModelMesh mesh in Casa.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = worldMatrix;
                    effect.View = gameCamera.ViewMatrix;
                    effect.Projection = gameCamera.ProjectionMatrix;

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    // Aquí podrías asignar una textura al efecto si la casa usa texturas
                    // Por ejemplo: effect.Texture = tuTextura;
                }
            }
            
            tanque.Draw(gameCamera.ViewMatrix, gameCamera.ProjectionMatrix);

            Effect.Parameters["View"].SetValue(gameCamera.ViewMatrix);
            Effect.Parameters["Projection"].SetValue(gameCamera.ProjectionMatrix);

            /*
                        foreach (var mesh in Casa.Meshes)
                        {
                            foreach (var meshPart in mesh.MeshParts)
                            {
                                meshPart.Effect = Effect;
                            }
                        }


                        foreach (var mesh in Antitanque.Meshes)
                        {
                            foreach (var meshPart in mesh.MeshParts)
                            {
                                meshPart.Effect = Effect;
                            }
                        }
            */

            for (int i = 0; i < 50; i++)
            {

                Vector3 vector = new Vector3(NumerosX[i], 2, NumerosZ[i]);
                
                Matrix translateMatrixAntitanque = Matrix.CreateTranslation(vector);
                worldMatrixAntitanque = scaleMatrix * translateMatrixAntitanque;

 //             Antitanque.Draw(worldMatrixAntitanque, gameCamera.ViewMatrix, gameCamera.ProjectionMatrix);
            }

            Casa.Draw(worldMatrix, gameCamera.ViewMatrix, gameCamera.ProjectionMatrix);

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















