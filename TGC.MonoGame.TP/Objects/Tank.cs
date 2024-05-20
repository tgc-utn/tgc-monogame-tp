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

        public Vector3 PanzerPosition { get; set; }

        public TargetCamera PanzerCamera { get; set; }

        public Matrix PanzerMatrix { get; set; }

        Vector3 Direction = new Vector3(0, 0, 0);

        public float Rotation = 0;
        public BoundingBox TankBox { get; set; }
        public float TankVelocity { get; set; }
        public float TankRotation { get; set; }

        private GraphicsDevice graphicsDevice;
        public List<ModelBone> Bones { get; private set; }
        public List<ModelMesh> Meshes { get; private set; }

        private float fireRate = 0.5f; // Tiempo mínimo entre disparos en segundos
        private float timeSinceLastShot = 0f;
        public float GunRotationFinal = 0;
        public float GunRotation { get; set; }

        public Matrix turretWorld { get; set; }
        public Matrix cannonWorld { get; set; }

        public Tank(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            turretWorld = Matrix.Identity;
            cannonWorld = Matrix.Identity;
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


        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastShot += time;

            if (keyboardState.IsKeyDown(Keys.W))
                Direction -= PanzerMatrix.Forward * TankVelocity * time;

            if (keyboardState.IsKeyDown(Keys.S))
                Direction += PanzerMatrix.Forward * TankVelocity * time;

            if (keyboardState.IsKeyDown(Keys.D))
                Rotation += TankRotation * time;

            if (keyboardState.IsKeyDown(Keys.A))
                Rotation -= TankRotation * time;

            GunRotationFinal = -GetRotationFromCursorX() + Rotation;
            float gunElevation = GetElevationFromCursorY();

            this.Position = Direction + new Vector3(0, 400f, 0f);
            PanzerMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Rotation)) * Matrix.CreateTranslation(Direction);
            turretWorld = Matrix.CreateRotationY(GunRotationFinal) * Matrix.CreateTranslation(Position);
            cannonWorld = Matrix.CreateScale(100f) * Matrix.CreateRotationX(gunElevation) * turretWorld;
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
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * turretWorld);
                }
                else if (mesh.Name.Equals("Cannon"))
                {
                    //Effect.Parameters["DiffuseColor"].SetValue(Color.Coral.ToVector3());
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * cannonWorld);
                }
                else
                {
                    Effect.Parameters["ModelTexture"].SetValue(PanzerTexture);

                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * PanzerMatrix);
                }
                mesh.Draw();
            }
        }

        public Projectile Shoot(Matrix TankMatrix)
        {
            if (timeSinceLastShot >= fireRate)
            {
                Matrix projectileMatrix = Matrix.CreateTranslation(new Vector3(0, 250, 600)) * TankMatrix;

                float projectileScale = 0.3f; // Ajusta esta escala según tus necesidades


                Projectile projectile = new Projectile(projectileMatrix, 50000f, projectileScale); // Crear el proyectil con la posición y dirección correcta

                timeSinceLastShot = 0f;

                return projectile;
            }
            else
            {
                return null;
            }
        }

        public BoundingBox MoveTankBoundingBox(Vector3 increment)
        {
            // Update its Bounding Box, moving both min and max positions
            TankBox = new BoundingBox(TankBox.Min + increment, TankBox.Max + increment);
            return TankBox;
        }


        private float GetRotationFromCursorX()
        {
            MouseState mouseState = Mouse.GetState();
            float mouseX = mouseState.X;
            float screenWidth = graphicsDevice.Viewport.Width;
            return MathHelper.ToRadians((mouseX / screenWidth) * 360f - 180f);
        }

        private float GetElevationFromCursorY()
        {
            MouseState mouseState = Mouse.GetState();
            float mouseY = mouseState.Y;
            float screenHeight = graphicsDevice.Viewport.Height;
            return MathHelper.ToRadians((mouseY / screenHeight) * 180f - 90f);
        }

    }
}
