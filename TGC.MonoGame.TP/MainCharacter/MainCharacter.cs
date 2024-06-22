using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Camera;
using TGC.MonoGame.TP.Stages;
using TGC.MonoGame.TP.Collisions;

namespace TGC.MonoGame.TP.MainCharacter
{
    public class Character
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

        Vector3 Position;
        Vector3 Velocity;
        Vector3 Acceleration = Vector3.Zero;
        Quaternion Rotation = Quaternion.Identity;
        Vector3 RotationAxis = Vector3.UnitY;
        float RotationAngle = 0f;

        // Colisiones
        public BoundingSphere EsferaBola { get; set; }
        public bool OnGround { get; set; }
        public Stage ActualStage;
        // Colisiones

        Vector3 BallSpinAxis=Vector3.UnitX;
        float BallSpinAngle=0f;
        Matrix WorldWithBallSpin;
        //float BallPitch=0f;
        //float BallRoll=0f;

        Vector3 LightPos{get;set;}
        public Matrix Spin;
        public Character(ContentManager content, Stage stage)
        {
            Content = content;
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

            /* 
            // Colisiones: ¿van en lo principal?
            int index = 0;
            for (; index < ActualStage.Colliders.Count; index++)
                ActualStage.Colliders[index] = BoundingVolumesExtensions.FromMatrix(ActualStage.StairsWorld[index]);

             // Instantiate a BoundingBox for the Box
             ActualStage.Colliders[index] = BoundingVolumesExtensions.FromMatrix(ActualStage.BoxWorld);
             index++;
             // Instantiate a BoundingBox for the Floor. Note that the height is almost zero
             ActualStage.Colliders[index] = new BoundingBox(new Vector3(-200f, -0.001f, -200f), new Vector3(200f, 0f, 200f));
            // Colisiones
            */

            // Bounding Sphere asociado a la bola principal
            EsferaBola = new BoundingSphere(Position, 10f);

            // Apply the effect to all mesh parts
            Sphere.Meshes.FirstOrDefault().MeshParts.FirstOrDefault().Effect = Effect;
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
            ProcessCollision(Velocity);

            // Verificar colisiones con los colliders del escenario actual
            //ActualStage.CheckCollisions(Position);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            var worldView = WorldWithBallSpin * view;
            Effect.Parameters["matWorld"].SetValue(WorldWithBallSpin);
            Effect.Parameters["matWorldViewProj"].SetValue(worldView * projection);
            Effect.Parameters["matInverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(WorldWithBallSpin)));
            //Effect.Parameters["lightPosition"].SetValue(new Vector3(25, 180, -800));
            //Effect.Parameters["lightPosition"].SetValue(Position + new Vector3(0, 60, 0));
            Effect.Parameters["lightPosition"].SetValue(LightPos);
            Effect.Parameters["lightColor"].SetValue(new Vector3(253, 251, 211));

            //Game.Gizmos.DrawSphere(World, Vector3.One*20, Color.Red);
            
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



        private void ProcessCollision(Vector3 scaledVelocity)
        {
            // Si la esfera tiene velocidad vertical
            if (scaledVelocity.Y == 0f)
                return;


            // Empieza moviendo la esfera
            BoundingSphere NuevaEsfera = EsferaBola; //tuvimos que crear una variable auxiliar porque no podemos modificarla directamente
            NuevaEsfera.Center += Vector3.Up * scaledVelocity.Y;
            EsferaBola = NuevaEsfera;
            // Set the OnGround flag on false, update it later if we find a collision
            OnGround = false;


            // Collision detection
            var collided = false;
            var foundIndex = -1;
            for (var index = 0; index < ActualStage.Colliders.Count; index++)
            {
                if (!EsferaBola.Intersects(ActualStage.Colliders[index]).Equals(BoxCylinderIntersection.Intersecting))
                    continue;
                
                // If we collided with something, set our velocity in Y to zero to reset acceleration
                Velocity = new Vector3(Velocity.X, 0f, Velocity.Z);

                // Set our index and collision flag to true
                // The index is to tell which collider the Robot intersects with
                collided = true;
                foundIndex = index;
                break;
            }


            // We correct based on differences in Y until we don't collide anymore
            // Not usual to iterate here more than once, but could happen
            while (collided)
            {
                var collider = ActualStage.Colliders[foundIndex];
                var colliderY = BoundingVolumesExtensions.GetCenter(collider).Y;
                var sphereY = EsferaBola.Center.Y;
                var extents = BoundingVolumesExtensions.GetExtents(collider);

                float penetration;
                // If we are on top of the collider, push up
                // Also, set the OnGround flag to true
                if (sphereY > colliderY)
                {
                    penetration = colliderY + extents.Y - sphereY + EsferaBola.Radius;
                    OnGround = true;
                }

                // If we are on bottom of the collider, push down
                else
                    penetration = -sphereY - EsferaBola.Radius + colliderY - extents.Y;

                // Move our Cylinder so we are not colliding anymore
                NuevaEsfera = EsferaBola;
                NuevaEsfera.Center += Vector3.Up * penetration;
                EsferaBola = NuevaEsfera;
                collided = false;

                // Check for collisions again
                for (var index = 0; index < ActualStage.Colliders.Count; index++)
                {
                    if (!EsferaBola.Intersects(ActualStage.Colliders[index]).Equals(BoxCylinderIntersection.Intersecting))
                        continue;

                    // Iterate until we don't collide with anything anymore
                    collided = true;
                    foundIndex = index;
                    break;
                }
            }
            
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

        private Vector2 pastMousePosition=Vector2.Zero;
        private float MouseSensitivity=0.3f;
        private void ProcessMovement(GameTime gameTime) 
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            

            var directionX = new Vector3();
            var directionY = new Vector3();
            var directionZ = new Vector3();
            
            bool salto = false;
            //bool onGround = true;
            float speed = 100;
            // Capturar Input teclado
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                Acceleration += Vector3.Transform(Vector3.UnitX * -speed, Rotation);
            }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                Acceleration += Vector3.Transform(Vector3.UnitX * -speed, Rotation) * (- 1);
            }
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                Acceleration += Vector3.Transform(Vector3.UnitZ * speed, Rotation);
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                Acceleration += Vector3.Transform(Vector3.UnitZ * speed, Rotation) * (-1);
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Space) && Velocity.Y == 0f)
            {
                Velocity += Vector3.Up * speed;
                salto = true;
                ProcessCollision(Velocity);
            }

            Vector3 gravity = Vector3.Zero;

            Vector3 SpinVelocity=Velocity;


            BallSpinAngle += Velocity.Length()*elapsedTime / (MathHelper.Pi*12.5f);
            BallSpinAxis = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, Velocity));
            //DeltaX+=elapsedTime*Velocity.X/MathHelper.TwoPi;
            //DeltaZ+=elapsedTime*Velocity.Z/MathHelper.TwoPi;
            //Spin*=Matrix.CreateFromAxisAngle(BallSpinAxis, BallSpinAngle);
            //BallSpinAxis.X=Math.Abs(BallSpinAxis.X);
            //BallSpinAxis.Z=Math.Abs(BallSpinAxis.Z);
            
            //if(Acceleration==Vector3.Zero && Velocity!=Vector3.Zero) Velocity *= (1-(elapsedTime/2));
            if(Acceleration==Vector3.Zero || Vector3.Dot(Acceleration, Velocity)<0) Velocity *= (1-(elapsedTime));
            if(Velocity==Vector3.Zero) BallSpinAxis = Vector3.UnitZ;

            Rotation = Quaternion.CreateFromAxisAngle(RotationAxis, RotationAngle);

            directionX = Vector3.Transform(Vector3.UnitX, Rotation);
            directionY = Vector3.Transform(Vector3.UnitY, Rotation);
            directionZ = Vector3.Transform(Vector3.UnitZ, Rotation);

            
            //if(Position.Y <= 25f)
            if(OnGround==true)
            {
                gravity = new Vector3(0f, 0f, 0f);
                if (!salto)
                {
                    Velocity = new Vector3(Velocity.X, 0f, Velocity.Z);
                }
            }
            else
            {
                gravity = new Vector3(0f, -100f, 0f);
            }

            Velocity += (Acceleration + gravity) * elapsedTime;
            Position += (directionX + directionY + directionZ) * Velocity * elapsedTime * 0.5f;


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