using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ThunderingTanks.Collisions;

namespace ThunderingTanks.Objects.Tanks
{
    public class Tank : GameObject
    {

        #region Graphics

        private GraphicsDevice GraphicsDevice;

        public float screenHeight;
        public float screenWidth;
        #endregion

        #region Tank

        public Model Tanque { get; set; }

        private Texture2D PanzerTexture { get; set; }
        private Texture2D TrackTexture { get; set; }
        private float trackOffset1 = 0;
        private float trackOffset2 = 0;

        public Matrix PanzerMatrix { get; set; }
        private Matrix RotationMatrix { get; set; }
        public Matrix TurretMatrix { get; set; }
        public Matrix CannonMatrix { get; set; }

        public Vector3 CameraPosition { get; set; }

        public float TankVelocity { get; set; }
        public float TankRotation { get; set; }
        public float Rotation { get; set; } = 0f;

        public float FireRate { get; set; }
        public float TimeSinceLastShot { get; set; }

        public float GunRotation { get; set; }
        public float GunElevation { get; set; }

        public OrientedBoundingBox TankBox { get; set; }
        public bool isColliding { get; set; } = false;



        public int MaxLife { get; } = 50;
        public int CurrentLife { get; set; }

        public int NumberOfProyectiles { get; set; }

        #endregion

        #region Physics

        private readonly Vector3 Gravity = new(0f, -9.8f, 0); 

        private Vector3 Spring1_Position;
        private Vector3 Spring1_Direction;
        private float   Spring1_Length;

        private Vector3 Spring2_Position;
        private Vector3 Spring2_Direction;
        private float   Spring2_Length;

        private Vector3 Spring3_Position;
        private Vector3 Spring3_Direction;
        private float   Spring3_Length;

        private Vector3 Spring4_Position;
        private Vector3 Spring4_Direction;
        private float   Spring4_Length;

        #endregion

        #region Sound
        public SoundEffectInstance MovingTankSound { get; set; }
        #endregion

        public float SensitivityFactor { get; set; }

        private bool _isPlaying = true;
        public bool isDestroyed = false;

        public Tank()
        {
            TurretMatrix = Matrix.Identity;
            CannonMatrix = Matrix.Identity;

            CurrentLife = MaxLife;
        }

        public override void LoadContent(ContentManager Content, Effect effect)
        {

            Tanque = Content.Load<Model>(ContentFolder3D + "Panzer/Panzer");

            PanzerTexture = Content.Load<Texture2D>(ContentFolder3D + "Panzer/PzVl_Tiger_I");
            TrackTexture = Content.Load<Texture2D>(ContentFolder3D + "Panzer/PzVI_Tiger_I_track");

            Effect = effect;

            MaxBox = new Vector3(200, 250, 300);
            MinBox = new Vector3(-200, 0, -300);
            BoundingBox = new BoundingBox(MinBox, MaxBox);

            TankBox = OrientedBoundingBox.FromAABB(BoundingBox);

            TimeSinceLastShot = FireRate;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, SimpleTerrain terrain)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeSinceLastShot += time;

            bool isMoving = false;

            var directionVector = Vector3.Zero;

            #region Movimiento

            if (keyboardState.IsKeyDown(Keys.W) && !isColliding)
            {
                directionVector -= Vector3.Forward * TankVelocity * time;
                trackOffset1 -= 0.1f;
                trackOffset2 -= 0.1f;
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.S) && !isColliding)
            {
                directionVector += Vector3.Forward * TankVelocity * time;
                trackOffset1 += 0.1f;
                trackOffset2 += 0.1f;
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                Rotation -= TankRotation * time;
                trackOffset1 += 0.1f;
                trackOffset2 -= 0.1f;
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                Rotation += TankRotation * time;
                trackOffset1 -= 0.1f;
                trackOffset2 += 0.1f;
                isMoving = true;
            }

            if (isMoving)
            {
                if (!_isPlaying)
                {
                    MovingTankSound.Play();
                    _isPlaying = true;
                }
            }
            else
            {
                MovingTankSound.Stop();
            }

            #endregion

            GunRotation -= GetRotationFromCursorX() * SensitivityFactor;

            GunElevation += GetElevationFromCursorY() * SensitivityFactor;
            GunElevation = MathHelper.Clamp(GunElevation, MathHelper.ToRadians(-10), MathHelper.ToRadians(6));

            Mouse.SetPosition((int)screenWidth / 2, (int)screenHeight / 2);

            #region MovimientoConRotacion

            RotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Rotation));

            Vector3 rotatedDirection = Vector3.Transform(directionVector, RotationMatrix);

            var newPos = new Vector2(Position.X, Position.Z) + new Vector2(rotatedDirection.X, rotatedDirection.Z);
            var X = newPos.X;
            var Z = newPos.Y;

            #endregion

            float terrainHeight = terrain.Height(X, Z);

            Position = new Vector3(X, terrainHeight - 400, Z);

            PanzerMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Rotation)) * Matrix.CreateTranslation(Position);
            TurretMatrix = Matrix.CreateRotationY(GunRotation) * Matrix.CreateTranslation(Position);
            CannonMatrix = Matrix.CreateTranslation(new Vector3(-15f, 0f, 0f)) * Matrix.CreateRotationX(GunElevation) * Matrix.CreateRotationY(GunRotation) * Matrix.CreateTranslation(Position);

            TankBox.Center = Position + new Vector3(0f, 125f, 0f);
            TankBox.Rotate(Matrix.CreateRotationY(MathHelper.ToRadians(Rotation)));

            CameraPosition = Position + new Vector3(0f, 500f, 0f);

            LastPosition = Position;
        }

        public void Draw(Matrix view, Matrix projection, GraphicsDevice graphicsDevice)
        {

            GraphicsDevice = graphicsDevice;
            GraphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.None };
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

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
                    effect.CurrentTechnique = effect.Techniques["Impact"];
                }

                if (mesh.Name.Equals("Turret"))
                {
                    Effect.Parameters["ModelTexture"].SetValue(PanzerTexture);
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * TurretMatrix);
                    Effect.Parameters["IsTrack"].SetValue(false);

                }
                else if (mesh.Name.Equals("Cannon"))
                {
                    Effect.Parameters["ModelTexture"].SetValue(PanzerTexture);
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * CannonMatrix);
                    Effect.Parameters["IsTrack"].SetValue(false);

                }
                else if (mesh.Name.Equals("Treadmill2"))
                {
                    Effect.Parameters["ModelTexture"].SetValue(TrackTexture);
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * PanzerMatrix);
                    Effect.Parameters["IsTrack"]?.SetValue(true);
                    Effect.Parameters["TrackOffset"].SetValue(trackOffset1);
                }
                else if (mesh.Name.Equals("Treadmill1"))
                {
                    Effect.Parameters["ModelTexture"].SetValue(TrackTexture);
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * PanzerMatrix);
                    Effect.Parameters["IsTrack"]?.SetValue(true);
                    Effect.Parameters["TrackOffset"].SetValue(trackOffset2);
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

        // ------------ FUNCTIONS ------------ //

        /// <summary>
        /// Genera un disparo de el tanque
        /// </summary>
        /// <returns>Projectile generado por el tanque</returns>
        public Projectile Shoot()
        {
            if (TimeSinceLastShot >= FireRate)
            {
                Matrix ProjectileMatrix = Matrix.CreateTranslation(new Vector3(0f, 210f, 400f)) * Matrix.CreateRotationX(GunElevation) * TurretMatrix;

                float projectileScale = 1f;
                float projectileSpeed = 15000f;

                Projectile projectile = new(ProjectileMatrix, GunRotation, projectileSpeed, projectileScale); // Crear el proyectil con la posición y dirección correcta

                TimeSinceLastShot = 0f;
                NumberOfProyectiles -= 1;

                return projectile;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Resta vida al tanque
        /// </summary>
        /// <param name="_juegoIniciado">Condicion de que el juego inicio</param>
        public void ReceiveDamage(ref bool _juegoIniciado)
        {
            CurrentLife -= 25;
            if (CurrentLife <= 0)
            {
                isDestroyed = true;
                _juegoIniciado = false;
            }

        }

        /// <summary>
        /// Valor X del movimiento del cursor
        /// </summary>
        /// <returns>Cantidad de Movimiento X</returns>
        private float GetRotationFromCursorX()
        {
            screenWidth = GraphicsDevice.Viewport.Width;

            MouseState mouseState = Mouse.GetState();
            float mouseX = mouseState.X;

            return MathHelper.ToRadians(mouseX / screenWidth * 360f - 180f);
        }

        /// <summary>
        /// Valor Y del movimiento del cursor
        /// </summary>
        /// <returns>Cantidad de Movimiento Y</returns>
        private float GetElevationFromCursorY()
        {
            screenHeight = GraphicsDevice.Viewport.Height;

            MouseState mouseState = Mouse.GetState();
            float mouseY = mouseState.Y;

            return MathHelper.ToRadians(mouseY / screenHeight * 180f - 90f);
        }
    }
}