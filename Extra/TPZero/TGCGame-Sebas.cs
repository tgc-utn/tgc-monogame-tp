using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Content.Models;

using System.Linq;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Clase principal del juego.
    /// </summary>
    public class TGCGame : Game
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        private GraphicsDeviceManager Graphics { get; }
        private FollowCamera FollowCamera { get; set; }
        private CityScene City { get; set; }
        //Car Properties
        private Model CarModel { get; set; }
        private Effect CarEffect { get; set; }
        private Vector3 CarPosition { get; set; }
        private Vector3 CarVelocity { get; set; }
        private Quaternion CarRotation { get; set; }
        private Vector3 JumpPower { get; set; }   
        private Matrix CarWorld { get; set; }

        public TGCGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = BlendState.Opaque;

            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();

            FollowCamera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);

            CarPosition = Vector3.Zero;
            CarRotation = Quaternion.Identity;
            CarWorld = Matrix.Identity;
            CarVelocity = Vector3.Zero;
            JumpPower = Vector3.Zero;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            City = new CityScene(Content);

            // La carga de contenido debe ser realizada aca.
            ////////////////////////////
            CarModel = Content.Load<Model>(ContentFolder3D + "scene/car");
            CarEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            var effect = CarModel.Meshes.FirstOrDefault().Effects.FirstOrDefault() as BasicEffect;
            var texture = effect.Texture;

            CarEffect.Parameters["ModelTexture"].SetValue(texture);

            foreach (var mesh in CarModel.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = CarEffect;
            ////////////////////////////
            
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // La logica debe ir aca.
            ////////////////////////////
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // ACELERACION
            float accelerationSign = 0f;
            if (keyboardState.IsKeyDown(Keys.W))
                accelerationSign = 1f;
            else if (keyboardState.IsKeyDown(Keys.S))
                accelerationSign = -0.5f; //Reversa mas lenta

            // GIRO
            float turning = 0f;
            if (keyboardState.IsKeyDown(Keys.A))
                turning += 1f;
            else if (keyboardState.IsKeyDown(Keys.D))
                turning -= 1f;
            CarRotation *= Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(turning * 0.5f * dTime));

            Vector3 Forward = new Vector3(
                2 * (CarRotation.X * CarRotation.Z + CarRotation.W*CarRotation.Y),
                2 * (CarRotation.Y*CarRotation.Z - CarRotation.W*CarRotation.X),
                1 - 2 * (CarRotation.X*CarRotation.X + CarRotation.Y*CarRotation.Y)
            );

            Vector3 CarAcceleration = Forward * accelerationSign * -550f;

            // VELOCIDAD FINAL: VF = (A * T) + V0
            CarVelocity += CarAcceleration * dTime;
            
            // DESPLAZAMIENTO: X = V0 * T + (A * T^2) / 2
            CarPosition += CarVelocity * dTime + (CarAcceleration * dTime * dTime) / 2;

            // ROZAMIENTO
            float u = -1.55f; //Coeficiente de Rozamiento
            if (keyboardState.IsKeyDown(Keys.LeftShift)) // LShift para Frenar
                u*=2;
            CarVelocity += new Vector3(CarVelocity.X, 0, CarVelocity.Z) * u * dTime;

            // GRAVEDAD
            float g = 2000f;
            float floor = 0f;
            CarVelocity -= Vector3.Up * g * dTime;
            if(CarPosition.Y<floor)
                CarPosition = new Vector3(CarPosition.X, floor, CarPosition.Z);

            // SALTO
            if (keyboardState.IsKeyDown(Keys.Space) && CarPosition.Y==0)
            {
                CarVelocity = new Vector3(CarVelocity.X, 0, CarVelocity.Z);
                JumpPower = Vector3.Up * 800f;
            }
            CarPosition += JumpPower * dTime;

            // MATRIZ DE MUNDO
            CarWorld = 
                Matrix.CreateFromQuaternion(CarRotation) * 
                Matrix.CreateTranslation(CarPosition);
            ////////////////////////////
            FollowCamera.Update(gameTime, CarWorld);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            City.Draw(gameTime, FollowCamera.View, FollowCamera.Projection);

            // El dibujo del auto debe ir aca.
            ////////////////////////////
            
            var modelMeshesBaseTransforms = new Matrix[CarModel.Bones.Count];
            CarModel.CopyAbsoluteBoneTransformsTo(modelMeshesBaseTransforms);
            
            foreach (var mesh in CarModel.Meshes)
            {
                var meshWorld = modelMeshesBaseTransforms[mesh.ParentBone.Index];

                CarEffect.Parameters["World"].SetValue(meshWorld * CarWorld);
                mesh.Draw();
            }
            
            ////////////////////////////
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}