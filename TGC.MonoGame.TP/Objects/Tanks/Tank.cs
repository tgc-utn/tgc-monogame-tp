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
using SharpDX.MediaFoundation;

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

        private GraphicsDevice GraphicsDevice;

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
        public BoundingBox PrevTankBox { get; set; }

        public Vector3 Center { get; set; }
        public Vector3 Extents { get; set; }
        public Vector3 MinBox = new(0, 0, 0);
        public Vector3 MaxBox = new(0, 0, 0);
        public bool isColliding { get; set; } = false;

        public Vector3 CollidingPosition { get; set; }

        public Texture2D LifeBar { get; set; }
        public Rectangle _lifeBarRectangle;

        public int _maxLife = 50;
        public int _currentLife;
        public int _numberOfProyectiles;

        public bool isDestroyed = false;
        #endregion

        public float SensitivityFactor { get; set; }

        public Matrix ProjectileMatrix { get; private set; }

        private bool _isPlaying = true;

        List<Vector3> verticesTanque;

        public Tank()
        {
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

            //TankBox = new BoundingBox(new Vector3(-200, 0, -300), new Vector3(200, 250, 300));
            //TankBox = new OrientedBoundingBox();


            verticesTanque = ObtenerVerticesModelo(Tanque);
            BoundingBox meshBox = BoundingBox.CreateFromPoints(verticesTanque);

            PrevTankBox = meshBox;

            //TankBox = TankBox == null ? meshBox : BoundingBox.CreateMerged(TankBox, meshBox);
            float factorEscala = 45f; // Escala del 20%

            TankBox = EscalarBoundingBox(PrevTankBox, factorEscala);
            
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

            GunRotationFinal -= GetRotationFromCursorX() * SensitivityFactor;
            GunElevation += GetElevationFromCursorY() * SensitivityFactor;

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

        public void Model(List<ModelBone> bones, List<ModelMesh> meshes)
        {
            Bones = bones;
            Meshes = meshes;
        }

        public void Draw(Matrix world, Matrix view, Matrix projection, GraphicsDevice graphicsDevice)
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

                if (isColliding)
                {
                    Effect.Parameters["onhit"].SetValue(true);
                    Effect.Parameters["ImpactPosition"].SetValue(CollidingPosition);
                } else
                {
                   // Effect.Parameters["onhit"].SetValue(false); si lo descomento el tanque resetea las deformaciones todo el rato, hay que buscar una forma de que no lo haga
                }

                Effect.Parameters["impacto"].SetValue(9000.0f);
                Effect.Parameters["velocidad"].SetValue(9000.0f);
                Effect.Parameters["TankPosition"].SetValue(Position);

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
                float projectileSpeed = 11000f;

                Projectile projectile = new(ProjectileMatrix, GunRotationFinal, projectileSpeed, projectileScale); // Crear el proyectil con la posición y dirección correcta

                TimeSinceLastShot = 0f;
                _numberOfProyectiles -=1;

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

        BoundingBox EscalarBoundingBox(BoundingBox originalBoundingBox, float escala)
        {

            Vector3[] puntosEsquina = originalBoundingBox.GetCorners();

            for (int i = 0; i < puntosEsquina.Length; i++)
            {
                puntosEsquina[i] *= escala;
            }

            return BoundingBox.CreateFromPoints(puntosEsquina);
        }

        List<Vector3> ObtenerVerticesModelo(Model modelo)
        {
            List<Vector3> vertices = new List<Vector3>();

            foreach (ModelMesh mesh in modelo.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Obtener los vértices de este meshPart
                    Vector3[] tempVertices = new Vector3[meshPart.NumVertices];
                    meshPart.VertexBuffer.GetData(tempVertices);

                    // Agregar los vértices a la lista
                    vertices.AddRange(tempVertices);
                }
            }

            return vertices;
        }

        public void RecibirImpacto(Vector3 puntoDeImpacto, float fuerzaImpacto)
        {
            for (int i = 0; i < verticesTanque.Count; i++)
            {
                Vector3 vertice = verticesTanque[i];

                // Calcula la distancia entre el vértice y el punto de impacto
                float distancia = Vector3.Distance(vertice, puntoDeImpacto);

                // Si la distancia está dentro de un radio de deformación
                if (distancia < 50)
                {
                    // Calcula el vector de dirección desde el punto de impacto hacia el vértice
                    Vector3 direccion = Vector3.Normalize(vertice - puntoDeImpacto);

                    // Aplica una deformación al vértice según la fuerza del impacto y la dirección
                    verticesTanque[i] += -direccion * fuerzaImpacto;
                }
            }
        }
    }
}