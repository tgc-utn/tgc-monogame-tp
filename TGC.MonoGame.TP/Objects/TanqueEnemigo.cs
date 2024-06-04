
using BepuPhysics.Collidables;
using BepuPhysics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using ThunderingTanks.Cameras;

namespace ThunderingTanks.Objects
{
    public class EnemyTank : GameObject
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";

        private Effect Effect { get; set; }
        public Model Tanque { get; set; }
        private Texture2D PanzerTexture { get; set; }
        public Vector3 PanzerPosition { get; set; }
        public Matrix PanzerMatrix { get; set; }

        private Vector3 Direction = Vector3.Zero;
        public float Rotation = 0;
        public BoundingBox TankBox { get; set; }
        public float TankVelocity { get; set; }
        public float TankRotation { get; set; }
        private GraphicsDevice graphicsDevice;
        public List<ModelBone> Bones { get; private set; }
        public List<ModelMesh> Meshes { get; private set; }
        public float FireRate { get; set; }
        private float timeSinceLastShot = 0f;

        public float screenHeight;
        public float screenWidth;
        public float GunRotationFinal = 0;
        public float GunRotation { get; set; }
        public float GunElevation { get; set; }
        public Matrix turretWorld { get; set; }
        public Matrix cannonWorld { get; set; }

        public EnemyTank(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            turretWorld = Matrix.Identity;
            cannonWorld = Matrix.Identity;
            TankVelocity = 100f;
        }

        public void LoadContent(ContentManager Content)
        {
            Tanque = Content.Load<Model>(ContentFolder3D + "Panzer/Panzer");
            PanzerTexture = Content.Load<Texture2D>(ContentFolder3D + "Panzer/PzVI_Tiger_I_SM");
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            foreach (var mesh in Tanque.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }

            PanzerMatrix = Matrix.CreateTranslation(Position);
            TankBox = CreateBoundingBox(Tanque, Matrix.CreateScale(1f), Position);
        }

        public void Update(GameTime gameTime, Vector3 playerPosition)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastShot += time;

            // Calcular la dirección hacia el jugador (solo en los ejes X y Z)
            Vector3 direction = playerPosition - Position;
            direction.Y = 0; // Ignorar la componente Y para evitar movimientos verticales
            float distanceToPlayer = direction.Length();
            Direction = Vector3.Normalize(direction);

            // Si el tanque está a una distancia menor que la permitida, no se mueve más
            if (distanceToPlayer < 2500f)
            {
                TankVelocity = 0f;
            }
            else
            {
                TankVelocity = 100f;
                // Moverse hacia el jugador
                Position += Direction * TankVelocity * time;
            }

            // Rotar el tanque enemigo hacia el jugador
            Rotation = (float)Math.Atan2(Direction.X, Direction.Z);

            GunRotationFinal = Rotation;

            // Actualizar la matriz del tanque con la rotación correcta
            PanzerMatrix = Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(Position);
            turretWorld = Matrix.CreateRotationY(GunRotationFinal) * Matrix.CreateTranslation(Position);
            cannonWorld = Matrix.CreateScale(100f) * Matrix.CreateRotationX(GunElevation) * turretWorld;
            TankBox = new BoundingBox(TankBox.Min + Direction, TankBox.Max + Direction);

            // Lógica de disparo hacia el jugador
            Shoot(playerPosition);
        }


        public void Draw(Matrix world, Matrix view, Matrix projection, GraphicsDevice _GraphicsDevice)
        {
            var rasterizerState = new RasterizerState();
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);

            foreach (var mesh in Tanque.Meshes)
            {
                if (mesh.Name.Equals("Turret"))
                {
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * turretWorld);
                }
                else if (mesh.Name.Equals("Cannon"))
                {
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


        public Projectile Shoot(Vector3 playerPosition)
        {
            if (timeSinceLastShot >= FireRate)
            {
                // Calcular la dirección hacia el jugador
                Vector3 direction = playerPosition - Position;
                direction.Y = 0; // Ignorar la componente Y para evitar movimientos verticales
                direction.Normalize();

                // Crear la matriz de transformación del proyectil
                Matrix projectileMatrix = Matrix.CreateWorld(Position, direction, Vector3.Up);
                float projectileScale = 0.3f; // Ajusta esta escala según tus necesidades

                // Crear el proyectil con la posición y dirección correcta
                Projectile projectile = new Projectile(projectileMatrix, GunRotationFinal, 50000f, projectileScale);
                timeSinceLastShot = 0f;
                return projectile;
            }
            else
            {
                return null;
            }
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