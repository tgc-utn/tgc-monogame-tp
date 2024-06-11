using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using ThunderingTanks.Cameras;
using ThunderingTanks.Collisions;

namespace ThunderingTanks.Objects.Tanks
{
    public class Tank : GameObject
    {
        #region ContentFolders
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";
        public const string ContentFolderMusic = "Music/";
        #endregion

        #region Graphics
        private GraphicsDevice graphicsDevice;
        public float screenHeight;
        public float screenWidth;
        #endregion

        #region Tank
        private Effect Effect { get; set; }
        public Model Tanque { get; set; }
        private Texture2D PanzerTexture { get; set; }
        private Texture2D TrackTexture { get; set; }
        public Matrix PanzerMatrix { get; set; }
        public Vector3 LastPosition { get; set; }

        public Vector3 Direction = new(0, 0, 0);
        public float TankVelocity { get; set; }
        public float TankRotation { get; set; }
        public float Rotation = 0;
        public Matrix TurretMatrix { get; set; }
        public Matrix CannonMatrix { get; set; }
        public float FireRate { get; set; }
        public float TimeSinceLastShot;
        public float GunRotationFinal { get; set; }
        public float GunElevation { get; set; }
        public List<ModelBone> Bones { get; private set; }
        public List<ModelMesh> Meshes { get; private set; }
        public TargetCamera PanzerCamera { get; set; }
        public SoundEffectInstance MovingTankSound { get; set; }

        //public BoundingBox TankboundingBox { get; set; }
        //public OrientedBoundingBox TankBox { get; set; }
        public BoundingBox TankBox { get; set; }
        public Vector3 Center { get; set; }
        public Vector3 Extents { get; set; }
        public Vector3 MinBox = new(0, 0, 0);
        public Vector3 MaxBox = new(0, 0, 0);
        public bool isColliding { get; set; } = false;

        public Texture2D LifeBar { get; set; }
        public Rectangle _lifeBarRectangle;

        public int _maxLife = 10;
        public int _currentLife;

        public bool isDestroyed = false;
        #endregion

        public Matrix ProjectileMatrix { get; private set; }

        private bool _isPlaying = true;

        public Tank(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;

            TurretMatrix = Matrix.Identity;
            CannonMatrix = Matrix.Identity;

            _currentLife = _maxLife;
        }

        public void LoadContent(ContentManager Content)
        {

            Tanque = Content.Load<Model>(ContentFolder3D + "Panzer/Panzer");

            PanzerTexture = Content.Load<Texture2D>(ContentFolder3D + "Panzer/PzVl_Tiger_I");
            TrackTexture = Content.Load<Texture2D>(ContentFolder3D + "Panzer/PzVI_Tiger_I_track");

            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            TimeSinceLastShot = FireRate;

            PanzerMatrix = Matrix.CreateTranslation(Position);

            TankBox = new BoundingBox(new Vector3(-200, 0, -300), new Vector3(200, 250, 300));
            //TankBox = new OrientedBoundingBox();

            MinBox = TankBox.Min;
            MaxBox = TankBox.Max;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            TimeSinceLastShot += time;

            bool isMoving = false;

            if (keyboardState.IsKeyDown(Keys.W) && !isColliding)
            {
                Direction -= PanzerMatrix.Forward * TankVelocity * time;
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.S) && !isColliding)
            {
                Direction += PanzerMatrix.Forward * TankVelocity * time;
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.D) && !isColliding)
            {
                Rotation -= TankRotation * time;
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.A) && !isColliding)
            {
                Rotation += TankRotation * time;
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

                if (_isPlaying)
                {
                    MediaPlayer.Stop();
                    _isPlaying = false;
                }
            }

            GunRotationFinal -= GetRotationFromCursorX();
            GunElevation += GetElevationFromCursorY();

            Mouse.SetPosition((int)screenWidth / 2, (int)screenHeight / 2);

            Position = Direction + new Vector3(0f, 500f, 0f);

            PanzerMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Rotation)) * Matrix.CreateTranslation(Direction);
            TurretMatrix = Matrix.CreateRotationY(GunRotationFinal) * Matrix.CreateTranslation(Direction);
            CannonMatrix = Matrix.CreateRotationX(GunElevation) * Matrix.CreateRotationY(GunRotationFinal) * Matrix.CreateTranslation(new Vector3(-0.1f, 0f, 0f)) * Matrix.CreateTranslation(Direction);

            TankBox = new BoundingBox(MinBox + Direction, MaxBox + Direction);

            Center = CollisionsClass.GetCenter(TankBox);
            Extents = CollisionsClass.GetExtents(TankBox);

            //TankBox = new OrientedBoundingBox(Center, Extents);
            //TankBox.Rotate(Matrix.CreateRotationY(MathHelper.ToRadians(Rotation)));

            LastPosition = Direction;

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
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * TurretMatrix);
                }
                else if (mesh.Name.Equals("Cannon"))
                {
                    Effect.Parameters["ModelTexture"].SetValue(PanzerTexture);
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * CannonMatrix);
                }
                else if (mesh.Name.Equals("Treadmill1") || mesh.Name.Equals("Treadmill2"))
                {
                    Effect.Parameters["ModelTexture"].SetValue(TrackTexture);
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * PanzerMatrix);
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

        /// <summary>
        /// Genera un disparo de el tanque
        /// </summary>
        /// <returns>Projectile generado por el tanque</returns>
        public Projectile Shoot()
        {
            if (TimeSinceLastShot >= FireRate)
            {
                ProjectileMatrix = Matrix.CreateTranslation(new Vector3(0f, 210f, 400f)) * Matrix.CreateRotationX(GunElevation) * TurretMatrix;

                float projectileScale = 1f;
                float projectileSpeed = 10000f;

                Projectile projectile = new(ProjectileMatrix, GunRotationFinal, projectileSpeed, projectileScale); // Crear el proyectil con la posición y dirección correcta

                TimeSinceLastShot = 0f;

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
            _currentLife -= 5;
            if (_currentLife <= 0)
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
            MouseState mouseState = Mouse.GetState();
            float mouseX = mouseState.X;
            screenWidth = graphicsDevice.Viewport.Width;
            return MathHelper.ToRadians(mouseX / screenWidth * 360f - 180f);
        }

        /// <summary>
        /// Valor Y del movimiento del cursor
        /// </summary>
        /// <returns>Cantidad de Movimiento Y</returns>
        private float GetElevationFromCursorY()
        {
            screenHeight = graphicsDevice.Viewport.Height;

            MouseState mouseState = Mouse.GetState();
            float mouseY = mouseState.Y;

            return MathHelper.ToRadians(mouseY / screenHeight * 180f - 90f);
        }
    }
}