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

namespace ThunderingTanks
{
    public class Tank : GameObject
    {
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

        public Matrix Update(GameTime gameTime, KeyboardState keyboardState, Matrix TankMatrix)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastShot += time;

            if (keyboardState.IsKeyDown(Keys.W))
                Direction -= TankMatrix.Forward * TankVelocity * time;
            MoveTankBoundingBox(Direction);

            if (keyboardState.IsKeyDown(Keys.S))
                Direction -= TankMatrix.Backward * TankVelocity * time;
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

            TankMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Rotation)) * Matrix.CreateTranslation(Direction);

            return TankMatrix;
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
            _GraphicsDevice.RasterizerState = originalRasterizerState;
        }

        public Projectile Shoot(Matrix TankMatrix)
        {
            if (timeSinceLastShot >= fireRate)
            {
                Matrix projectileMatrix = Matrix.CreateTranslation(new Vector3(0, 250, 600)) * TankMatrix;

                Projectile projectile = new Projectile(projectileMatrix, 50000f); // Crear el proyectil con la posición y dirección correcta

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
