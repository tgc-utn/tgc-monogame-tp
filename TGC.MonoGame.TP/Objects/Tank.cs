using BepuPhysics.Collidables;
using BepuPhysics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepuPhysics.Constraints;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using ThunderingTanks.Cameras;
using Microsoft.VisualBasic.FileIO;
using ThunderingTanks.Collisions;

namespace ThunderingTanks.Objects
{
    public class Tank : GameObject
    {

        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";
        private GraphicsDevice graphicsDevice;

        public float screenHeight;
        public float screenWidth;

        private Effect Effect { get; set; }
        public Model Tanque { get; set; }
        private Texture2D PanzerTexture { get; set; }
        public Vector3 PanzerPosition { get; set; }
        public TargetCamera PanzerCamera { get; set; }


        public List<ModelBone> Bones { get; private set; }
        public List<ModelMesh> Meshes { get; private set; }

        public float TankVelocity { get; set; }
        public float TankRotation { get; set; }

        public Vector3 Direction = new(0, 0, 0);

        public float Rotation = 0;

        public BoundingCylinder TankBox { get; set; }

        public float GunRotation { get; set; }
        public float GunElevation { get; set; }
        public float GunRotationFinal { get; set; }


        private readonly float FireRate = 0.5f;// Tiempo mínimo entre disparos en segundos

        private float TimeSinceLastShot = 0f;

        public Matrix TurretMatrix { get; set; }
        public Matrix CannonMatrix { get; set; }
        public Matrix PanzerMatrix { get; set; }

        public bool IsMoving { get; set; } = true;

        public Tank(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            TurretMatrix = Matrix.Identity;
            CannonMatrix = Matrix.Identity;
        }

        public void LoadContent(ContentManager Content)
        {

            Tanque = Content.Load<Model>(ContentFolder3D + "Panzer/Panzer");

            PanzerTexture = Content.Load<Texture2D>(ContentFolder3D + "Panzer/PzVl_Tiger_I");

            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            foreach (var mesh in Tanque.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }

            PanzerMatrix = Matrix.CreateTranslation(Position);

            TankBox = new BoundingCylinder(PanzerPosition, 10f, 20f);


        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {

            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeSinceLastShot += time;

            if (keyboardState.IsKeyDown(Keys.W))
                Direction -= PanzerMatrix.Forward * TankVelocity * time;

            if (keyboardState.IsKeyDown(Keys.S))
                Direction += PanzerMatrix.Forward * TankVelocity * time;

            if (keyboardState.IsKeyDown(Keys.D))
                Rotation -= TankRotation * time;

            if (keyboardState.IsKeyDown(Keys.A))
                Rotation += TankRotation * time;

            GunRotationFinal -= GetRotationFromCursorX();
            GunElevation += GetElevationFromCursorY();

            Mouse.SetPosition((int)screenWidth / 2, (int)screenHeight / 2);

            Position = Direction + new Vector3(0, 400f, 0f);
            PanzerMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Rotation)) * Matrix.CreateTranslation(Direction);
            TurretMatrix = Matrix.CreateRotationY(GunRotationFinal) * Matrix.CreateTranslation(Direction);
            CannonMatrix = Matrix.CreateScale(100f) * Matrix.CreateRotationX(GunElevation) * TurretMatrix;

            // Mover bounding box en base a los movimientos del tanque
            TankBox = new BoundingCylinder(Position, 10f, 20f);

            Console.WriteLine(TankBox.Center);
        }

        public void Model(GraphicsDevice graphicsDevice, List<ModelBone> bones, List<ModelMesh> meshes)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice", "The GraphicsDevice must not be null when creating new resources.");
            }

            this.graphicsDevice = graphicsDevice;
            Bones = bones;
            Meshes = meshes;
        }

        public void Draw(Matrix world, Matrix view, Matrix projection, GraphicsDevice _GraphicsDevice)
        {
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);
            //Effect.Parameters["DiffuseColor"].SetValue(Color.Blue.ToVector3());

            foreach (var mesh in Tanque.Meshes)
            {
                if (mesh.Name.Equals("Turret"))
                {
                    //Effect.Parameters["DiffuseColor"]?.SetValue(Color.Aquamarine.ToVector3());
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * TurretMatrix);
                }
                else if (mesh.Name.Equals("Cannon"))
                {
                    //Effect.Parameters["DiffuseColor"].SetValue(Color.Coral.ToVector3());
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * CannonMatrix);
                }
                else
                {
                    Effect.Parameters["ModelTexture"].SetValue(PanzerTexture);

                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * PanzerMatrix);
                }
                mesh.Draw();
            }
        }

        public Projectile Shoot()
        {
            if (TimeSinceLastShot >= FireRate)
            {
                Matrix projectileMatrix = Matrix.CreateRotationX(GunElevation) * TurretMatrix;

                float projectileScale = 100f;

                Projectile projectile = new(projectileMatrix, 50000f, projectileScale); // Crear el proyectil con la posición y dirección correcta

                TimeSinceLastShot = 0f;

                return projectile;
            }
            else
            {
                return null;
            }
        }
        private float GetRotationFromCursorX()
        {
            MouseState mouseState = Mouse.GetState();
            float mouseX = mouseState.X;
            screenWidth = graphicsDevice.Viewport.Width;
            return MathHelper.ToRadians((mouseX / screenWidth) * 360f - 180f);
        }
        private float GetElevationFromCursorY()
        {
            screenHeight = graphicsDevice.Viewport.Height;

            MouseState mouseState = Mouse.GetState();
            float mouseY = mouseState.Y;

            return MathHelper.ToRadians((mouseY / screenHeight) * 180f - 90f);
        }


    }
}
