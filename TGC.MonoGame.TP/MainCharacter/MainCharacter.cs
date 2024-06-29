using System;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Camera;
using TGC.MonoGame.TP.Stages;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.MainCharacter;
using System.Collections.Generic;

namespace TGC.MonoGame.TP.MainCharacter
{
    public class Character : Entity
    {
        const string ContentFolder3D = "3D/";
        const string ContentFolderEffects = "Effects/";
        const string ContentFolderTextures = "Textures/";

        string TexturePath;

        ContentManager Content;

        Model Sphere;
        public Matrix World;
        Matrix Scale = Matrix.CreateScale(12.5f);
        Effect Effect;

        Material CurrentMaterial = Material.RustedMetal;

        //public Vector3 Position;
        //Vector3 Velocity;
        //Vector3 Acceleration = Vector3.Zero;
        //Quaternion Rotation = Quaternion.Identity;
        //Vector3 RotationAxis = Vector3.UnitY;
        //float RotationAngle = 0f;


        // Colisiones
        public BoundingSphere EsferaBola { get; set; }
        public bool OnGround { get; set; }
        public Stage ActualStage;
        public struct Face
        {
            public Vector3 Normal;
            public Vector3[] Vertices;

            public Face(Vector3 normal, Vector3[] vertices)
            {
                Normal = normal;
                Vertices = vertices;
            }
        }
        // Colisiones

        Vector3 BallSpinAxis = Vector3.UnitX;
        float BallSpinAngle = 0f;
        Matrix WorldWithBallSpin;

        Vector3 LightPos { get; set; }
        public Matrix Spin;

        //Vector3 startPos;


        public Vector3 ForwardVector = Vector3.UnitX;

        public Vector3 RightVector = Vector3.UnitZ;
        public Character(ContentManager content, Stage stage, List<Entity> entities)
        {
            Content = content;
            Spin = Matrix.CreateFromAxisAngle(Vector3.UnitZ, 0);

            ActualStage = stage;

            InitializeEffect();
            InitializeSphere(stage.CharacterInitialPosition);
            InitializeTextures();
            InitializeLight();
        }
        void InitializeLight()
        {
            LightPos = Position + new Vector3(0, 10, 0);
        }

        private void InitializeSphere(Vector3 initialPosition)
        {
            // Got to set a texture, else the translation to mesh does not map UV
            Sphere = Content.Load<Model>(ContentFolder3D + "geometries/sphere");

            Position = initialPosition;
            World = Scale * Matrix.CreateTranslation(Position);
            WorldWithBallSpin = World;

            // Bounding Sphere asociado a la bola principal
            UpdateBBSphere(Position);

            // Apply the effect to all mesh parts
            Sphere.Meshes.FirstOrDefault().MeshParts.FirstOrDefault().Effect = Effect;
        }

        private void UpdateBBSphere(Vector3 center)
        {
            EsferaBola = new BoundingSphere(center, 10f);
        }

        private void InitializeEffect()
        {
            Effect = Content.Load<Effect>(ContentFolderEffects + "PBR");
            Effect.CurrentTechnique = Effect.Techniques["PBR"];
        }

        private void InitializeTextures()
        {
            UpdateMaterialPath();
            LoadTextures();
        }

        public void Update(GameTime gameTime)
        {
            ProcessMaterialChange();
            ProcessMovement(gameTime);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            var worldView = WorldWithBallSpin * view;
            Effect.Parameters["matWorld"].SetValue(WorldWithBallSpin);
            Effect.Parameters["matWorldViewProj"].SetValue(worldView * projection);
            Effect.Parameters["matInverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(WorldWithBallSpin)));
            Effect.Parameters["lightPosition"].SetValue(LightPos);
            Effect.Parameters["lightColor"].SetValue(new Vector3(253, 251, 211));
            Sphere.Meshes.FirstOrDefault().Draw();
        }

        private void LoadTextures()
        {
            Texture2D albedo, ao, metalness, roughness, normals;

            normals = Content.Load<Texture2D>(TexturePath + "normal");
            ao = Content.Load<Texture2D>(TexturePath + "ao");
            metalness = Content.Load<Texture2D>(TexturePath + "metalness");
            roughness = Content.Load<Texture2D>(TexturePath + "roughness");
            albedo = Content.Load<Texture2D>(TexturePath + "color");

            Effect.Parameters["albedoTexture"]?.SetValue(albedo);
            Effect.Parameters["normalTexture"]?.SetValue(normals);
            Effect.Parameters["metallicTexture"]?.SetValue(metalness);
            Effect.Parameters["roughnessTexture"]?.SetValue(roughness);
            Effect.Parameters["aoTexture"]?.SetValue(ao);
        }

        private void ProcessMaterialChange()
        {
            var keyboardState = Keyboard.GetState();

            var NewMaterial = CurrentMaterial;

            if (keyboardState.IsKeyDown(Keys.D1))
            {
                NewMaterial = Material.RustedMetal;
            }
            else if (keyboardState.IsKeyDown(Keys.D2))
            {
                NewMaterial = Material.Grass;
            }
            else if (keyboardState.IsKeyDown(Keys.D3))
            {
                NewMaterial = Material.Gold;
            }
            else if (keyboardState.IsKeyDown(Keys.D4))
            {
                NewMaterial = Material.Marble;
            }
            else if (keyboardState.IsKeyDown(Keys.D5))
            {
                NewMaterial = Material.Metal;
            }

            if (NewMaterial != CurrentMaterial)
            {
                CurrentMaterial = NewMaterial;
                SwitchMaterial();
                LoadTextures();
            }

        }

        private void UpdateMaterialPath()
        {
            TexturePath = ContentFolderTextures + "materials/";
            switch (CurrentMaterial)
            {
                case Material.RustedMetal:
                    TexturePath += "harsh-metal";
                    break;

                case Material.Marble:
                    TexturePath += "marble";
                    break;

                case Material.Gold:
                    TexturePath += "gold";
                    break;

                case Material.Metal:
                    TexturePath += "metal";
                    break;

                case Material.Grass:
                    TexturePath += "ground";
                    break;
            }

            TexturePath += "/";
        }

        private void SwitchMaterial()
        {
            // We do not dispose textures, as they cannot be loaded again
            UpdateMaterialPath();
            LoadTextures();
        }


        public float DistanceToGround(Vector3 pos)
        {
            float dist = 1000000.0f;
            foreach (BoundingBox box in ActualStage.Colliders)
            {
                Ray tempRay = new Ray(pos, -Vector3.Up);
                float? tempDist = tempRay.Intersects(box);

                if (dist > tempDist)
                {
                    dist = (float)tempDist;
                }
            }

            return dist;
        }

        public bool IsOnGround(Vector3 pos)
        {
            foreach (BoundingBox box in ActualStage.Colliders)
            {
                Ray tempRay = new Ray(pos, -Vector3.Up);
                float? dist = tempRay.Intersects(box);
                if (dist.HasValue && dist <= 12.5f)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsColliding()
        {
            foreach (BoundingBox box in ActualStage.Colliders)
            {
                if (EsferaBola.Intersects(box))
                {
                    return true;
                }
            }
            return false;
        }

        public static Vector3 GetCollisionPoint(BoundingSphere sphere, BoundingBox box)
        {
            // Encuentra el punto más cercano en la superficie de la caja al centro de la esfera
            Vector3 closestPoint = Vector3.Clamp(sphere.Center, box.Min, box.Max);

            // Calcula la dirección desde el punto más cercano al centro de la esfera
            Vector3 direction = sphere.Center - closestPoint;

            // Si la dirección es cero, el centro de la esfera está dentro de la caja, podemos retornar el punto más cercano
            if (direction == Vector3.Zero)
            {
                return closestPoint;
            }

            // Normaliza la dirección
            direction.Normalize();

            // Calcula el punto exacto de colisión en la superficie de la esfera
            Vector3 collisionPoint = closestPoint + direction * sphere.Radius;

            return collisionPoint;
        }

        Vector3 getCollisionNormalNEW(BoundingSphere sphere, BoundingBox box)
        {
            Vector3 puntoContacto = GetCollisionPoint(sphere, box);
            return Vector3.Normalize(sphere.Center - puntoContacto);
        }
        public void ProcessCollisionNEW(float deltaTime)
        {
            Vector3 oldPosition = Position;
            Vector3 movement = Velocity * deltaTime * deltaTime * 0.5f;
            Vector3 newPosition = oldPosition + movement;

            UpdateBBSphere(newPosition);

            bool collisionDetected = false;
            bool isOnGround = IsOnGround(newPosition); // Verificar si está en el suelo en la nueva posición
            //bool isOnGround = false;

            foreach (BoundingBox collider in ActualStage.Colliders)
            {
                if (EsferaBola.Intersects(collider))
                {
                    collisionDetected = true;

                    // Manejar la colisión
                    Vector3 surfaceNormal = getCollisionNormalNEW(EsferaBola, collider);

                    // Determinar si la colisión es con el suelo (por ejemplo, si la normal es vertical hacia arriba)
                    if (Math.Abs(surfaceNormal.Y) > 0f) // Ajusta este valor según tu escenario
                    {
                        isOnGround = true;
                    }
                    Velocity = Vector3.Reflect(Velocity, surfaceNormal);
                    movement = Velocity * deltaTime * deltaTime * 0.5f;
                    newPosition = oldPosition + movement;
                    UpdateBBSphere(newPosition);
                }
            }

            // Actualizar la posición solo si no hubo colisión con el suelo o está en el suelo
            if (!collisionDetected || isOnGround)
            {
                Position = newPosition;
                UpdateBBSphere(Position);
            }
        }

        public void ChangeDirection(float angle)
        {
            ForwardVector = Vector3.Transform(Vector3.UnitX, Matrix.CreateRotationY(angle));
            RightVector = Vector3.Transform(Vector3.UnitZ, Matrix.CreateRotationY(angle));
        }


        private void ProcessMovement(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float speed = 100f;

            // Capturar Input teclado
            var keyboardState = Keyboard.GetState();

            // Procesamiento del movimiento horizontal
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                Acceleration += Vector3.Transform(ForwardVector * -speed, Rotation); //amtes unitx
            }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                Acceleration += Vector3.Transform(ForwardVector * -speed, Rotation) * (-1);

            }
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                Acceleration += Vector3.Transform(RightVector * speed, Rotation); //antes unitz
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                Acceleration += Vector3.Transform(RightVector * speed, Rotation) * (-1);

            }

            Acceleration += new Vector3(0f, -100f, 0f);

            //Procesamiento del movimiento vertical
            float distGround = DistanceToGround(Position);
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && (distGround <= 12.5f || IsColliding()))
            {
                // Seteo la velocidad vertical en 0 para que el salto sea siempre a la misma distancia
                Velocity = new Vector3(Velocity.X, 0f, Velocity.Z);
                Velocity += Vector3.Up * speed * 100f;
            }


            Vector3 HorizontalVelocity = new Vector3(Velocity.X, 0, Velocity.Z);
            BallSpinAngle += HorizontalVelocity.Length() * elapsedTime * elapsedTime / (MathHelper.Pi * 12.5f);
            BallSpinAxis = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, Velocity));

            if (Acceleration == Vector3.Zero || Vector3.Dot(Acceleration, Velocity) < 0)
            {
                Velocity *= 1 - elapsedTime;
            }

            if (Velocity == Vector3.Zero) BallSpinAxis = Vector3.UnitZ;

            Rotation = Quaternion.CreateFromAxisAngle(RotationAxis, RotationAngle);

            Velocity += Acceleration;

            ProcessCollisionNEW(elapsedTime);

            MoveTo(Position);

            // Resetea la posición inicial del nivel si se cae al vacío
            if (Position.Y < -200)
            {
                Position = ActualStage.CharacterInitialPosition;
                Velocity = Vector3.Zero;
                MoveTo(Position);
                UpdateBBSphere(Position);
            }

            Acceleration = Vector3.Zero;
        }
        //float DeltaX, DeltaZ;
        public void MoveTo(Vector3 position)
        {
            World = Scale * Matrix.CreateTranslation(position);
            WorldWithBallSpin = Matrix.CreateFromAxisAngle(BallSpinAxis, BallSpinAngle) * World;
            LightPos = position + new Vector3(0, 30, -30);

            //WorldWithBallSpin=Matrix.CreateRotationX(DeltaX) * Matrix.CreateRotationZ(DeltaZ) * World;
        }
    }
}