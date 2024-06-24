using System;
using System.Linq;
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

       private List<Entity> myEntities;

        // Colisiones
        public BoundingSphere EsferaBola { get; set; }
        public bool OnGround { get; set; }
        public Stage ActualStage;
        // Colisiones

        Vector3 BallSpinAxis=Vector3.UnitX;
        float BallSpinAngle=0f;
        Matrix WorldWithBallSpin;

        Vector3 LightPos{get;set;}
        public Matrix Spin;
        public Character(ContentManager content, Stage stage, List<Entity> entities)
        {
            Content = content;
            myEntities = entities;
            Spin= Matrix.CreateFromAxisAngle(Vector3.UnitZ, 0);

            ActualStage = stage;

            InitializeEffect();
            InitializeSphere(stage.CharacterInitialPosition);
            InitializeTextures();
            InitializeLight();
        }
        void InitializeLight(){
            LightPos=Position+ new Vector3(0,10,0);
        }

        private void InitializeSphere(Vector3 initialPosition)
        {
            // Got to set a texture, else the translation to mesh does not map UV
            Sphere = Content.Load<Model>(ContentFolder3D + "geometries/sphere");

            Position = initialPosition;
            World = Scale * Matrix.CreateTranslation(Position);
            WorldWithBallSpin=World;

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
            foreach (Entity entity in myEntities)
            {
                if (entity is CubePrimitive)
                {
                    CubePrimitive entityAux = (CubePrimitive)entity;
                    Ray tempRay = new Ray(pos, -Vector3.Up);
                    float? tempDist = tempRay.Intersects(entityAux.BoundingCube);

                    if (dist > tempDist)
                    {
                        dist = (float)tempDist;
                    }
                }
            }

            return dist;
        }

        public bool IsOnGround(Vector3 pos)
        {
            GeometricPrimitive collidedEntity = null;
            foreach (Entity entity in myEntities)
            {
                if (entity is CubePrimitive)
                {
                    CubePrimitive entityAux = (CubePrimitive)entity;
                    Ray tempRay = new Ray(pos, -Vector3.Up);
                    float? dist = tempRay.Intersects(entityAux.BoundingCube);

                    if (dist.HasValue && dist < 10.2f)
                    {
                        collidedEntity = (CubePrimitive)entity;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsColliding()
        {
            foreach (Entity entity in myEntities)
            {
                if (entity is CubePrimitive)
                {
                    CubePrimitive entityAux = (CubePrimitive)entity;
                    if (EsferaBola.Intersects(entityAux.BoundingCube))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void ProcessCollision_Old(Vector3 LocalVelocity, float deltaTime)
        {
            Vector3 oldPosition = Position;
            Vector3 movement = LocalVelocity * deltaTime * deltaTime * 0.5f;
            Vector3 newPosition = oldPosition + movement;

            UpdateBBSphere(newPosition);
            int steps = 1000;
            bool neverFound = true;
            foreach (Entity entity in myEntities)
            {
                if (entity is CubePrimitive)
                {
                    CubePrimitive entityAux = (CubePrimitive)entity;
                    if (EsferaBola.Intersects(entityAux.BoundingCube))
                    {
                        neverFound = false;
                        if (IsOnGround(newPosition))
                        {
                           //LocalVelocity = new Vector3(LocalVelocity.X, 0.0f, LocalVelocity.Z);
                        }

                        bool intersecting = true;
                        float moveFactor = 1.0f;
                        while (intersecting && steps > 0)
                        {
                            steps -= 1;
                            movement = LocalVelocity * deltaTime * deltaTime * 0.5f;
                            newPosition = oldPosition + movement * moveFactor;
                            UpdateBBSphere(newPosition);
                            intersecting = EsferaBola.Intersects(entityAux.BoundingCube);
                            moveFactor = moveFactor / 2.0f;
                        }

                        Position = newPosition;
                        UpdateBBSphere(Position);
                    }
                }
            }

            if (neverFound)
            {
                Position = newPosition;
                UpdateBBSphere(Position);
            }
        }

        public void ProcessCollision(Vector3 LocalVelocity, float deltaTime)
        {
            Vector3 oldPosition = Position;
            Vector3 movement = LocalVelocity * deltaTime * deltaTime * 0.5f;
            Vector3 newPosition = oldPosition + movement;

            UpdateBBSphere(newPosition);

            bool collisionDetected = false;

            foreach (Entity entity in myEntities)
            {
                if (entity is CubePrimitive)
                {
                    CubePrimitive entityAux = (CubePrimitive)entity;
                    if (EsferaBola.Intersects(entityAux.BoundingCube))
                    {
                        collisionDetected = true;

                        // Manejar la colisión
                        Vector3 collisionNormal = GetCollisionNormal(entityAux.BoundingCube);

                        // Resolver la posición después de la colisión
                        newPosition = ResolveCollision(oldPosition, newPosition, collisionNormal);

                        // Actualizar la esfera de colisión
                        UpdateBBSphere(newPosition);

                        // Puedes hacer más ajustes según tu lógica específica de juego
                        // Por ejemplo, ajustar la velocidad según la normal de la colisión
                        // LocalVelocity = ModifyVelocity(LocalVelocity, collisionNormal);
                    }
                }
            }

            // Si no hubo colisión, simplemente actualizar la posición
            if (!collisionDetected)
            {
                Position = newPosition;
                UpdateBBSphere(Position);
            }
        }

        private Vector3 GetCollisionNormal(BoundingBox boundingBox)
        {
            // Aquí obtienes la normal de la colisión basada en la caja de colisión
            // Puedes implementar la lógica según tus necesidades específicas
            // Por ejemplo, calcular la normal de la cara con la que colisiona la esfera
            // y devolverla como un vector normalizado
            // Este es un ejemplo simplificado, puede necesitar ajustes según tu escenario
            return Vector3.Normalize(Position - boundingBox.Min);
        }

        private Vector3 ResolveCollision(Vector3 oldPosition, Vector3 newPosition, Vector3 collisionNormal)
        {
            // Calcula el vector desde la posición anterior hacia la nueva posición
            Vector3 movementDirection = newPosition - oldPosition;

            // Calcula la distancia que se movió la bola antes de la colisión
            float movementDistance = movementDirection.Length();

            // Si no se movió, no hay colisión que resolver
            if (movementDistance == 0)
                return newPosition;

            // Normaliza la dirección del movimiento
            Vector3 movementDirectionNormalized = movementDirection / movementDistance;

            // Calcula la distancia a lo largo de la normal de la colisión
            float distanceAlongNormal = Vector3.Dot(collisionNormal, newPosition - oldPosition);

            // Si la bola se estaba moviendo hacia el objeto (distancia negativa a lo largo de la normal), la refleja
            if (distanceAlongNormal < 0)
            {
                // Calcula la parte de la velocidad que va en la dirección opuesta a la normal
                Vector3 velocityParallel = distanceAlongNormal * collisionNormal;

                // Calcula la parte de la velocidad que va en la dirección perpendicular a la normal
                Vector3 velocityPerpendicular = newPosition - oldPosition - velocityParallel;

                // Invierte la parte de la velocidad paralela para simular un rebote
                newPosition = oldPosition - velocityParallel + velocityPerpendicular;
            }

            // Retorna la nueva posición ajustada después de la resolución de la colisión
            return newPosition;
        }

        private Vector2 pastMousePosition=Vector2.Zero;
        private float MouseSensitivity=0.3f;

        private Vector3 GetVelocity()
        {
            return Velocity;
        }

        private void ProcessMovement(GameTime gameTime) 
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var directionX = new Vector3();
            var directionY = new Vector3();
            var directionZ = new Vector3();
            
            float speed = 100f;

            // Capturar Input teclado
            var keyboardState = Keyboard.GetState();
            
            // Procesamiento del movimiento horizontal
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                Acceleration += Vector3.Transform(Vector3.UnitX * -speed, Rotation);
            }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                Acceleration += Vector3.Transform(Vector3.UnitX * speed, Rotation);
            }
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                Acceleration += Vector3.Transform(Vector3.UnitZ * speed, Rotation);
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                Acceleration += Vector3.Transform(Vector3.UnitZ * -speed, Rotation);
            }

            Acceleration += new Vector3(0f, -100f, 0f);

            //Procesamiento del movimiento vertical
            float distGround = DistanceToGround(Position);
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && (distGround < 12.5f || IsColliding()))
            {
                Velocity += Vector3.Up * speed * 100;
            }

            BallSpinAngle += Velocity.Length()*elapsedTime / (MathHelper.Pi*12.5f);
            BallSpinAxis = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, Velocity));

            if (Acceleration == Vector3.Zero || Vector3.Dot(Acceleration, Velocity) < 0)
            {
                Velocity *= (1 - (elapsedTime));
            }

            if(Velocity==Vector3.Zero) BallSpinAxis = Vector3.UnitZ;

            Rotation = Quaternion.CreateFromAxisAngle(RotationAxis, RotationAngle);

            directionX = Vector3.Transform(Vector3.UnitX, Rotation);
            directionY = Vector3.Transform(Vector3.UnitY, Rotation);
            directionZ = Vector3.Transform(Vector3.UnitZ, Rotation);

            Velocity += Acceleration;

            ProcessCollision(Velocity * (directionX + directionY + directionZ), elapsedTime);
            
            MoveTo(Position);

            Acceleration = Vector3.Zero;
        }
        //float DeltaX, DeltaZ;
        public void MoveTo(Vector3 position)
        {
            World = Scale * Matrix.CreateTranslation(position);
            WorldWithBallSpin=Matrix.CreateFromAxisAngle(BallSpinAxis, BallSpinAngle) * World;
            LightPos=Position+ new Vector3(0,30,-30);
            
            //WorldWithBallSpin=Matrix.CreateRotationX(DeltaX) * Matrix.CreateRotationZ(DeltaZ) * World;
        }
    }
}