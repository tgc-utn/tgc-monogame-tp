﻿using BepuPhysics.Collidables;
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

        public float TankVelocity { get; set; }
        public float TankRotation { get; set; }

        private GraphicsDevice graphicsDevice;
        public List<ModelBone> Bones { get; private set; }
        public List<ModelMesh> Meshes { get; private set; }
        private List<Projectile> projectiles = new List<Projectile>();
        private float fireRate = 0.5f; // Tiempo mínimo entre disparos en segundos
        private float timeSinceLastShot = 0f;

        public Matrix Update(GameTime gameTime, KeyboardState keyboardState, Matrix TankMatrix)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastShot += time;


            if (keyboardState.IsKeyDown(Keys.W))
                Direction -= TankMatrix.Forward * TankVelocity * time;

            if (keyboardState.IsKeyDown(Keys.S))
                Direction -= TankMatrix.Backward * TankVelocity * time;

            if (keyboardState.IsKeyDown(Keys.D))
                Rotation -= TankRotation * time;

            if (keyboardState.IsKeyDown(Keys.A))
                Rotation -= -TankRotation * time;

            this.Position = Direction + new Vector3(0, 400f, 0f);
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                Shoot(TankMatrix);
            }

            UpdateProjectiles(gameTime);

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

            }
            _GraphicsDevice.RasterizerState = originalRasterizerState;
        }
        public void Shoot(Matrix TankMatrix)
        {
            if (timeSinceLastShot >= fireRate)
            {   
                Matrix projectileMatrix = TankMatrix * Matrix.CreateTranslation(new Vector3(0, 250, 600));
                Vector3 projectilePosition = projectileMatrix.Translation; // Obtener la posición del tanque desde la matriz
                Vector3 projectileDirection = TankMatrix.Backward; // Dirección del proyectil es hacia adelante del tanque
                Projectile projectile = new Projectile(projectilePosition , projectileDirection, 2000f); // Crear el proyectil con la posición y dirección correcta
                projectiles.Add(projectile);
                timeSinceLastShot = 0f;
            }
        }
        public void UpdateProjectiles(GameTime gameTime)
        {
            foreach (Projectile projectile in projectiles)
            {
                projectile.Update(gameTime);
            }
        }

        public void DrawProjectiles(Model projectileModel, Matrix view, Matrix projection)
        {
            foreach (Projectile projectile in projectiles)
            {
                Matrix worldMatrix = Matrix.CreateTranslation(projectile.Position);
                // Dibujar el proyectil en su posición actual
                foreach (ModelMesh mesh in projectileModel.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = worldMatrix;
                        effect.View = view;
                        effect.Projection = projection;
                    }
                    mesh.Draw();
                }
            }
        }
    }
}
