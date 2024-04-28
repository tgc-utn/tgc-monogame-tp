using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP
{
    public class TankGame : Game
    {
        private GraphicsDeviceManager graphics;
        private GameObject ground;
        private Camera gameCamera;
        private Effect Effect { get; set; }
        public const string ContentFolderEffects = "Effects/";

        private Tanque tanque;
        private Model Casa { get; set; }
        private List<Texture2D> Textures {get; set; }
        //private Barrier[] barriers;

        private Vector3 startPosition = new Vector3(10, GameConstants.HeightOffset -8, 0);






        // States to store input values
        private KeyboardState lastKeyboardState = new KeyboardState();
        private KeyboardState currentKeyboardState = new KeyboardState();
        private GamePadState lastGamePadState = new GamePadState();
        private GamePadState currentGamePadState = new GamePadState();


        public TankGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            ground = new GameObject();
            gameCamera = new Camera();


            base.Initialize();

        }






    protected override void LoadContent()
    {
            ground.Model = Content.Load<Model>("Models/Grid/ground");

            Casa = Content.Load<Model>("Models/Casa/house");

            
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
            /*
            foreach (var mesh in Casa.Meshes)
            {
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                {
                    var basicEffect = ((BasicEffect)meshPart.Effect);
                    if(basicEffect.Texture != null)
                        Textures.Add(basicEffect.Texture);
                    meshPart.Effect = Effect;
                }
            }
            */



        }

        protected override void Update(GameTime gametime)
        {
        // Update input from sources, Keyboard and GamePad
        lastKeyboardState = currentKeyboardState;
        currentKeyboardState = Keyboard.GetState();
        lastGamePadState = currentGamePadState;
        currentGamePadState = GamePad.GetState(PlayerIndex.One);



            tanque.Update(currentGamePadState, currentKeyboardState);
        gameCamera.Update(tanque.ForwardDirection, tanque.Position, graphics.GraphicsDevice.Viewport.AspectRatio);
            // Allows the game to exit
            if (currentKeyboardState.IsKeyDown(Keys.Escape) || currentGamePadState.Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

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

            Matrix translateMatrix = Matrix.CreateTranslation(startPosition);

            Matrix scaleMatrix = Matrix.CreateScale(5f, 5f, 5f);

            worldMatrix = scaleMatrix * translateMatrix;

            Casa.Draw(worldMatrix, gameCamera.ViewMatrix, gameCamera.ProjectionMatrix);
            
            tanque.Draw(gameCamera.ViewMatrix, gameCamera.ProjectionMatrix);

            //Casa.Draw(World, View, Projection);

             // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
         
       



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















