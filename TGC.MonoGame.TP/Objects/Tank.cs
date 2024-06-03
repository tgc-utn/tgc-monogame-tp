using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using ThunderingTanks.Cameras;
using ThunderingTanks.Collisions;

namespace ThunderingTanks.Objects
{
    public class Tank : GameObject
    {

        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";

        private Effect Effect { get; set; }

        public Model Tanque { get; set; }

        private Texture2D PanzerTexture { get; set; }

        public Vector3 LastPosition { get; set; }

        public TargetCamera PanzerCamera { get; set; }

        public Matrix PanzerMatrix { get; set; }

        public Vector3 Direction = new Vector3(0, 0, 0);

        public float Rotation = 0;
        public OrientedBoundingBox TankBox { get; set; }
        public Vector3 MinBox = new Vector3(-184, 0, -334);
        public Vector3 MaxBox = new Vector3(184, 286, 658);
        public float TankVelocity { get; set; }
        public float TankRotation { get; set; }
        public bool isColliding { get; set; } = false;

        private GraphicsDevice graphicsDevice;
        public List<ModelBone> Bones { get; private set; }
        public List<ModelMesh> Meshes { get; private set; }
        public float FireRate { get; set; }
        private float TimeSinceLastShot = 0f;

        public float screenHeight;
        public float screenWidth;

        public float GunRotationFinal = 0;

        public float GunRotation { get; set; }
        public float GunElevation { get; set; }

        public Matrix TurretMatrix { get; set; }
        public Matrix CannonMatrix { get; set; }

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
            var BoundingBox = new BoundingBox(MinBox, MaxBox);
            Console.WriteLine($"Colisión detectada con roca en índice {BoundingBox}");

            TankBox = OrientedBoundingBox.FromAABB(BoundingBox);
            Console.WriteLine($"Colisión detectada con roca en índice {TankBox}");
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {

            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeSinceLastShot += time;

            //if(isColliding)
            //TankVelocity = TankVelocity * 0.01f;

            if (keyboardState.IsKeyDown(Keys.W) && !isColliding)
                Direction -= PanzerMatrix.Forward * TankVelocity * time;

            if (keyboardState.IsKeyDown(Keys.S) && !isColliding)
                Direction += PanzerMatrix.Forward * TankVelocity * time;

            if (keyboardState.IsKeyDown(Keys.D) && !isColliding)
                Rotation -= TankRotation * time;

            if (keyboardState.IsKeyDown(Keys.A) && !isColliding)
                Rotation += TankRotation * time;

            GunRotationFinal -= GetRotationFromCursorX();
            GunElevation += GetElevationFromCursorY();

            Mouse.SetPosition((int)screenWidth / 2, (int)screenHeight / 2);

            Position = Direction + new Vector3(0f, 500f, 0f);

            PanzerMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Rotation)) * Matrix.CreateTranslation(Direction);
            TurretMatrix = Matrix.CreateRotationY(GunRotationFinal) * Matrix.CreateTranslation(Direction);
            CannonMatrix = Matrix.CreateScale(100f) * Matrix.CreateRotationX(GunElevation) * TurretMatrix;

            // Mover bounding box en base a los movimientos del tanque
            var BoundingBox = new BoundingBox(MinBox + Direction, MaxBox + Direction);
            TankBox = OrientedBoundingBox.FromAABB(BoundingBox);

            LastPosition = Direction;

            Console.WriteLine(TankBox.Extents);
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

        // ------------ FUNCTIONS ------------ //

        public Projectile Shoot()
        {
            if (TimeSinceLastShot >= FireRate)
            {
                Matrix projectileMatrix = Matrix.CreateTranslation(new Vector3(0f, 20f, 0f)) * Matrix.CreateRotationX(GunElevation) * TurretMatrix;

                float projectileScale = 50f;

                Projectile projectile = new(projectileMatrix, GunRotationFinal, 50000f, projectileScale); // Crear el proyectil con la posición y dirección correcta

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