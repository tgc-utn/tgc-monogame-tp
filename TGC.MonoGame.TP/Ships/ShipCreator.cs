using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Ships
{
    public class Ship
    {
        private TGCGame Game;
        private float time;

        private Model ShipModel { get; set; }

        public string ModelName;
        private Effect ShipEffect { get; set; }
        
        public string EffectName;

        public Texture2D ShipTexture;

        public string TextureName;
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 wavesRotation { get; set; }
        public Vector3 Scale { get; set; }
        public float Speed { get; set; }

        private Vector3 FrontDirection;

        private float RotationRadians;

        private float MovementSpeed { get; set; }
        private float RotationSpeed { get; set; }

        public Matrix BoatMatrix { get; set; }

        public bool playerMode = false;
        private Matrix waterMatrix { get; set; }


        //private Matrix[] BoneMatrix;



        public Ship(TGCGame game, Vector3 pos, Vector3 rot, Vector3 scale, float speed, string modelName, string effect, string textureName)
        {
            Game = game;
            Position = pos;
            Rotation = rot;
            Scale = scale;
            Speed = speed;
            ModelName = modelName;
            EffectName = effect;
            TextureName = textureName;
            MovementSpeed = 100.0f;
            RotationSpeed = 0.5f;
            FrontDirection = Vector3.Forward;
            RotationRadians = 0;

            BoatMatrix = Matrix.Identity * Matrix.CreateScale(scale) * Matrix.CreateRotationY(rot.Y) * Matrix.CreateTranslation(pos);
        }
        public void LoadContent()
        {
            ShipModel = Game.Content.Load<Model>(TGCGame.ContentFolder3D + ModelName);
            ShipEffect = Game.Content.Load<Effect>(TGCGame.ContentFolderEffects + EffectName);
            ShipTexture = Game.Content.Load<Texture2D>(TGCGame.ContentFolderTextures + TextureName);

        }


        public void Draw()
        {
            ShipEffect.Parameters["ModelTexture"].SetValue(ShipTexture);
            DrawModel(ShipModel, BoatMatrix, ShipEffect);
        }

        private void DrawModel(Model geometry, Matrix transform, Effect effect)
        {
            foreach (var mesh in geometry.Meshes)
            {
                effect.Parameters["World"].SetValue(transform);
                effect.Parameters["View"].SetValue(Game.CurrentCamera.View);
                effect.Parameters["Projection"].SetValue(Game.CurrentCamera.Projection);
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
                mesh.Draw();
            }
        }
        public void Update(GameTime gameTime)
        {
            // Esto es el tiempo que transcurre entre update y update (promedio 0.0166s)
            float elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Esto es el tiempo total transcurrido en el tiempo, siempre se incrementa
            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            float yPos = GetWaterPositionY(Position.X, Position.Z);
            Position = new Vector3(Position.X, yPos, Position.Z);
            BoatMatrix = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateTranslation(Position);
            FrontDirection = -new Vector3((float)Math.Sin(RotationRadians), 0.0f, (float)Math.Cos(RotationRadians));
            //FrontDirection = -new Vector3(Rotation.X, 0, Rotation.Z);

            if (playerMode)
            {
                ProcessKeyboard(elapsedTime);
            }
        }

        float frac(float val)
        {
            return val - MathF.Floor(val);
        }

        public float GetWaterPositionY(float xPosition, float zPosition)
        {
            float fade = MathF.Min(MathF.Cos(time * 0.3f) + 0.5f, 1f);
            fade = MathF.Max(fade, 0.15f);


            float speed = 0.05f;
            float offset = 10f;
            float radius = 3f;
            var worldPosition = Position;

            var posY = 0.4 * MathF.Cos(((float)(xPosition * 0.0075f) * .5f + time * speed) * offset) * radius * 0.5f;
            posY += (1 - frac(time * 0.1f)) * frac(time * 0.1f) * 0.1f * MathF.Cos((-(float)(zPosition * 0.0075f) + time * speed * 1.3f) * offset) * radius;

            posY *= 3f + (1 - fade) * 12f;

            var wavetan1 = Vector3.Normalize(new Vector3(1f,
                0.4f * MathF.Cos(((float)(xPosition) * .5f + time * speed) * offset) * radius * 0.5f
                , 0f));

            var wavetan2 = Vector3.Normalize(new Vector3(0,
                (1 - frac(time * 0.1f)) * frac(time * 0.1f) * 0.1f * MathF.Cos((-(float)(zPosition) + time * speed * 1.3f) * offset) * radius,
                1));

            worldPosition = new Vector3(xPosition, (float)(posY) * 2, zPosition);

            Position = worldPosition;

            var waterNormal = Vector3.Normalize(Vector3.Cross(wavetan2, wavetan1));

            wavesRotation = Rotation - Vector3.Dot(Rotation, waterNormal) * waterNormal;

            waterMatrix = Matrix.CreateLookAt(Vector3.Zero, wavesRotation, waterNormal);

            return (float)posY;
        }

        private void ProcessKeyboard(float elapsedTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Game.Exit();
            }

            //var currentMovementSpeed = MovementSpeed;

            if (keyboardState.IsKeyDown(Keys.W))
            {
                MoveForward(MovementSpeed * elapsedTime);
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                MoveBackwards(MovementSpeed * elapsedTime);
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                RotateRight(RotationSpeed * elapsedTime);
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                RotateLeft(RotationSpeed * elapsedTime);
            }


        }

        private void MoveForward(float amount)
        {
            Position += FrontDirection * amount;
        }
        private void MoveBackwards(float amount)
        {
            MoveForward(-amount);
        }
        private void RotateRight(float amount)
        {
            Rotation = new Vector3(Rotation.X, Rotation.Y + amount, Rotation.Z);
            RotationRadians += amount;
        }
        private void RotateLeft(float amount)
        {
            RotateRight(-amount);
        }
    }
}