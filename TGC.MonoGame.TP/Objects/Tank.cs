using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using ThunderingTanks.Cameras;
using ThunderingTanks.Collisions;

namespace ThunderingTanks.Objects
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
        public Matrix PanzerMatrix { get; set; }
        public Vector3 LastPosition { get; set; }

        public Vector3 Direction = new(0, 0, 0);
        public float TankVelocity { get; set; }
        public float TankRotation { get; set; }
        public float Rotation     = 0;
        public Matrix TurretMatrix { get; set; }
        public Matrix CannonMatrix { get; set; }
        public float FireRate { get; set; }
        private float TimeSinceLastShot = 0f;
        public float GunRotationFinal { get; set; }
        public float GunElevation { get; set; }
        public List<ModelBone> Bones { get; private set; }
        public List<ModelMesh> Meshes { get; private set; }
        public TargetCamera PanzerCamera { get; set; }
        private Song movingTankSound { get; set; }
        public OrientedBoundingBox TankBox { get; set; }
        public Vector3 MinBox = new(0, 0, 0);
        public Vector3 MaxBox = new(0, 0, 0);
        public bool isColliding { get; set; } = false;
        public Texture2D LifeBar { get; set; }
        public Rectangle _lifeBarRectangle;
        public int _maxLife = 50;
        public int _currentLife;

        public bool isDestroyed = false;
        #endregion

        public Matrix ProjectileMatrix { get; private set; }

        private bool _isPlaying = true;

        public Tank(GraphicsDevice graphicsDevice, Song movingSound)
        {
            movingTankSound = movingSound;
            this.graphicsDevice = graphicsDevice;

            TurretMatrix = Matrix.Identity;
            CannonMatrix = Matrix.Identity;

            _currentLife = _maxLife;
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

            var BoundingBox = CreateBoundingBox(Tanque, Matrix.CreateScale(1f), Position);
            Console.WriteLine($"Colisión detectada con roca en índice {BoundingBox}");

            MinBox = BoundingBox.Min;
            MaxBox = BoundingBox.Max;

            TankBox = OrientedBoundingBox.FromAABB(BoundingBox);
            Console.WriteLine($"Colisión detectada con roca en índice {TankBox}");

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
                    MediaPlayer.Play(movingTankSound);
                    _isPlaying = true;
                }
            }
            else
            {
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
            CannonMatrix = Matrix.CreateTranslation(new Vector3(-0.1f, 0f, 0f)) * Matrix.CreateScale(100f) * Matrix.CreateRotationX(GunElevation) * TurretMatrix;

            var BoundingBox = new BoundingBox(MinBox + Direction, MaxBox + Direction);

            TankBox = OrientedBoundingBox.FromAABB(BoundingBox);
            TankBox.Rotate(Matrix.CreateRotationY(MathHelper.ToRadians(Rotation)));

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

            foreach (var mesh in Tanque.Meshes)
            {
                if (mesh.Name.Equals("Turret"))
                {
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * TurretMatrix);
                }
                else if (mesh.Name.Equals("Cannon"))
                {
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
                ProjectileMatrix = Matrix.CreateTranslation(new Vector3(0f, 210f, 300f)) * Matrix.CreateRotationX(GunElevation) * TurretMatrix;

                float projectileScale = 1f;
                float projectileSpeed = 5000f;

                Projectile projectile = new(ProjectileMatrix, GunRotationFinal, projectileSpeed, projectileScale); // Crear el proyectil con la posición y dirección correcta

                TimeSinceLastShot = 0f;

                return projectile;
            }
            else
            {
                return null;
            }
        }

        public void ReceiveDamage(ref bool _juegoIniciado)
        {
            _currentLife -= 5;
            if (_currentLife <= 0)
            {
                isDestroyed = true;
                _juegoIniciado = false;

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

        private BoundingBox CreateBoundingBox(Model model, Matrix escala, Vector3 position)
        {
            var minPoint = Vector3.One * float.MaxValue;
            var maxPoint = Vector3.One * float.MinValue;

            var transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (var mesh in model.Meshes)
            {
                var meshParts = mesh.MeshParts;

                foreach (var meshPart in meshParts)
                {
                    var vertexBuffer = meshPart.VertexBuffer;
                    var declaration = vertexBuffer.VertexDeclaration;
                    var vertexSize = declaration.VertexStride / sizeof(float);

                    var rawVertexBuffer = new float[vertexBuffer.VertexCount * vertexSize];
                    vertexBuffer.GetData(rawVertexBuffer);

                    for (var vertexIndex = 0; vertexIndex < rawVertexBuffer.Length; vertexIndex += vertexSize)
                    {
                        var transform = transforms[mesh.ParentBone.Index] * escala;
                        var vertex = new Vector3(rawVertexBuffer[vertexIndex], rawVertexBuffer[vertexIndex + 1], rawVertexBuffer[vertexIndex + 2]);
                        vertex = Vector3.Transform(vertex, transform);
                        minPoint = Vector3.Min(minPoint, vertex);
                        maxPoint = Vector3.Max(maxPoint, vertex);
                    }
                }
            }

            return new BoundingBox(minPoint + position, maxPoint + position);
        }
    }
}