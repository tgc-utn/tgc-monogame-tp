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

   
        public void LoadContent(ContentManager Content)
        {

            Tanque = Content.Load<Model>(ContentFolder3D + "Panzer/Panzer");

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
            MoveTankBoundingBox(Direction);

            if (keyboardState.IsKeyDown(Keys.S))
                Direction -= PanzerMatrix.Backward * TankVelocity * time;
            MoveTankBoundingBox(Direction);

            if (keyboardState.IsKeyDown(Keys.D))
                Rotation -= TankRotation * time;

            if (keyboardState.IsKeyDown(Keys.A))
                Rotation -= -TankRotation * time;

            // Actualizar la rotación de la torreta
            if (keyboardState.IsKeyDown(Keys.LeftShift))
                GunRotationFinal -= GunRotation * time;

            if (keyboardState.IsKeyDown(Keys.LeftControl))
                GunRotationFinal += GunRotation * time;

            this.Position = Direction + new Vector3(0, 400f, 0f);

            PanzerMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Rotation)) * Matrix.CreateTranslation(Direction);

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
            Effect.Parameters["DiffuseColor"].SetValue(Color.Blue.ToVector3());
            foreach (var mesh in Tanque.Meshes)
            {
                Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * PanzerMatrix);
                mesh.Draw();
            }

            /*
            var originalRasterizerState = _GraphicsDevice.RasterizerState;
            foreach (var mesh in GameModel.Meshes)
            {
                var rasterizerState = new RasterizerState();
                if (mesh.Name.Equals("Gun"))
                    rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
                else
                    rasterizerState.CullMode = CullMode.CullClockwiseFace;
                _GraphicsDevice.RasterizerState = rasterizerState;
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                } //sacar el basic effect
                mesh.Draw();


            */

            /*
            Creo que tendria que ser algo asi para la torreta, habria que ver como implementar una matriz que este pegada a otra.
            foreach (BasicEffect effect in mesh.Effects)
            {
                if (mesh.Name.Equals("Gun"))
                {
                    Matrix gunWorld = Matrix.CreateRotationY(MathHelper.ToRadians(GunRotationFinal)) * world;
                    effect.World = gunWorld;
                }
                else
                {
                    effect.World = world;
                }
                effect.View = view;
                effect.Projection = projection;
            }
            */

        }

        public Projectile Shoot(Matrix TankMatrix)
        {
            if (timeSinceLastShot >= fireRate)
            {
                Matrix projectileMatrix = Matrix.CreateTranslation(new Vector3(0, 250, 600)) * TankMatrix;

                float projectileScale = 0.1f; // Ajusta esta escala según tus necesidades


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
    }
}
