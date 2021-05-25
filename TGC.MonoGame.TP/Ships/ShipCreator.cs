using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Ships
{
    public class Ship
    {
        private TGCGame Game;
        private Model ShipModel { get; set; }

        public string ModelName;
        private Effect ShipEffect { get; set; }
        
        public string EffectName;

        public Texture2D ShipTexture;

        public string TextureName;
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public float Speed { get; set; }

        private Vector3 FrontDirection;

        private float PlayerRotation;

        private float MovementSpeed { get; set; }
        private float RotationSpeed { get; set; }

        public Matrix PlayerBoatMatrix { get; set; }

        public bool playerMode = false;
        private Matrix waterMatrix { get; set; }

        private Matrix[] BoneMatrix;

        // Son Rays que se usan para calcular la altura del agua en ese punto
        // Hay un ray en la punta de adelante del barco y otro atras, para poder saber la inclinacion
        float FrontRayOffset;
        float BackRayOffset;
        private Model asd1;
        private Model asd2;


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
            PlayerRotation = 0;
            PlayerBoatMatrix = Matrix.Identity * Matrix.CreateScale(scale) * Matrix.CreateRotationY(rot.Y) * Matrix.CreateTranslation(pos);

            FrontRayOffset = -50;
            BackRayOffset = 50;
        }
        public void LoadContent()
        {
            ShipModel = Game.Content.Load<Model>(TGCGame.ContentFolder3D + ModelName);
            ShipEffect = Game.Content.Load<Effect>(TGCGame.ContentFolderEffects + EffectName);
            ShipTexture = Game.Content.Load<Texture2D>(TGCGame.ContentFolderTextures + TextureName);

            BoneMatrix = new Matrix[ShipModel.Bones.Count];
            ShipModel.CopyAbsoluteBoneTransformsTo(BoneMatrix);

            asd1 = Game.Content.Load<Model>(TGCGame.ContentFolder3D + "tgc-logo/tgc-logo");
            asd2 = Game.Content.Load<Model>(TGCGame.ContentFolder3D + "tgc-logo/tgc-logo");

        }


        public void Draw()
        {
            ShipEffect.Parameters["ModelTexture"].SetValue(ShipTexture);
            DrawModel(ShipModel, PlayerBoatMatrix, ShipEffect);
            //DrawModel(ShipModel, Matrix.CreateScale(Scale) * Matrix.CreateRotationY((float)PlayerRotation) * Matrix.CreateTranslation(Position), ShipEffect);
            DrawModel(asd1, PlayerBoatMatrix * Matrix.CreateTranslation(0, 50, 0), ShipEffect);


            
            //DrawModel(ShipModel, Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position), ShipEffect);
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
            PlayerBoatMatrix = Matrix.CreateScale(Scale) * Matrix.CreateRotationY((float)PlayerRotation) * Matrix.CreateTranslation(Position);
            FrontDirection = - new Vector3((float)Math.Sin(PlayerRotation), 0.0f, (float)Math.Cos(PlayerRotation));

            if (playerMode)
            {
                var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                ProcessKeyboard(elapsedTime);
            }
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
            PlayerRotation += amount;
        }
        private void RotateLeft(float amount)
        {
            RotateRight(-amount);
        }
    }
}