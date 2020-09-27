using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.InsaneGames.Obstacles
{
    class Sawhorse : Obstacle
    {
        private const string ModelName = "obstacles/sawhorse/sawhorse";
        static private Model Model;
        static private Matrix Misalignment;
        private Matrix SpawnPoint;

        public Sawhorse(Matrix spawnPoint, Matrix? scaling = null)
        {
            if (Model is null)
            {
                Misalignment = Matrix.CreateTranslation(0, 0, 0);
            }
            SpawnPoint = Misalignment *
                        scaling.GetValueOrDefault(Matrix.CreateScale(0.4f)) *
                        spawnPoint;
        }
        public override void Load()
        {
            if (Model is null)
                Model = ContentManager.Instance.LoadModel(ModelName);
        }
        public override void Draw(GameTime gameTime)
        {
            Model.Draw(SpawnPoint, Game.Camera.View, Game.Camera.Projection);
        }
    }
}
