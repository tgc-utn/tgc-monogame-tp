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

        public Vector3 FrontDirection;

        public float RotationRadians;

        public float MovementSpeed { get; set; }
        public float RotationSpeed { get; set; }

        public Matrix BoatMatrix { get; set; }

        public bool playerMode = false;
        private Matrix waterMatrix { get; set; }

        //private Matrix[] BoneMatrix;

        public BoundingSphere BoatBox { get; set; }
        //public BoundingBox BoatBox { get; set; }

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

            //BoatBox = new BoundingBox(Vector3.Transform(-Vector3.One * 0.5f, BoatMatrix), Vector3.Transform(Vector3.One * 0.5f, BoatMatrix));
            BoatBox = new BoundingSphere(Position, 50);
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

            float WavePosY = GetWaterPositionY(time);
            Position = new Vector3(Position.X, WavePosY, Position.Z);
            BoatMatrix = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateTranslation(Position);
            FrontDirection = -new Vector3((float)Math.Sin(RotationRadians), 0.0f, (float)Math.Cos(RotationRadians));
            //BoatBox = new BoundingBox(Vector3.Transform(-Vector3.One * 0.5f, BoatMatrix), Vector3.Transform(Vector3.One * 0.5f, BoatMatrix));
            BoatBox = new BoundingSphere(Position, 50);
        }

        float frac(float val)
        {
            return val - MathF.Floor(val);
        }

        public Vector3 createWave(float time, float steepness, float numWaves, Vector2 waveDir, float waveAmplitude, float waveLength, float peak, float speed, Vector3 position)
        {
            Vector3 wave = new Vector3(0.0f, 0.0f, 0.0f);

            float spaceMult = (float)(2 * 3.14159265359 / waveLength);
            float timeMult = (float)(speed * 2 * 3.14159265359 / waveLength);

            Vector2 posXZ = new Vector2(position.X, position.Z);
            wave.X = waveAmplitude * steepness * waveDir.X * MathF.Cos(Vector2.Dot(posXZ, waveDir) * spaceMult + time * timeMult);
            wave.Y = 2 * waveAmplitude * MathF.Pow((MathF.Sin(Vector2.Dot(posXZ, waveDir) * spaceMult + time * timeMult) + 1) / 2, peak);
            wave.Z = waveAmplitude * steepness * waveDir.Y * MathF.Cos(Vector2.Dot(posXZ, waveDir) * spaceMult + time * timeMult);
            return wave;
        }

        public float GetWaterPositionY(float time)
        {   
            Vector3 worldPosition = Position;

            //createWave(float steepness, float numWaves, float2 waveDir, float waveAmplitude, float waveLength, float peak, float speed, float4 position) {

            Vector3 wave1 = createWave(time, 4, 5, new Vector2( 0.5f, 0.3f), 40, 160, 3, 10, worldPosition);
            Vector3 wave2 = createWave(time, 8, 5, new Vector2(0.8f, -0.4f), 12, 120, 1.2f, 20, worldPosition);
            Vector3 wave3 = createWave(time, 4, 5, new Vector2(0.3f, 0.2f), 2, 90, 5, 25, worldPosition);
            Vector3 wave4 = createWave(time, 2, 5, new Vector2(0.4f, 0.25f), 2, 60, 15, 15, worldPosition);
            Vector3 wave5 = createWave(time, 6, 5, new Vector2(0.1f, 0.8f), 20, 250, 2, 40, worldPosition);

            Vector3 wave6 = createWave(time, 4, 5, new Vector2(-0.5f, -0.3f), 0.5f, 8, 0.2f, 4, worldPosition);
            Vector3 wave7 = createWave(time, 8, 5, new Vector2(-0.8f, 0.4f), 0.3f, 5, 0.3f, 6, worldPosition);

            worldPosition = (wave1 + wave2 + wave3 + wave4 + wave5 + wave6 * 0.4f + wave7 * 0.6f) / 6;
            return (float)worldPosition.Y;
        }
    }
}
