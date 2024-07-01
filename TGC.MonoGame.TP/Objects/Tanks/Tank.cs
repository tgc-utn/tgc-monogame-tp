using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.DirectWrite;
//using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        private GraphicsDevice GraphicsDevice;

        public float screenHeight;
        public float screenWidth;
        #endregion

        #region Tank
        private Effect Effect { get; set; }
        public Model Tanque { get; set; }
        private Texture2D PanzerTexture { get; set; }
        private Texture2D TrackTexture { get; set; }
        private float trackOffset1 = 0;
        private float trackOffset2 = 0;
        public Matrix PanzerMatrix { get; set; }
        public Vector3 LastPosition { get; set; }

        public Vector3 Direction = new(0, 0, 0);
        public float TankVelocity { get; set; }

        public Matrix RotationMatrix { get; set; }
        public float TankRotation { get; set; }
        public float Rotation = 0;
        public float LastRotation;

        private bool collided;
        public Matrix TurretMatrix { get; set; }
        public Matrix CannonMatrix { get; set; }
        public float FireRate { get; set; }
        public float TimeSinceLastShot;
        public float GunRotationFinal { get; set; }
        public float GunElevation { get; set; }
        public List<ModelBone> Bones { get; private set; }
        public List<ModelMesh> Meshes { get; private set; }
        public Camera PanzerCamera { get; set; }
        public SoundEffectInstance MovingTankSound { get; set; }

        //public BoundingBox TankboundingBox { get; set; }
        //public OrientedBoundingBox TankBox { get; set; }
        public OrientedBoundingBox TankBox { get; set; }
        public OrientedBoundingBox PrevTankBox { get; set; }

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

        public float VelocidadImpacto { get; set; }

        public Matrix ProjectileMatrix { get; private set; }

        private bool _isPlaying = true;

        List<Vector3> verticesTanque;

        public Tank()
        {
            TurretMatrix = Matrix.Identity;
            CannonMatrix = Matrix.Identity;
            TankBox = new OrientedBoundingBox();


            _currentLife = _maxLife;
        }

        public void LoadContent(ContentManager Content, Effect effect)
        {

            Tanque = Content.Load<Model>(ContentFolder3D + "Panzer/Panzer");

            PanzerTexture = Content.Load<Texture2D>(ContentFolder3D + "Panzer/PzVl_Tiger_I");
            TrackTexture = Content.Load<Texture2D>(ContentFolder3D + "Panzer/PzVI_Tiger_I_track");


            Effect = effect;

            // Obtener los vértices del modelo del tanque
            List<Vector3> verticesTanque = ObtenerVerticesModelo(Tanque).ToList();
            TankBox = OrientedBoundingBox.ComputeFromPoints(verticesTanque.ToArray());
            TankBox.Extents *= 100f;

            TimeSinceLastShot = FireRate;



            collided = false;

            Effect.Parameters["impacto"].SetValue(140f);
            VelocidadImpacto = -320f;

        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, SimpleTerrain terrain)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeSinceLastShot += time;

            bool isMoving = false;
            bool isRotating = false;

            PrevTankBox = TankBox;

            if (isColliding)
            {
                collided = true;
            }

            var directionVector = Vector3.Zero;

            if (keyboardState.IsKeyDown(Keys.W) && !isColliding)
            {
                directionVector -= Vector3.Forward * TankVelocity * time;
                if (collided)
                {
                    CollidingPosition -= PanzerMatrix.Forward * TankVelocity * time;
                }
                trackOffset1 -= 0.1f;
                trackOffset2 -= 0.1f;
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.S) && !isColliding)
            {
                directionVector += Vector3.Forward * TankVelocity * time;
                if (collided)
                {
                    CollidingPosition += PanzerMatrix.Forward * TankVelocity * time;
                }
                trackOffset1 += 0.1f;
                trackOffset2 += 0.1f;
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                Rotation -= TankRotation * time;
                isRotating = true;
                trackOffset1 += 0.1f;
                trackOffset2 -= 0.1f;
                isMoving = true;
                //Effect.Parameters["angulo"].SetValue(TankRotation);
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                Rotation += TankRotation * time;
                isRotating = true;
                trackOffset1 -= 0.1f;
                trackOffset2 += 0.1f;
                isMoving = true;
                //Effect.Parameters["angulo"].SetValue(TankRotation);
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

            GunElevation = MathHelper.Clamp(GunElevation, MathHelper.ToRadians(-10), MathHelper.ToRadians(6));

            Mouse.SetPosition((int)screenWidth / 2, (int)screenHeight / 2);

            RotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Rotation));

            Vector3 rotatedDirection = Vector3.Transform(directionVector, RotationMatrix);

            var newPos = new Vector2(Direction.X, Direction.Z) + new Vector2(rotatedDirection.X, rotatedDirection.Z);
            var X = newPos.X;
            var Z = newPos.Y;
            float terrainHeight = terrain.Height(X, Z);

            Direction = new Vector3(X, terrainHeight - 400, Z);

            // Aquí calculamos la inclinación
            float currentHeight = terrain.Height(Direction.X, Direction.Z);
            float previousHeight = terrain.Height(LastPosition.X, LastPosition.Z);
            float heightDifference = -(currentHeight - previousHeight);

            float pitch = (float)Math.Atan2(heightDifference, Vector3.Distance(new Vector3(Direction.X, 0, Direction.Z), new Vector3(LastPosition.X, 0, LastPosition.Z)));
            Matrix pitchMatrix = Matrix.CreateRotationX(pitch);

            Position = Direction + new Vector3(0f, 500f, 0f);

            PanzerMatrix = pitchMatrix * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation)) * Matrix.CreateTranslation(Direction);
            TurretMatrix = Matrix.CreateRotationY(GunRotationFinal) * Matrix.CreateTranslation(Direction);
            CannonMatrix = Matrix.CreateTranslation(new Vector3(-15f, 0f, 0f)) * Matrix.CreateRotationX(GunElevation) * Matrix.CreateRotationY(GunRotationFinal) * Matrix.CreateTranslation(Direction);

            // Actualizar la posición y rotación de la caja orientada
            //Quaternion RotationQuaternion = Quaternion.CreateFromRotationMatrix(PanzerMatrix);
            TankBox.Center = Direction; // Actualiza el centro de la OBB
            if (isRotating)
                TankBox.Rotate(Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-90)) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation))); // Actualiza la rotación de la OBB


            LastPosition = Direction;
            LastRotation = Rotation;

            Vector3 direccion = VersorDireccion(CollidingPosition, Direction);

            Vector3 direccion_R = rotacion(direccion);
            Vector3 c_Esfera = Direction + (new Vector3(direccion_R.X, direccion_R.Y, direccion_R.Z) * VelocidadImpacto);

            Vector4 Plano_ST = crearPlano(c_Esfera, Direction);

            Effect.Parameters["c_Esfera"].SetValue(c_Esfera);
            //Effect.Parameters["Plano_ST"].SetValue(Plano_ST); //experimental
        }

        public void Model(List<ModelBone> bones, List<ModelMesh> meshes)
        {
            Bones = bones;
            Meshes = meshes;
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

                if (isColliding)
                {
                    Effect.Parameters["onhit"].SetValue(1);
                }
                else
                {
                    //Effect.Parameters["onhit"].SetValue(false); //si lo descomento el tanque resetea las deformaciones todo el rato, hay que buscar una forma de que no lo haga
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
                float projectileSpeed = 15000f;

                Projectile projectile = new(ProjectileMatrix, GunRotationFinal, projectileSpeed, projectileScale); // Crear el proyectil con la posición y dirección correcta

                TimeSinceLastShot = 0f;
                _numberOfProyectiles -= 1;

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

        /*List<Vector3> ObtenerVerticesModelo(Model modelo)
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
        }*/

        public void RecibirImpacto(Vector3 puntoDeImpacto, float fuerzaImpacto)
        {
            if(verticesTanque == null)
                return;
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

        public Vector3 VersorDireccion(Vector3 A, Vector3 B)
        {
            Vector3 Vector = B - A;
            float moduloVector = Vector.Length();

            return Vector / moduloVector;
        }

        public Vector3 rotacion(Vector3 direccion)
        {
            //derivado de los apuntes de 2D
            Vector3 direc_R;
            direc_R.Y = direccion.Y;
            direc_R.Z = direccion.X * (float)Math.Cos(MathHelper.ToRadians(Rotation)) - direccion.Z * (float)Math.Sin(MathHelper.ToRadians(Rotation));
            direc_R.X = direccion.Z * (float)Math.Cos(MathHelper.ToRadians(Rotation)) + direccion.X * (float)Math.Sin(MathHelper.ToRadians(Rotation));
            return direc_R;
        }

        public Vector4 crearPlano(Vector3 centro, Vector3 TankPosition)
        {
            Vector3 versorNormal = VersorDireccion(centro, TankPosition); // normal del plano
            Vector4 plano;
            plano = new Vector4( // pi = ax + by + cz + d
            versorNormal.X, //a
            versorNormal.Y, //b
            versorNormal.Z, //c
            -(versorNormal.X * centro.X + versorNormal.Y * centro.Y + versorNormal.Z * centro.Z) //d
            );
            return plano;
        }

        private List<Vector3> ObtenerVerticesModelo(Model model)
        {
            List<Vector3> vertices = new List<Vector3>();

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    VertexBuffer vertexBuffer = meshPart.VertexBuffer;
                    int vertexCount = vertexBuffer.VertexCount;
                    VertexPositionNormalTexture[] vertexData = new VertexPositionNormalTexture[vertexCount];
                    vertexBuffer.GetData(vertexData);

                    foreach (VertexPositionNormalTexture vertex in vertexData)
                    {
                        vertices.Add(vertex.Position);
                    }
                }
            }
            return vertices;
        }
    }
}