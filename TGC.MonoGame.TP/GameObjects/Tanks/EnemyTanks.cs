using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using ThunderingTanks.Cameras;
using ThunderingTanks.Collisions;

namespace ThunderingTanks.Objects.Tanks
{
    public class EnemyTank : GameObject
    {

        public ParticleSystem particleSystem;

        public Model Tanque { get; set; }
        private Texture2D PanzerTexture { get; set; }
        private Texture2D TrackTexture { get; set; }
        public Vector3 PanzerPosition { get; set; }
        public Matrix PanzerMatrix { get; set; }

        private Vector3 Direction = Vector3.Zero;
        public float Rotation = 0;

        public BoundingBox TankBox { get; set; }

        public float TankVelocity { get; set; }
        public float TankRotation { get; set; }
        public List<ModelBone> Bones { get; private set; }
        public List<ModelMesh> Meshes { get; private set; }
        public float FireRate { get; set; }
        private float timeSinceLastShot = 0f;

        public float screenHeight;
        public float screenWidth;
        public float GunRotationFinal = 0;
        public float GunElevation = 0;

        public float GunRotation { get; set; }

        public Matrix turretWorld { get; set; }
        public Matrix cannonWorld { get; set; }

        public bool Stop { get; set; } = false;

        public float shootInterval;
        public float lifeSpan;
        public float life = 5;
        private float trackOffset = 0;

        public EnemyTank(GraphicsDevice graphicsDevice)
        {
            turretWorld = Matrix.Identity;
            cannonWorld = Matrix.Identity;
            TankVelocity = 100f;
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            Tanque = Content.Load<Model>(ContentFolder3D + "M4/M4");
            PanzerTexture = Content.Load<Texture2D>(ContentFolder3D + "M4/M4_Sherman");
            TrackTexture = Content.Load<Texture2D>(ContentFolder3D + "M4/Grant_track");
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            PanzerMatrix = Matrix.CreateTranslation(Position);

            MinBox = new(-200, 0, -300);
            MaxBox = new(200, 250, 300);
            TankBox = new BoundingBox(Position + MinBox, Position + MaxBox);

            //particleSystem = new ParticleSystem(graphicsDevice);
        }

        public void Update(GameTime gameTime, Vector3 playerPosition, SimpleTerrain terrain, GraphicsDevice graphicsDevice, Camera camera)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastShot += time;
            Vector3 screenPosition = graphicsDevice.Viewport.Project(Position, camera.Projection, camera.View, Matrix.Identity);

            Vector3 direction = playerPosition - Position;

            direction.Y = 0;

            var X = Position.X;
            var Z = Position.Z;

            float terrainHeight = terrain.Height(X, Z);

            Position = new Vector3(Position.X, terrainHeight - 400, Position.Z);

            float distanceToPlayer = direction.Length();
            Direction = Vector3.Normalize(direction);

            if (distanceToPlayer < 2500f)
            {
                TankVelocity = 0f;
            }
            else if (Stop == true)
            {
                TankVelocity = 0f;
                GunRotationFinal = 0f;
            }
            else
            {
                Rotation = (float)Math.Atan2(Direction.X, Direction.Z);
                GunElevation = (float)Math.Atan2(playerPosition.Y - Position.Y, distanceToPlayer);

                GunRotationFinal = Rotation;
                TankVelocity = 180f;

                LastPosition = Position;

                Position += Direction * TankVelocity * time;
                if (Direction.Y > 0)
                    trackOffset += 0.1f;
                else
                    trackOffset -= 0.1f;

                TankBox = new BoundingBox(Position + MinBox, Position + MaxBox);

            }

            PanzerMatrix = Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(Position);
            turretWorld = Matrix.CreateRotationY(GunRotationFinal) * Matrix.CreateTranslation(Position);
            cannonWorld = Matrix.CreateScale(100f) * Matrix.CreateRotationX(GunElevation) * turretWorld;
        }

        public void Draw(Matrix world, Matrix view, Matrix projection, GraphicsDevice _GraphicsDevice)
        {
            var rasterizerState = new RasterizerState();
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);

            foreach (var mesh in Tanque.Meshes)
            {

                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = Effect;
                }

                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                }

                if (mesh.Name.Equals("Turret"))
                {
                    Effect.Parameters["ModelTexture"].SetValue(PanzerTexture);
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * turretWorld);
                }
                else if (mesh.Name.Equals("Cannon"))
                {
                    Effect.Parameters["ModelTexture"].SetValue(PanzerTexture);
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * cannonWorld);
                }
                else if (mesh.Name.Equals("Treadmill1") || mesh.Name.Equals("Treadmill2"))
                {
                    Effect.Parameters["ModelTexture"].SetValue(TrackTexture);
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * PanzerMatrix);
                    Effect.Parameters["IsTrack"]?.SetValue(true);
                    Effect.Parameters["TrackOffset"].SetValue(trackOffset);
                }
                else
                {
                    Effect.Parameters["ModelTexture"].SetValue(PanzerTexture);
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * PanzerMatrix);

                    Effect.Parameters["IsTrack"].SetValue(false);
                }

                mesh.Draw();

            }
        }

        public void StopEnemy()
        {
            Stop = true;
        }

        public Projectile Shoot()
        {
            Matrix ProjectileMatrix = Matrix.CreateTranslation(new Vector3(0f, 210f, 400f)) * Matrix.CreateRotationX(GunElevation) * turretWorld;

            float projectileScale = 1f;
            float projectileSpeed = 15000f;

            Projectile projectile = new(ProjectileMatrix, GunRotationFinal, projectileSpeed, projectileScale);

            return projectile;
        }



    }
}