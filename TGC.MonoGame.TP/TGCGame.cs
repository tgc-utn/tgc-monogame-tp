using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.Obstacles;

namespace TGC.MonoGame.TP
{
    public class TGCGame : Game
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        public TGCGame()
        {
            Graphics = new GraphicsDeviceManager(this);

            Graphics.PreferredBackBufferWidth =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private GraphicsDeviceManager Graphics;

        private Effect Effect;
        private Matrix View;
        private Matrix Projection;

        private float CameraAngle = -MathF.PI / 2;
        private float CameraRotationSpeed = 5f;
        private float CameraDistanceToPlayer = 7f;
        private float CameraUpAngle = 0;
        private Vector3 GetCameraPosition(float angle)
        {
            return new Vector3(MathF.Cos(angle) * CameraDistanceToPlayer, 3,
                               MathF.Sin(angle) * CameraDistanceToPlayer);
        }

        private SpherePrimitive Sphere;
        private CubePrimitive Cube;
        private CylinderPrimitive Cylinder;
        private PyramidPrimitive Pyramid;
        private TeapotPrimitive teapot;
        private CylinderPrimitive cylinder;

        private Vector3 PlayerPosition = Vector3.Zero;
        private float PlayerSpeed = 4f;
        private Matrix PlayerWorld;

        private Elevator Elevator;
        private Pendulum Pendulum;
        private Pendulum Pendulum2;
        private Pendulum Pendulum3;

        private TrianglePrimitive triangle;

        private int PlayerRadius = 1;
        private int FloorUnitHeight = 6;

        private float Yaw { get; set; }
        private float Pitch { get; set; }
        private float Roll { get; set; }

        protected override void Initialize()
        {
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            PlayerWorld = Matrix.Identity;
            View = Matrix.CreateLookAt(GetCameraPosition(CameraAngle) + PlayerPosition,
                                       PlayerPosition + Vector3.UnitY * CameraUpAngle,
                                       Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Sphere = new SpherePrimitive(GraphicsDevice);
            Cube = new CubePrimitive(GraphicsDevice);
            Cylinder = new CylinderPrimitive(GraphicsDevice);
            Pyramid = new PyramidPrimitive(GraphicsDevice, new Vector3(0, 1, 0),
                                           new Vector3[] {
                                     new Vector3(-1, 0, -1),
                                     new Vector3(1, 0, -1),
                                     new Vector3(1, 0, 1),
                                     new Vector3(-1, 0, 1),
                                           });
            triangle = new TrianglePrimitive(
                GraphicsDevice, new Vector3(-1f, 1f, 1f), new Vector3(0f, 2f, 1f),
                new Vector3(1f, 1f, 1f), Color.Black, Color.Cyan, Color.Magenta);
            teapot = new TeapotPrimitive(GraphicsDevice, 1);
            cylinder = new CylinderPrimitive(GraphicsDevice, 10, 7, 8);

            Elevator = new Elevator(Cube, -Vector3.UnitY, 1, 2, Color.Green, 5);

            float starting_angle = MathF.PI / 4;
            Pendulum = new Pendulum(Cylinder, Sphere, new Vector3(0, 11, 3), 0,
                                    starting_angle, starting_angle, -starting_angle, 10,
                                    3, Color.DarkGreen, Color.Yellow, 0.5f);
            Pendulum2 = new Pendulum(Cylinder, Sphere, new Vector3(0, 11, 10), 0,
                                     -starting_angle, starting_angle, -starting_angle,
                                     10, 3, Color.Blue, Color.White, 0.5f);
            Pendulum3 =
                new Pendulum(Cylinder, Sphere, new Vector3(0, 5, 0), 0, 0, MathF.PI * 2,
                             0, 7, 5, Color.Gray, Color.Orange, 1.5f);

            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {

            float dt = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.W))
                PlayerPosition.Z += PlayerSpeed * dt;

            if (keyboardState.IsKeyDown(Keys.S))
                PlayerPosition.Z -= PlayerSpeed * dt;

            if (keyboardState.IsKeyDown(Keys.D))
                PlayerPosition.X -= PlayerSpeed * dt;

            if (keyboardState.IsKeyDown(Keys.A))
                PlayerPosition.X += PlayerSpeed * dt;

            if (keyboardState.IsKeyDown(Keys.Space))
                PlayerPosition.Y += PlayerSpeed * dt;

            if (keyboardState.IsKeyDown(Keys.LeftShift))
                PlayerPosition.Y -= PlayerSpeed * dt;

            // Movimiento de la cámara con las flechas para facilidad de ver las cosas
            if (keyboardState.IsKeyDown(Keys.Up))
                CameraUpAngle += CameraRotationSpeed * dt;

            if (keyboardState.IsKeyDown(Keys.Down))
                CameraUpAngle -= CameraRotationSpeed * dt; // Mover la cámara hacia atrás

            if (keyboardState.IsKeyDown(Keys.Left))
                CameraAngle +=
                    CameraRotationSpeed * dt; // Mover la cámara hacia la izquierda

            if (keyboardState.IsKeyDown(Keys.Right))
                CameraAngle -=
                    CameraRotationSpeed * dt; // Mover la cámara hacia la derecha

            base.Update(gameTime);

            Elevator.Update(dt);

            Pendulum.Update(dt);
            Pendulum2.Update(dt);

            Pendulum3.Position.Z += dt;
            Pendulum3.RotationAngle += dt * 0.1f;
            Pendulum3.Update(dt);

            View = Matrix.CreateLookAt(GetCameraPosition(CameraAngle) + PlayerPosition,
                                       PlayerPosition + Vector3.UnitY * CameraUpAngle,
                                       Vector3.Up);

            PlayerWorld = Matrix.CreateTranslation(PlayerPosition);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            DrawGeometry(teapot, new Vector3(2, 0, 10), Yaw, -Pitch, Roll);

            Effect.Parameters["World"].SetValue(PlayerWorld);
            Effect.Parameters["View"].SetValue(View);
            Effect.Parameters["Projection"].SetValue(Projection);
            Effect.Parameters["DiffuseColor"].SetValue(Color.DarkBlue.ToVector3());
            Sphere.Draw(Effect);

            Effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
            Matrix initial_floor = Matrix.Identity;
            for (int i = 0; i < 10; i++)
            {
                Matrix floor_world =
                    Matrix.CreateScale(FloorUnitHeight) *
                    Matrix.CreateTranslation(new Vector3(
                        0, -PlayerRadius - (FloorUnitHeight) / 2, i * FloorUnitHeight)) *
                    initial_floor;
                Effect.Parameters["World"].SetValue(floor_world);
                Cube.Draw(Effect);
            }

            Effect.Parameters["DiffuseColor"].SetValue(Color.White.ToVector3());
            Effect.Parameters["World"].SetValue(Matrix.Identity);
            Pyramid.Draw(Effect);
            Elevator.Draw(Effect);
            Pendulum.Draw(Effect);
            Pendulum2.Draw(Effect);
            Pendulum3.Draw(Effect);

            var triangleEffect = triangle.Effect;
            triangleEffect.World = Matrix.Identity;
            triangleEffect.View = View;
            triangleEffect.Projection = Projection;
            triangleEffect.LightingEnabled = false;
            triangle.Draw(triangleEffect);

            var cylinderEffect = cylinder.Effect;

            Matrix rotation = Matrix.CreateRotationX(MathHelper.PiOver2);

            cylinderEffect.World = rotation *
                                   Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll) *
                                   Matrix.CreateTranslation(new Vector3(0, 0, 30));
            cylinderEffect.View = View;
            cylinderEffect.Projection = Projection;

            cylinder.Draw(cylinderEffect);
        }

        private void DrawGeometry(GeometricPrimitive geometry, Vector3 position,
                                  float yaw, float pitch, float roll)
        {
            var effect = geometry.Effect;

            effect.World = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) *
                           Matrix.CreateTranslation(position);
            effect.View = View;
            effect.Projection = Projection;

            geometry.Draw(effect);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            Sphere.Dispose();
            Cube.Dispose();
            Cylinder.Dispose();
            Pyramid.Dispose();
            teapot.Dispose();
            cylinder.Dispose();
            base.UnloadContent();
        }
    }
}
